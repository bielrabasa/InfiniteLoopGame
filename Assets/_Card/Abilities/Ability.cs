using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ability : MonoBehaviour
{
    //const 

    public CardValues me;
    public Vector2Int myStartPosition;
    Vector2Int myCurrentPosition;

    public bool isDying = false;

    private CardValues other;

    public IEnumerator MakeCardAttackSequence()
    {

        //TODO: All about passing through layers...

        //After reaching mid-field
        myCurrentPosition = myStartPosition;
        myCurrentPosition.y = MapState.ROWS / 2 + (MapState.bottomPlayerAtacking ? -1 : 0);

        //Forward attacking enemy cards
        for(int i = 0; i < me.range; i++) 
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
        Debug.Log("Attacking: " + myStartPosition + " On: " + myCurrentPosition);

        //TODO: attack animations & effects

        Attack();

        ReceiveAttack();

        //Die?
        if(me.hp <= 0) OnDie();

        if (other.hp <= 0)
        {
            other.abilityScript.other = me;
            other.abilityScript.OnDie();
        }

        //Update Card Visual Values
        if (!isDying) me.UpdateVisuals();
        if (other.abilityScript.isDying) other.UpdateVisuals();

        yield return new WaitForSeconds(0.2f);
    }

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

    //After the card is placed randomly
    public virtual void OnPlay() { }

    //When the card starts moving (towards enemies)
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

        finalPos.y += 0.5f;
        finalPos.z += MapState.bottomPlayerAtacking ? -0.5f : 0.5f;
        Vector3 velocity = Vector3.zero;
        while (Vector3.Distance(transform.position, finalPos) > 0.01f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref velocity, 0.1f);
            yield return null;
        }
    }

    protected virtual IEnumerator OnReturning()
    {
        Vector3 finalPos = MapState.boardPositions[myStartPosition.x, myStartPosition.y];
        Vector3 velocity = Vector3.zero;
        while (Vector3.Distance(transform.position, finalPos) > 0.01f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref velocity, 0.1f);
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
}
