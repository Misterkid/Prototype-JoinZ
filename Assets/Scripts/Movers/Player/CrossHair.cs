using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour 
{

	public Texture tCrossHair;
   // public Texture tMouseCursor;
    public Vector2 crossHairSize = new Vector2(25, 25);
	private Rect rPosition;

    private Texture tCrossHairStandard;
    private Vector2 crossHairSizeStandard; 
	//private float fSize = 25;
    public bool showGameCursor = false;
    private bool mouseOnScreen = false;
    private Rect screenRect;
	// Use this for initialization
	void Start () 
	{

        tCrossHairStandard = tCrossHair;
        crossHairSizeStandard = crossHairSize;
        rPosition = new Rect((Screen.width * 0.5f) - (crossHairSize.x * 0.5f), (Screen.height * 0.5f) - (crossHairSize.y * 0.5f), crossHairSize.x, crossHairSize.y);
        Cursor.visible = false;
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (!mouseOnScreen)
        {
            if (screenRect.Contains(Input.mousePosition))
            {
                Screen.lockCursor = true;
                mouseOnScreen = true;
               //Debug.Log("Lock!");
            }
        }
        else
        {
            if(!screenRect.Contains(Input.mousePosition))
            {
                Screen.lockCursor = false;
                mouseOnScreen = false;
                //Debug.Log("unlock!");
            }
        }
	}
    public void ResetCrosshair()
    {
        tCrossHair = tCrossHairStandard;
        crossHairSize = crossHairSizeStandard;
        rPosition = new Rect((Screen.width * 0.5f) - (crossHairSize.x * 0.5f), (Screen.height * 0.5f) - (crossHairSize.y * 0.5f), crossHairSize.x, crossHairSize.y);
        
    }
    public void SetCrosshair(Texture newCrossHair,Vector2 newCroshairSize)
    {
        tCrossHair = newCrossHair;
        crossHairSize = newCroshairSize;
        rPosition = new Rect((Screen.width * 0.5f) - (crossHairSize.x * 0.5f), (Screen.height * 0.5f) - (crossHairSize.y * 0.5f), crossHairSize.x, crossHairSize.y);
    }
    void EnabledCurser()
    {

    }
	// GUI! Textures and shit on screen.
	void OnGUI()
	{
        rPosition = new Rect((Screen.width * 0.5f) - (crossHairSize.x * 0.5f), (Screen.height * 0.5f) - (crossHairSize.y * 0.5f), crossHairSize.x, crossHairSize.y);
        screenRect = new Rect(0, 0, Screen.width, Screen.height);

        GUI.depth = 0;
        if (!showGameCursor)
        {
            GUI.DrawTexture(rPosition, tCrossHair);
        }
	}
}
