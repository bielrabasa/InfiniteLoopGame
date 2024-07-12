//
// on attack: if fights an enemy card, deals damage to all the cards in that layer 
//

public class Ability_3 : Ability
{
    protected override void Attack()
    {
        for(int i = 0; i < MapState.COLUMNS; i++)
        {
            if (i == myCurrentPosition.x) continue;
            if (MapState.cardPositions[i, myCurrentPosition.y] == null) continue;

            //Attacks to every card on layer
            other = MapState.cardPositions[i, myCurrentPosition.y].GetComponent<CardValues>();
            base.Attack();
            CheckKillingOther();
            if(!other.abilityScript.isDying) other.UpdateVisuals();
        }

        //Finally the card in front
        other = MapState.cardPositions[myCurrentPosition.x, myCurrentPosition.y].GetComponent<CardValues>();
        base.Attack();
    }
}
