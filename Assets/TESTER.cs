using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTER : MonoBehaviour
{
    [SerializeField] bool setCardsOnMap = false;
    [Space]
    //ALWAYS 3 COLUMNS
    [SerializeField] Vector3Int[] cardsOnMap = new Vector3Int[MapState.ROWS];

    private void Start()
    {
        if (setCardsOnMap) SetCardsOnMap();
    }

    void SetCardsOnMap()
    {
        for (int i = 0; i < MapState.ROWS; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //Do not place cards where there is a 0
                if(j == 0) if (cardsOnMap[i].x == 0) continue;
                if(j == 1) if (cardsOnMap[i].y == 0) continue;
                if(j == 2) if (cardsOnMap[i].z == 0) continue;

                //Set other cards on board
                SetOneCardOnMap(new Vector2Int(i, j));
            }
        }
    }

    void SetOneCardOnMap(Vector2Int pos)
    {
        uint val = 0;

        if (pos.y == 0) val = (uint)cardsOnMap[pos.x].x;
        if (pos.y == 1) val = (uint)cardsOnMap[pos.x].y;
        if (pos.y == 2) val = (uint)cardsOnMap[pos.x].z;

        GameObject h = CardLoader.LoadCards(val);

        //Redirect this card parenting
        if(h.transform.childCount == 1)
        {
            Transform card = h.transform.GetChild(0);
            card.SetParent(transform);
            MapState.cardPositions[pos.y, pos.x] = card.gameObject;
            MapState.SetCardOnPhysicalBoard(card, new Vector2Int(pos.y, pos.x));
        }

        Destroy(h);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) StartCoroutine(MapState.NextPhase());

        if (Input.GetKeyDown(KeyCode.P))
        {
            Particle.InstanceParticle(Particle.ParticleType.ADD_DAMAGE, 1, Vector3.forward);
            Particle.InstanceParticle(Particle.ParticleType.ADD_MANA, 2, Vector3.back);
            Particle.InstanceParticle(Particle.ParticleType.ADD_RANGE, 3, Vector3.right);
            Particle.InstanceParticle(Particle.ParticleType.RECEIVE_DAMAGE, 4, Vector3.left);
        }
    }
}
