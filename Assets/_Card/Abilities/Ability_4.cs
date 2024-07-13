//
// ongoing: if passes through mana layer, generates 2 mana
//

public class Ability_4 : Ability
{
    protected override void ApplyManaPerk()
    {
        base.ApplyManaPerk();
        base.ApplyManaPerk();
    }
}
