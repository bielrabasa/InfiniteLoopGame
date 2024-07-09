using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// on die: deals 2 more damage to the enemy card or hero if dies 
//

public class Ability_1 : Ability
{
    public override void OnDie()
    {
        Debug.Log("Apply ondie ability");

        if (!other.abilityScript.isDying)
        {
            other.hp -= 2;

            CheckKillingOther();
        }
        
        isDying = true;

        MapState.cardPositions[myStartPosition.x, myStartPosition.y] = null;
        Destroy(gameObject);
    }
}
