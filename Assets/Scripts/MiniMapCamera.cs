using UnityEngine;
using System.Collections;

public class MiniMapCamera : MonoBehaviour 
{
    public float addHeightY = 45;
    public GameObject playerTarget;
	// Use this for initialization
	void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.gameObject.transform.position = new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y + addHeightY, playerTarget.transform.position.z);
	}
    /*
    void OnGUI()
    {
        
        GUI.DrawTexture(new Rect(Screen.width-200, 0, 200, 200), this.camera.targetTexture);
        this.camera.Render();
         
    }
*/
}