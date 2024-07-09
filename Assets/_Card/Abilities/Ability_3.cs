//
// on attack: if fights an enemy card, deals damage to all the cards in that layer 
//

public class Ability_3 : Ability
{
    protected override void Attack()
    {
        for(int i = 0; i < MapState.COLUMNS; i++)
        {
            if (MapState.cardPositions[i, myCurrentPosition.y] == null) continue;

            //Attacks to every card on layer
            other = MapState.cardPositions[i, myCurrentPosition.y].GetComponent<CardValues>();
            base.Attack();
            other.UpdateVisuals();
        }

        //Set other to the correct card again
        other = MapState.cardPositions[myCurrentPosition.x, myCurrentPosition.y].GetComponent<CardValues>();
    }
}
