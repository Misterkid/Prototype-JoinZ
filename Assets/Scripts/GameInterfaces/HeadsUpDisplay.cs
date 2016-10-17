using UnityEngine;
using System.Collections;

public class HeadsUpDisplay : MonoBehaviour 
{
    public Player player;
    public Texture healthBarTexture;
    public Texture StaminaBarTexture;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	        
	}
    void OnGUI()
    {
        for (int i = 0; i < (player.health / 10); i++)
        {
            GUI.DrawTexture(new Rect((23 * i + 1),Screen.height - (23 * 2),23,23),healthBarTexture);
        }
        // GUI.Label(new Rect(xySpacing.x, xySpacing.y, 200, 18), "Health: " + (int)player.health);

        for (int i = 0; i < (player.stamina / 10); i++)
        {
            GUI.DrawTexture(new Rect((23 * i + 1), Screen.height - 23, 23, 23), StaminaBarTexture);
        }

        //GUI.Label(new Rect(xySpacing.x, xySpacing.y + 18, 200, 18), "Stamina: " + (int)player.stamina);


        GUI.Label(new Rect(0, Screen.height - (23 * 3), 200, 23), "Hunger:" + (int)player.hunger);
        GUI.Label(new Rect(0, Screen.height - (23 * 4), 200, 23), "thirst: " + (int)player.thirst);

        if (player.holdingItem != null)
        {
            GunBase holdingGun = player.holdingItem as GunBase;
            if (holdingGun != null)
            {
                GUI.Label(new Rect(0, Screen.height - (23 * 5), 200, 35), holdingGun.itemName + ": " + holdingGun.bulletsInMeg + "/" + holdingGun.bullets);
            }
        }
    }
}
