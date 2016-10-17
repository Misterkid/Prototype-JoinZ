using UnityEngine;
using System.Collections;

public class InventoryWindow : WindowBase 
{
    public Mover mover;
    private Inventory inventory;
	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        initializeWindow("Inventory", new Rect((Screen.width * 0.5f) - (350 * 0.5f), (Screen.height * 0.5f) - 50, 350, 200), 2, InventoryWindowFunc, 60, false, KeyCode.I);
        inventory = mover.inventory;
        Debug.Log(inventory);
    }
    void InventoryWindowFunc(int id)
    {
        string itemNameLabel = "None";
        int maxLineW = 10;
        int startLineW = 32 / 2;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            float xPos = (i % maxLineW) * 32;
            int yPos = (i / maxLineW) * 32;
            Rect buttonRect = new Rect(startLineW + xPos, xySpacing.y + yPos, 32, 32);

            //Rect labelRect = new Rect(xySpacing.x + (32 * i) -32,(windowRect.height/2) - 32,200,64);
            if (buttonRect.Contains(Event.current.mousePosition))
            {
                itemNameLabel = inventory.items[i].itemName;
            }
            if (GUI.Button(buttonRect, inventory.items[i].inventoryItemTexture, ""))
            {
                bool canAdd = true;
                Debug.Log(inventory.items[i].GetType());
                //if(inventory.items[i]
                if (typeof(WeaponBase).IsAssignableFrom(inventory.items[i].GetType()))//Weapons
                {
                    if (mover.holdingItem != null)
                    {
                        if (inventory.AddItem(mover.holdingItem))
                        {
                            mover.holdingItem.gameObject.GetComponent<Collider>().enabled = false;
                            mover.holdingItem.gameObject.GetComponent<Renderer>().enabled = false;
                        }
                        else
                        {
                            canAdd = false;
                        }
                    }
                    if (canAdd)
                    {
                        inventory.items[i].UseItem(mover);
                        inventory.items[i].gameObject.GetComponent<Renderer>().enabled = true;
                        inventory.RemoveItem(inventory.items[i]);
                    }
                }
                else//Default
                {
                    if(inventory.items[i].useSound != null)
                        AudioSource.PlayClipAtPoint(inventory.items[i].useSound, mover.transform.position);

                    inventory.items[i].UseItem(mover);
                    Destroy(inventory.items[i]);
                    Destroy(inventory.items[i].gameObject);
                    inventory.RemoveItem(inventory.items[i]);
                }
            }
        }
        GUI.Label(new Rect(xySpacing.x, xySpacing.y + (32 * 5), 300, 32), "Item Name: " + itemNameLabel);
        GUI.DragWindow();
    }
}
