using System.Collections;
using UnityEngine;

using DESIGN;

public class Ability : MonoBehaviour
{
    protected static Vector3 addManaParticleOffset = new Vector3(1, 3, 2.5f);
    protected static Vector3 addDamageParticleOffset = new Vector3(-1, 3, -2.5f);
    protected static Vector3 addRangeParticleOffset = new Vector3(0, 3, -2.5f);

    protected static Vector3 receiveDamageParticleOffset = new Vector3(2, 3, -2.5f);
    protected static Vector3 makeDamageParticleOffset = new Vector3(2, 3, -1f);

    //Constants
    Vector3 attackPositionOffset = new Vector3(0, 0.5f, 0.5f);

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
        AudioManager.SetSFX(AudioManager.SFX.MOVINGCARD);

        //After reaching mid-field
        myCurrentPosition = myStartPosition;
        myCurrentPosition.y = MapState.ROWS / 2 + (MapState.bottomPlayerAtacking ? -1 : 0);

        //Forward attacking enemy cards
        for (int i = 0; i < me.tempRange; i++) 
        {
            //Attack HERO
            if(i == 3)
            {
                yield return OnGoing(true);
                AttackHero();
                break;
            }

            //Go to the desired position
            yield return OnGoing();

            //If there is a card -> attack it!
            if (MapState.cardPositions[myCurrentPosition.x, myCurrentPosition.y] != null)
            {

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
        AudioManager.SetSFX(AudioManager.SFX.ATTACK);

        Attack();

        ReceiveAttack();

        //Die?
        if(me.hp <= 0) OnDie();

        //Kill?
        CheckKillingOther();

        //Update Card Visual Values
        if (!isDying) me.UpdateVisuals();
        if (!other.abilityScript.isDying) other.UpdateVisuals();

        yield return new WaitForSeconds(DESIGN_VALUES.timeAfterEncounter);
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
        Particle.InstanceParticle(Particle.ParticleType.RECEIVE_DAMAGE, me.tempDamage, makeDamageParticleOffset, other.transform);
    }

    //When the cards receive damage
    protected virtual void ReceiveAttack()
    {
        me.hp -= other.tempDamage;
        Particle.InstanceParticle(Particle.ParticleType.RECEIVE_DAMAGE, other.tempDamage, receiveDamageParticleOffset, transform);
    }

    //When the card attacks specifically the Hero
    protected virtual void AttackHero()
    {
        //Damage the other hero
        AudioManager.SetSFX(AudioManager.SFX.ATTACK);
        MapState.DamageHero(!MapState.bottomPlayerAtacking, me.tempDamage);
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
        Particle.InstanceParticle(Particle.ParticleType.ADD_RANGE, 1, addRangeParticleOffset, transform);
    }
    protected virtual void ApplyDamagePerk()
    {
        me.tempDamage += 1;
        me.UpdateVisuals();
        Particle.InstanceParticle(Particle.ParticleType.ADD_DAMAGE, 1, addDamageParticleOffset, transform);
    }
    protected virtual void ApplyManaPerk()
    {
        if (MapState.bottomPlayerAtacking)
        {
            if (++MapState.bottomMana > MapState.MAX_MANA) {
                MapState.bottomMana = MapState.MAX_MANA;
                //No mana added
                Particle.InstanceParticle(Particle.ParticleType.ADD_MANA, 0, addManaParticleOffset, transform);
                return;
            }
        }
        else if (++MapState.topMana > MapState.MAX_MANA)
        {
            MapState.topMana = MapState.MAX_MANA;
            //No mana added
            Particle.InstanceParticle(Particle.ParticleType.ADD_MANA, 0, addManaParticleOffset, transform);
            return;
        }

        Particle.InstanceParticle(Particle.ParticleType.ADD_MANA, 1, addManaParticleOffset, transform);
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
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref velocity, DESIGN_VALUES.timeOnGoing);
            yield return null;
        }
    }

    //When the card returns to its starting position after attacking
    protected virtual IEnumerator OnReturning()
    {
        AudioManager.SetSFX(AudioManager.SFX.MOVINGCARD);

        Vector3 finalPos = MapState.boardPositions[myStartPosition.x, myStartPosition.y];
        Vector3 velocity = Vector3.zero;
        while (Vector3.Distance(transform.position, finalPos) > 0.01f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref velocity, DESIGN_VALUES.timeOnReturning);
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
