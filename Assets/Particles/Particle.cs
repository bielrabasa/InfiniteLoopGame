using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public static Particle particle;
    [SerializeField] GameObject particlePrefab;
    [SerializeField] Color colorAD;
    [SerializeField] Color colorAR;
    [SerializeField] Color colorAM;
    [SerializeField] Color colorRD;

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

    public static void InstanceParticle(ParticleType type, int text, Vector3 pos)
    {
        if(particle == null)
        {
            Debug.Log("No Particle gameobject exists!");
            return;
        }

        GameObject newParticle = Instantiate(particle.particlePrefab, pos, Quaternion.identity);

        newParticle.transform.Rotate(90,0,0);

        Color colorP = new Color(1,1,1,1);
        string valueP = "0";

        switch (type)
        {
            case ParticleType.NONE:
                return;
            case ParticleType.ADD_DAMAGE:
                colorP = particle.colorAD;
                valueP = "+" + text.ToString();
                break;
            case ParticleType.ADD_RANGE:
                colorP = particle.colorAR;
                valueP = "+" + text.ToString();
                break;
            case ParticleType.ADD_MANA:
                colorP = particle.colorAM;
                valueP = "+" + text.ToString();
                break;
            case ParticleType.RECEIVE_DAMAGE:
                colorP = particle.colorRD;
                valueP = "-" + text.ToString();
                break;
        }

        newParticle.GetComponent<IndivParticleScript>().Init(valueP, colorP);
    }
}
