using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndivParticleScript : MonoBehaviour
{
    public void Init()
    {
        //todo: set canvas values
    }

    void Start()
    {
        Destroy(gameObject, 1);
    }
}
