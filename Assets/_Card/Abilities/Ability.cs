using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ability : MonoBehaviour
{
    //Constants
    Vector3 attackPositionOffset = new Vector3(0, 0.5f, 0f);
    const float timeAfterEncounter = 0.2f;
    const float timeOnGoing = 0.08f;
    const float timeOnReturning = 0.05f;

    public CardValues me;
    public Vector2Int myStartPosition;
    protected Vector2Int myCurrentPosition;

    public bool isDying = false;

    public CardValues other;

    //---------------------------------

    public IEnumerator MakeCardAttackSequence()
    {
        //Getting to mid-field
        OnPassingThroughLayers();

        //After reaching mid-field
        myCurrentPosition = myStartPosition;
        myCurrentPosition.y = MapState.ROWS / 2 + (MapState.bottomPlayerAtacking ? -1 : 0);

        //Forward attacking enemy cards
        for(int i = 0; i < me.tempRange; i++) 
        {
            //Attack HERO
            if(i == 3)
            {
                yield return OnGoing(true);
                AttackHero();
                break;
            }

            //If there is a card -> attack it!
            if (MapState.cardPositions[myCurrentPosition.x, myCurrentPosition.y] != null)
            {
                //Go to the desired position
                yield return OnGoing();

                other = MapState.cardPositions[myCurrentPosition.x, myCurrentPosition.y].GetComponent<CardValues>();
                yield return FullEncounter();

                //If I'm dead stop or other is alive stop
                if (isDying || !other.abilityScript.isDying) break;
            }

            //Continue attacking
            myCurrentPosition.y += (MapState.bottomPlayerAtacking ? -1 : +1);
        }

        //Return to start position
        if (!isDying) yield return OnReturning();
    }

    protected virtual IEnumerator FullEncounter()
    {
        Attack();

        ReceiveAttack();

        //Die?
        if(me.hp <= 0) OnDie();

        //Kill?
        CheckKillingOther();

        //Update Card Visual Values
        if (!isDying) me.UpdateVisuals();
        if (!other.abilityScript.isDying) other.UpdateVisuals();

        yield return new WaitForSeconds(timeAfterEncounter);
    }

    //---------------------------------

    public virtual void OnStartPlayerTurn() { }
    public virtual void OnEndPlayerTurn() 
    {        
        me.ResetTemporalValues();
        me.UpdateVisuals();
    }

    public virtual void OnStartEnemyTurn() { }
    public virtual void OnEndEnemyTurn() 
    {
        me.ResetTemporalValues();
        me.UpdateVisuals();
    }

    //---------------------------------

    //When the card makes damage to the other card
    protected virtual void Attack()
    {
        other.hp -= me.tempDamage;
    }

    //When the cards receive damage
    protected virtual void ReceiveAttack()
    {
        me.hp -= other.tempDamage;
    }

    //When the card attacks specifically the Hero
    protected virtual void AttackHero()
    {
        if (MapState.bottomPlayerAtacking)  MapState.TopHeroHP -= me.tempDamage;
        else                                MapState.BottomHeroHP -= me.tempDamage;
    }

    //After the start sequence, when all the layer perks are applied
    protected virtual void OnPassingThroughLayers()
    {
        myCurrentPosition = myStartPosition;
        myCurrentPosition.y = MapState.ROWS / 2 + (MapState.bottomPlayerAtacking ? -1 : 0);

        if (MapState.bottomPlayerAtacking)
        {
            for (int i = myStartPosition.y; i > MapState.ROWS / 2 - 1; i--)
            {
                ApplyLayerPerk(MapState.Layer[i]);
            }
        }
        else
        {
            for (int i = myStartPosition.y; i < MapState.ROWS / 2; i++)
            {
                ApplyLayerPerk(MapState.Layer[i]);
            }
        }
    }

    void ApplyLayerPerk(MapState.LayerPerks layerPerk)
    {
        switch (layerPerk)
        {
            case MapState.LayerPerks.NONE:
                Debug.Log("LayerPerk not set!");
                break;
            case MapState.LayerPerks.RANGE: ApplyRangePerk(); break;
            case MapState.LayerPerks.DAMAGE: ApplyDamagePerk(); break;
            case MapState.LayerPerks.MANA: ApplyManaPerk(); break;
        }
    }
    protected virtual void ApplyRangePerk()
    {
        me.tempRange += 1;
        me.UpdateVisuals();
    }
    protected virtual void ApplyDamagePerk()
    {
        me.tempDamage += 1;
        me.UpdateVisuals();
    }
    protected virtual void ApplyManaPerk()
    {
        //TODO: do it good
        //Debug.Log("Applying Mana!");
    }

    //Every time a card moves towards an enemy
    protected virtual IEnumerator OnGoing(bool toHero = false) 
    {
        Vector3 finalPos;
        if (toHero)
        {
            finalPos = MapState.bottomPlayerAtacking? MapState.topHeroPosition : MapState.bottomHeroPosition;
        }
        else
        {
            finalPos = MapState.boardPositions[myCurrentPosition.x, myCurrentPosition.y];
        }

        Vector3 attackOffset = attackPositionOffset;
        if (MapState.bottomPlayerAtacking) attackOffset.z = -attackOffset.z;
        finalPos += attackOffset;

        Vector3 velocity = Vector3.zero;
        while (Vector3.Distance(transform.position, finalPos) > 0.01f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref velocity, timeOnGoing);
            yield return null;
        }
    }

    //When the card returns to its starting position after attacking
    protected virtual IEnumerator OnReturning()
    {
        Vector3 finalPos = MapState.boardPositions[myStartPosition.x, myStartPosition.y];
        Vector3 velocity = Vector3.zero;
        while (Vector3.Distance(transform.position, finalPos) > 0.01f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref velocity, timeOnReturning);
            yield return null;
        }
    }

    //When the card dies either attacking or defending
    public virtual void OnDie() 
    {
        isDying = true;

        MapState.cardPositions[myStartPosition.x, myStartPosition.y] = null;
        Destroy(gameObject);
    }

    //Check killing other
    protected virtual void CheckKillingOther()
    {
        if (other.hp <= 0 && !other.abilityScript.isDying)
        {
            other.abilityScript.other = me;
            other.abilityScript.OnDie();
        }
    }

    //---------------------------------

    //After the card is placed randomly
    //public virtual void OnPlay() { }

    //In the start of the attacking sequence
    //protected virtual void OnStartSequence() { }
}
