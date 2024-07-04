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
        Transform traCanvas = transform.Find("Canvas");

        traCanvas.Find("ManaCostOutline").Find("ManaCost_Text").GetComponent<TMP_Text>().text = manaCost.ToString();
        traCanvas.Find("HPOutline").Find("HP_Text").GetComponent<TMP_Text>().text = hp.ToString();
        traCanvas.Find("DamageOutline").Find("Damage_Text").GetComponent<TMP_Text>().text = damage.ToString();
        traCanvas.Find("RangeOutline").Find("Range_Text").GetComponent<TMP_Text>().text = range.ToString();

        spriteName = "BanishCard";
        traCanvas.Find("CardImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + spriteName);

        if (cardName == "") cardName = "[_____ _____]";
        traCanvas.Find("Name_Text").GetComponent<TMP_Text>().text = cardName;

        if (faction == "") faction = "[_____]";
        traCanvas.Find("Faction_Text").GetComponent<TMP_Text>().text = faction;

        traCanvas.Find("Description_Text").GetComponent<TMP_Text>().text = abilityDescription;

        //TODO ROGER <3: Initialize visual representation
    }

    //TODO: All attack & move functions
    public void Attack()
    {

    }
}
