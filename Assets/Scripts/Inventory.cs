using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Inventory : MonoBehaviour 
{
    public List<Item> items = new List<Item>();
    private int maxItemSlot = 20;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    public bool AddItem(Item item)
	{
		if(items.Count < maxItemSlot)
		{
			items.Add(item);
			return true;
		}
		return false;
	}
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        List<Item> updateItems = new List<Item>();
        foreach (Item oldItem in items)
        {
            updateItems.Add(oldItem);
        }
    }
}
