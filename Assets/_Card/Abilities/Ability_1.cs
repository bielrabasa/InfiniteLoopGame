//
// on die: deals 2 more damage to the enemy card or hero if dies 
//

public class Ability_1 : Ability
{
    const int damage = 2;

    public override void OnDie()
    {
        isDying = true;

        if (!other.abilityScript.isDying)
        {
            other.hp -= damage;
            Particle.InstanceParticle(Particle.ParticleType.RECEIVE_DAMAGE, damage, makeDamageParticleOffset, other.transform);

            CheckKillingOther();
        }
        
        //Do everything it does normally
        base.OnDie();
    }
}
