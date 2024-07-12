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
    
    void Start()
    {
        ResetTemporalValues(); //To initialize tempDamage

        InitializeVisuals();
    }

    public void AddAbilityScript() //Has to be called as soon as the ability id is set (btw Awake & Start)
    {
        Debug.Log(abilityId);
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

        spriteName = "Dwarf_01";
        traCanvas.Find("CardImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + spriteName);

        if (cardName == "") cardName = "[_____ _____]";
        traCanvas.Find("Name_Text").GetComponent<TMP_Text>().text = cardName;

        if (faction == "") faction = "[_____]";
        traCanvas.Find("Faction_Text").GetComponent<TMP_Text>().text = faction;

        traCanvas.Find("Description_Text").GetComponent<TMP_Text>().text = abilityDescription;
    }

    public void UpdateVisuals()
    {
        hpText.text = hp.ToString();
        damageText.text = tempDamage.ToString();
        rangeText.text = tempRange.ToString();
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

        //After attack, reset tempDamage & tempRange
        ResetTemporalValues();
        if(!abilityScript.isDying) UpdateVisuals();
    }
}
