using UnityEngine;
using System.Collections;

public class MainMenu : WindowBase 
{
	public int levelId = 1;
	private LoadLevelProgressScreen loadLevelManager;
	private bool bShowInfo = false;
    public OptionsWindow optionsWindow;
	protected override void Start () 
	{
		base.Start();
		loadLevelManager = GetComponent<LoadLevelProgressScreen>();
		initializeWindow("Main Menu",new Rect(0,0,Screen.width * 0.5f,Screen.height * 0.5f),1,MainMenuFunc,90);
	}
	/*
	protected override void Update()
	{
		base.Update();
	}
	// Update is called once per frame
	protected override void OnGUI ()
	{
		base.OnGUI();
	}
	*/
	void MainMenuFunc(int id)
	{
		if(GUI.Button(new Rect(xySpacing.x,xySpacing.y,200,35),"Test Level 1"))
		{
			loadLevelManager.LoadLevel(1);
		}
        if (GUI.Button(new Rect(xySpacing.x, xySpacing.y + 35, 200, 35), "Test Level 2"))
        {
            loadLevelManager.LoadLevel(2);
        }
        if (GUI.Button(new Rect(xySpacing.x, xySpacing.y + (35 * 2), 200, 35), "Pokemon bonus level"))
        {
            loadLevelManager.LoadLevel(3);
        }
        if (GUI.Button(new Rect(xySpacing.x, xySpacing.y + (35 * 3), 200, 35), "Testing other features"))
        {
            loadLevelManager.LoadLevel(4);
        }
        if (GUI.Button(new Rect(xySpacing.x, xySpacing.y + (35 * 4), 125, 35), "Options"))
        {
            if (optionsWindow.activated)
                optionsWindow.activated = false;
            else
                optionsWindow.activated = true;
        }
		GUI.Label(new Rect(xySpacing.x,(windowRect.height - 64),windowRect.width,32),"Created by Eddy Meivogel");
		GUI.Label(new Rect(xySpacing.x,(windowRect.height - 32),windowRect.width,32),"Website: http://EddyMeivogel.com");
        GUI.DragWindow();
	}
}
