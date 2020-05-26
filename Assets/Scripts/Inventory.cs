using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> items;
    // Start is called before the first frame update
    void Start()
    {
        GUI.enabled = true;
        //items = new List<string>();
    }

    private void AddItem(string itemName)
    {
        items.Add(itemName);
    }

    public void Add(List<string> items)
    {
        foreach (string item in items)
        {
            this.items.Add(item);
        }
    }
    
    private string PrintItems()
    {
        string output = String.Empty;
        output = "Inventory: \n";
        foreach (string item in this.items)
        {
            output += "- " + item + "\n";
        }

        return output;
    }

    void OnGUI()
    {
        if(gameObject.CompareTag("Player"))
            GUI.Label(new Rect(10, 10, 100, 200), PrintItems());
        
    }
}
