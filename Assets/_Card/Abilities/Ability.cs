using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public CardValues me;
    public Vector2Int myStartPosition;
    Vector2Int myCurrentPosition;

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

            //TODO: if killed the enemy and i'm not dead, continue, else break
        }

        //TODO: go back to myStartingPosition
    }

    protected virtual void FullEncounter()
    {
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
        //TODO: destroy card and set MapState position to null

        Debug.Log("CardDied!");
    }
}
