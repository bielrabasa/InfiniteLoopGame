using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardValues : MonoBehaviour
{
    public uint id;

    public string cardName;
    public string faction;

    public int manaCost;
    public int range;
    public int damage;
    public int hp;

    public uint abilityId;
    public string abilityDescription;

    public string spriteName;

    //-------------------------------------

    public int tempDamage;
    public int tempRange;

    //-------------------------------------

    public Ability abilityScript;

    //-------------------------------------

    TMP_Text rangeText;
    TMP_Text damageText;
    TMP_Text hpText;

    //-------------------------------------

    GameObject particleObject;
    ParticleSystem particleSystem;
    TMP_Text particleText;

    //-------------------------------------

    void Start()
    {
        ResetTemporalValues(); //To initialize tempDamage

        InitializeVisuals();

        particleObject = GameObject.Find("Damage_Particle");
        particleSystem = GameObject.Find("Damage_Particle").GetComponent<ParticleSystem>();
        particleText = GameObject.Find("Particle_Text").GetComponent<TMP_Text>();
    }

    public void AddAbilityScript() //Has to be called as soon as the ability id is set (btw Awake & Start)
    {
        switch (abilityId)
        {
            case 1: abilityScript = gameObject.AddComponent<Ability_1>(); break;
            case 2: abilityScript = gameObject.AddComponent<Ability_2>(); break;
            case 3: abilityScript = gameObject.AddComponent<Ability_3>(); break;
            case 4: abilityScript = gameObject.AddComponent<Ability_4>(); break;

            default: abilityScript = gameObject.AddComponent<Ability>();  break;
        }

        abilityScript.me = this;
    }

    void InitializeVisuals()
    {
        Transform traCanvas = transform.Find("Canvas");

        traCanvas.Find("ManaCost_Text").GetComponent<TMP_Text>().text = manaCost.ToString();
        
        //Variable info
        hpText = traCanvas.Find("HP_Text").GetComponent<TMP_Text>();
        hpText.text = hp.ToString();

        damageText = traCanvas.Find("Damage_Text").GetComponent<TMP_Text>();
        damageText.text = tempDamage.ToString();

        rangeText = traCanvas.Find("Range_Text").GetComponent<TMP_Text>();
        rangeText.text = tempRange.ToString();
        //

        //spriteName = "Dwarf_01";
        traCanvas.Find("CardImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + spriteName);

        //if (cardName == "") cardName = "[_____ _____]";
        traCanvas.Find("Name_Text").GetComponent<TMP_Text>().text = cardName;

        //if (faction == "") faction = "[_____]";
        traCanvas.Find("Faction_Text").GetComponent<TMP_Text>().text = faction;

        traCanvas.Find("Description_Text").GetComponent<TMP_Text>().text = abilityDescription;
    }

    public void UpdateVisuals()
    {
        hpText.text = hp.ToString();
        damageText.text = tempDamage.ToString();
        rangeText.text = tempRange.ToString();
    }
    
    public void SetParticle(Color text_color, Vector3 pos, string text_value)
    {
        particleObject.transform.localPosition = pos;

        var psMain = particleSystem.main;
        psMain.startColor = text_color;
        particleText.text = text_value;
        particleSystem.Play();
    }

    //-----------PLAY FUNCTIONS----------

    public void ResetTemporalValues()
    {
        tempDamage = damage;
        tempRange = range;
    }

    public IEnumerator Attack(Vector2Int myPosition)
    {
        abilityScript.myStartPosition = myPosition;
        yield return abilityScript.MakeCardAttackSequence();
    }
}
