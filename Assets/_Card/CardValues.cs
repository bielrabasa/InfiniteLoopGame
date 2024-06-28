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

    public uint manaCost;
    public uint range;
    public uint damage;
    public uint hp;

    public uint abilityId;
    public string abilityDescription;

    public string spriteName;

    void Start()
    {
        AddAbilityScript();
        InitializeVisuals();
    }

    void AddAbilityScript()
    {
        //TODO: Add ability script from ability id
    }

    void InitializeVisuals()
    {
        transform.Find("ManaCostOutline").Find("ManaCost_Text").GetComponent<TMP_Text>().text = manaCost.ToString();
        transform.Find("HPOutline").Find("HP_Text").GetComponent<TMP_Text>().text = hp.ToString();
        transform.Find("DamageOutline").Find("Damage_Text").GetComponent<TMP_Text>().text = damage.ToString();
        transform.Find("RangeOutline").Find("Range_Text").GetComponent<TMP_Text>().text = range.ToString();

 
        transform.Find("CardImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + spriteName);

        transform.Find("Name_Text").GetComponent<TMP_Text>().text = cardName;
        transform.Find("Faction_Text").GetComponent<TMP_Text>().text = faction;
        transform.Find("Description_Text").GetComponent<TMP_Text>().text = abilityDescription;

        //TODO ROGER <3: Initialize visual representation
    }

    //TODO: All attack & move functions
    public void Attack()
    {

    }
}
