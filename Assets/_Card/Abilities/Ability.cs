using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public CardValues me;
    public Vector2Int myStartPosition;
    Vector2Int myCurrentPosition;

    public bool isDying = false;

    private CardValues other;

    public void MakeCardAttackSequence()
    {

        //TODO: All about passing through layers...

        //After reaching mid-field
        myCurrentPosition = myStartPosition;
        myCurrentPosition.y = MapState.ROWS / 2 + (MapState.bottomPlayerAtacking ? -1 : 0);

        //Forward attacking enemy cards
        for(int i = 0; i < me.range; i++) 
        {
            //TODO: move to in front of myPosition (to encounter or just to advance)
            OnGoing();

            //If there is a card -> attack it!
            if (MapState.cardPositions[myCurrentPosition.x, myCurrentPosition.y] != null)
            {
                other = MapState.cardPositions[myCurrentPosition.x, myCurrentPosition.y].GetComponent<CardValues>();
                FullEncounter();
            }

            //If I'm dead stop or other is alive stop
            if (isDying || !other.abilityScript.isDying) break;

            //Continue attacking
            myCurrentPosition.y += (MapState.bottomPlayerAtacking ? -1 : +1); //TODO on top
        }

        //TODO: go back to myStartingPosition
        if (!isDying) Debug.Log("Returning!");
    }

    protected virtual void FullEncounter()
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

    //After the card is placed randomly
    public virtual void OnPlay() { }

    //When the card starts moving (towards enemies)
    protected virtual void OnGoing() { }

    //When the card dies either attacking or defending
    public virtual void OnDie() 
    {
        isDying = true;

        MapState.cardPositions[myStartPosition.x, myStartPosition.y] = null;
        Destroy(gameObject);
    }
}
