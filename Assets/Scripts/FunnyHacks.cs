using UnityEngine;
using System.Collections;

public class FunnyHacks : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}

	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            Creeper[] creepers = GameObject.FindObjectsOfType(typeof(Creeper)) as Creeper[];
            if (creepers.Length != 0)
            {
                bool foundRight = false;
                Creeper creeper = null;
                while (!foundRight)
                {
                    if (creepers.Length < 2)
                        foundRight = true;
                    creeper = creepers[Random.Range(0, creepers.Length)];
                    if (creeper.health > 0)
                        foundRight = true;
                }
                if (creeper.health > 0)
                    creeper.doExplosion(creeper.health);
            }
        }
	}
    void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height - 32, Screen.width, 32), "Hacks activated! press 0 to explode a creeper.");
    }
}
