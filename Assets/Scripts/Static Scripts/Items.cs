﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Items : MonoBehaviour
{
    static List<Item> items;

    // Use this for initialization
    public static void Start()
    {
        items = new List<Item>();
        string[] files = Directory.GetFiles("items");
        foreach (string file in files)
        {
            Item item = new Item();
            string[] lines = File.ReadAllLines(file);

            foreach (string line in lines)
            {
                int x = 0;
                bool b = false; ;
                string[] l = line.Split(' ');

                try
                {
                    if (l[0].Equals("tags"))
                    {
                        List<string> tags = new List<string>();
                        for (int i = 2; i < l.Length; i++)
                            tags.Add(l[i]);
                        item.tags = tags;
                    }
					else if (l[0].Equals("sprite"))
                    {
                        Sprite sprite = Resources.Load<Sprite>(l[2]);
                        GameObject gameObject = new GameObject();
                        gameObject.AddComponent<SpriteRenderer>();
                        SpriteRenderer SR = gameObject.GetComponent<SpriteRenderer>();
                        SR.sprite = sprite;
                        item.sprite = Instantiate(gameObject) as GameObject;
                        item.sprite.SetActive(false);
                    }
                    else if (int.TryParse(l[2], out x))
                        item.GetType().GetProperty(l[0]).SetValue(item, x, null);
                    else if (bool.TryParse(l[2], out b))
                        item.GetType().GetProperty(l[0]).SetValue(item, b, null);
                    else
                        item.GetType().GetProperty(l[0]).SetValue(item, l[2], null);

                }
                catch (NullReferenceException)
                {
                    print("Error with item " + file + " property " + l[0] + " is invalid");
                }
            }
            items.Add(item);
        }
    }

    public class Item
    {
        public Item() {
            isArmor = false;
            canWield = false;
        }
        public Item(int val, string nam, List<string> tags, string description, bool wield, bool armor)
        {
            value = val;
            name = nam;
            this.tags = tags;
            this.description = description;
            canWield = wield;
            isArmor = armor;
        }

		public GameObject sprite { get; set; }
        public int value { get; set; }
        public string name { get; set; }
        public List<string> tags { get; set; }
        public string description { get; set; }
        public bool canWield { get; set; }
        public bool isArmor { get; set; }
        public int damage { get; set; }
        public int armor { get; set; }
        public int heal { get; set; }
    }

    internal static Item getRandomItemOfTag(string key, Dictionary<Item, int> items)
    {
        List<Item> itemsWithTag = new List<Item>();
        foreach (KeyValuePair<Item, int> item in items)
        {
            if (item.Key.tags.Contains(key))
            {
                itemsWithTag.Add(item.Key);
            }
        }
        if (itemsWithTag.Count == 0)
        {
            return null;
        }
        return itemsWithTag[Random.Range(0, itemsWithTag.Count)];
    }

    //maybe make this more effecient by keeping lists of all items with each tag
    internal static Item getRandomItemOfTag(string key)
    {
        List<Item> itemsWithTag = new List<Item>();
        foreach (Item item in items)
        {
            if (item.tags.Contains(key))
            {
                itemsWithTag.Add(item);
            }
        }
        return itemsWithTag[Random.Range(0, itemsWithTag.Count)];
    }

	// Gets an item with the given name
    public static Item getItemWithName(string name)
    {
        foreach (Item item in items)
        {
            if (item.name.Equals(name))
                return item;
        }
        return null;
    }

    public static Item getRandomItem()
    {
        int item = Random.Range(0, items.Count);
        return items[item];
    }
}
