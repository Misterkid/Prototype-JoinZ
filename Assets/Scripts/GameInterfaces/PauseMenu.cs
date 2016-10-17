using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
    private bool bShowWindow = false;
    public Vector2 sWidthHeight = new Vector2(250, 25);
    public Vector2 bWidthHeight = new Vector2(100, 25);
    private Rect rWindowRect;
    public OptionsWindow optionsWindow;
	// Use this for initialization
	void Start () 
    {
        rWindowRect = new Rect(Screen.width / 2 - (300 / 2), Screen.height / 2 - (500 / 2), 300, 500);
	}

	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bShowWindow)
                bShowWindow = false;
            else
                bShowWindow = true;
        }
	}

    void OnGUI()
    {   
        if(bShowWindow)
            rWindowRect = GUI.Window(4, rWindowRect, PauseWindow, "Pause Window");
    }
    private void PauseWindow(int id)
    {
        float xPos = (rWindowRect.width / 2) - (bWidthHeight.x / 2);
        float yPos = 25;
        if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x, bWidthHeight.y), "Options"))
        {
            if (optionsWindow.activated)
                optionsWindow.activated = false;
            else
                optionsWindow.activated = true;
        }
        yPos = yPos + bWidthHeight.y;
        if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x, bWidthHeight.y), "Exit"))
        {
            bShowWindow = false;
        }
        yPos = yPos + bWidthHeight.y;
        if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x, bWidthHeight.y), "Quit to menu"))
        {
            Screen.lockCursor = false;
            Cursor.visible = true;

            Application.LoadLevel(0);
        }
        yPos = yPos + bWidthHeight.y;
        GUI.DragWindow();
    }
}
