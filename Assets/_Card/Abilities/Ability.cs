using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    //After the card is placed randomly
    public virtual void OnPlay() { }

    //When the card starts moving (towards enemies)
    public virtual void OnGoing() { }

    //When the card is attacking another (and making the hp balance)
    public virtual void OnFight() { }

    //When the card dies either attacking or defending
    public virtual void OnDie() { }
}
