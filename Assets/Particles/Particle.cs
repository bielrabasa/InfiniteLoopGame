using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public static Particle particle;
    [SerializeField] GameObject particlePrefab;

    public enum ParticleType
    {
        NONE,
        ADD_DAMAGE,
        ADD_RANGE,
        ADD_MANA,
        RECEIVE_DAMAGE
    }

    private void Awake()
    {
        particle = this;
    }

    public void InstanceParticle(ParticleType type, int text, Vector3 pos)
    {
        Instantiate(particlePrefab, pos, Quaternion.identity).GetComponent<IndivParticleScript>().Init();
        //TODO: depending on the type -> Color, size...
        //TODO: Add + or -
    }
}
