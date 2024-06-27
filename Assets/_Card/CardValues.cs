using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //TODO ROGER <3: Initialize visual representation
    }

    //TODO: All attack & move functions
    public void Attack()
    {

    }
}
