using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;

public static class CardLoader
{
    public static GameObject cardPrefab = null;
    public static string[] csv = null;

    static void LoadCardPrefab()
    {
        var loadedObject = Resources.Load<GameObject>("Card");

        if (loadedObject == null) 
            throw new FileNotFoundException("Resources/ <No 'Card' prefab found>");

        cardPrefab = loadedObject;
    }

    static void LoadCsvFile()
    {
        var loadedObject = Resources.Load<TextAsset>("CardsInfo");

        if (loadedObject == null)
            throw new FileNotFoundException("Resources/ <No 'CardsInfo' prefab found>");

        csv = loadedObject.text.Split('\n');
    }

    public static GameObject LoadCards(ref List<int> cardIds)
    {
        if(cardPrefab == null) LoadCardPrefab();
        if(csv == null) LoadCsvFile();

        GameObject parent = new GameObject("CardLoader");

        foreach(int id in cardIds)
        {
            string[] row = csv[id + 1].Split(','); //First line are column names

            GameObject card = GameObject.Instantiate(cardPrefab, parent.transform);
            card.name = "Card<" + row[0] + ">"; //Row[0] is card ID, using this for testing purposes

            InitializeCardValues(ref card, ref row);
        }

        return parent;
    }

    public static GameObject LoadCards(int id)
    {
        List<int> c = new List<int>() { id };
        return LoadCards(ref c);
    }

    static void InitializeCardValues(ref GameObject card, ref string[] row)
    {
        CardValues vals = card.GetComponent<CardValues>();

        vals.id = int.Parse(row[0]);

        vals.cardName = row[1];
        vals.faction = row[2];

        vals.manaCost = int.Parse(row[3]);
        vals.range = int.Parse(row[4]);
        vals.damage = int.Parse(row[5]);
        vals.hp = int.Parse(row[6]);

        vals.abilityDescription = row[7];
        //vals.abilityId = int.Parse(row[9]); TODO: Reorder columns, this has to be before
    }
}
