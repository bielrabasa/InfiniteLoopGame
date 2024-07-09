//
// on die: deals 2 more damage to the enemy card or hero if dies 
//

public class Ability_1 : Ability
{
    public override void OnDie()
    {
        if (!other.abilityScript.isDying)
        {
            other.hp -= 2;

            CheckKillingOther();
        }
        
        //Do everything it does normally
        base.OnDie();
    }
}
