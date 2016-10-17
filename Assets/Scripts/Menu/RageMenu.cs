using UnityEngine;
using System.Collections;

public class RageMenu : MonoBehaviour 
{
    public string[] sceneNames;
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
        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (GUI.Button(new Rect(0, 35 + (35 * i), 200, 35), sceneNames[i]))
            {
                Application.LoadLevel(sceneNames[i]);
            }
        }
    }
}
