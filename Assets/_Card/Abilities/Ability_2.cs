//
// ongoing: gives +1 damage to the cards in the same starting layer
//

public class Ability_2 : Ability
{
    public override void OnStartEnemyTurn()
    {
        BoostAllCardsInLine();
    }

    public override void OnStartPlayerTurn()
    {
        BoostAllCardsInLine();
    }

    void BoostAllCardsInLine()
    {
        for (int i = 0; i < MapState.COLUMNS; i++)
        {
            if (i == myStartPosition.x) continue;
            if(MapState.cardPositions[i, myStartPosition.y] == null) continue;

            //Boosts every card on layer
            CardValues friendCard = MapState.cardPositions[i, myStartPosition.y].GetComponent<CardValues>();
            friendCard.tempDamage += 1;
            friendCard.UpdateVisuals();
            Particle.InstanceParticle(Particle.ParticleType.ADD_DAMAGE, 1, addDamageParticleOffset, friendCard.transform);
        }
    }
}
