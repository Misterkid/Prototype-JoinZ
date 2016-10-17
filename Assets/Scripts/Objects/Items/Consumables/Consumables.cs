using UnityEngine;
using System.Collections;

public class Consumables : Item 
{
    public float fillRate;
    public enum ConsumableType
    {
        HUNGER,
        THIRST,
        HEALTH
    }
    public ConsumableType consumableType = ConsumableType.HEALTH;
	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    public override void UseItem(Mover mover)
    {
        if (mover.GetType().ToString() == "Player")
        {
            Player player = mover as Player;

            switch (consumableType)
            {
                case ConsumableType.HEALTH:
                player.health += fillRate;
                if (player.health >= player.maxHealth)
                    player.health = player.maxHealth;
                    break;

                case ConsumableType.HUNGER:
                    player.hunger += fillRate;
                if (player.hunger >= player.maxHunger)
                    player.hunger = player.maxHunger;
                    break;

                case ConsumableType.THIRST:
                player.thirst += fillRate;
                if (player.thirst >= player.maxThirst)
                    player.thirst = player.maxThirst;
                    break;

                default: break;
            }
        }
        base.UseItem(mover);
    }
}
