using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
    public Texture inventoryItemTexture;
    public string itemName;
    public int inventoryId;
    public Vector3 oldObjectScale = Vector3.zero;

    public AudioClip useSound;
    public AudioClip pickUpSound;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    public virtual void UseItem(Mover mover)
    {
        
    }
}
