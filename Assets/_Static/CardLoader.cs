using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CardLoader
{
    static GameObject cardPrefab = null;
    static string[] csv = null;
    
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

    public static GameObject LoadCards(ref List<uint> cardIds)
    {
        if(cardPrefab == null) LoadCardPrefab();
        if(csv == null) LoadCsvFile();

        GameObject parent = new GameObject("CardLoader");

        foreach(int id in cardIds)
        {
            if (id <= 0 || id >= csv.Length)
            {
                Debug.Log("[CardLoading] Card ID <" + id + "> out of CSV file length.");
                continue;
            }

            string[] row = csv[id].Split(','); //First line are column names (csv[0])

            GameObject card = GameObject.Instantiate(cardPrefab, parent.transform);
            card.name = "Card<" + row[0] + ">"; //Row[0] is card ID, using this for testing purposes

            InitializeCardValues(ref card, ref row);
        }

        return parent;
    }

    public static GameObject LoadCards(uint id)
    {
        List<uint> c = new List<uint>() { id };
        return LoadCards(ref c);
    }

    static void InitializeCardValues(ref GameObject card, ref string[] row)
    {
        CardValues vals = card.GetComponent<CardValues>();

        vals.id = uint.Parse(row[0]);

        vals.cardName = row[1];
        if (vals.cardName == "") vals.cardName = "No Name";

        vals.faction = row[2];
        if (vals.faction == "") vals.faction = "No Faction";

        vals.manaCost = int.Parse(row[3]);
        vals.range = int.Parse(row[4]);
        vals.damage = int.Parse(row[5]);
        vals.hp = int.Parse(row[6]);

        if(!uint.TryParse(row[7], out vals.abilityId)) vals.abilityId = 0; //In case of no ability, set value 0
        vals.abilityDescription = row[8];
    }
}
