using UnityEngine;
using System.Collections;
public class StatusWindow : WindowBase 
{
	public Player player;
    public Texture healthBarTexture;
    public Texture StaminaBarTexture;
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		initializeWindow("Status",new Rect(0,0,300,100),1,StatusWindowFunc,90);

	}
	/*
	// Update is called once per frame
	void Update () 
	{
	
	}
	*/
	void StatusWindowFunc(int id)
	{
        for (int i = 0; i < (player.health / 10); i++)
        {
            GUI.DrawTexture(new Rect(xySpacing.x + (23 * i + 1), xySpacing.y + 4, 23, 23), healthBarTexture);
        }
       // GUI.Label(new Rect(xySpacing.x, xySpacing.y, 200, 18), "Health: " + (int)player.health);

        for (int i = 0; i < (player.stamina / 10); i++)
        {
            GUI.DrawTexture(new Rect(xySpacing.x + (23 * i + 1), xySpacing.y + 23, 23, 23), StaminaBarTexture);
        }
      //  GUI.Label(new Rect(xySpacing.x, xySpacing.y + 18, 200, 18), "Stamina: " + (int)player.stamina);
        GUI.Label(new Rect(xySpacing.x, xySpacing.y + 46, 200, 18), "Hunger:" + (int)player.hunger);
        GUI.Label(new Rect(xySpacing.x, xySpacing.y + 64, 200, 18), "thirst: " + (int)player.thirst);

        if (player.holdingItem != null)
        {
            GunBase holdingGun = player.holdingItem as GunBase;
            if (holdingGun != null)
            {
                GUI.Label(new Rect(xySpacing.x, xySpacing.y + 82/*(18 * 2)*/, 200, 35), holdingGun.itemName + ": " + holdingGun.bulletsInMeg + "/" + holdingGun.bullets);
            }
        }
        GUI.DragWindow();
       // GUI.Label(new Rect(xySpacing.x, xySpacing.y + (16 * 2), 200, 35), "Bullets: " + (player.holdingGun.bulletsInMeg + player.holdingGun.bullets));
	}
	
}
