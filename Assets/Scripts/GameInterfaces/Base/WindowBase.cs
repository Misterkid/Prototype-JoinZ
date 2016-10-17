/*
Author: Eduard Meivogel
Website: http://eddymeivogel.com
Contact: Eddymeivogel@gmail.com
*/

using UnityEngine;
using System.Collections;

public class WindowBase : MonoBehaviour 
{
#region public variables
	
#endregion
#region protected variables
	protected Vector2 xySpacing = new Vector2(16,16);//spacing! Might be usefull for adding text and buttons.
	protected Rect windowRect;//Window size/position 
#endregion
#region private Variables
	private string windowTitle;//Window title(text)
	private bool bShowWindow = true;//Is the window vissible?
	private int windowId;//WindowId(It must never be the same).
	private GUI.WindowFunction windowFunc;//Window flunction:) what funtion does the window use?
	private KeyCode hideShowKey;//What key to hide and show.
	private GUISkin guiSkin;//Window guiskin.
	private float transparantcyAlpha;
#endregion
#region Override protected functions
#region unity
	// Use this for initialization
	protected virtual void Start () 
	{
		guiSkin = Resources.Load("GameSkin",typeof(GUISkin)) as GUISkin;//Load guiskin.
		//print (guiSkin);
	}
	
	// Update is called once per frame
	protected virtual void Update()
	{
		//I can drag the window? If so then drag window with my mouse!
		
		if(Input.GetKeyUp(hideShowKey))
			bShowWindow = !bShowWindow;
	}
	protected virtual void OnGUI () 
	{
		if(windowFunc != null)
		{
			GUI.skin = guiSkin;
			GUI.color = new Color(GUI.color.r,GUI.color.g,GUI.color.b,transparantcyAlpha);
			//Is the window vissible ? If yes then show it!
			if(bShowWindow)
			{
                windowRect = GUI.Window(windowId, windowRect, windowFunc, windowTitle);//Creates a window.
			}
		}
		else
		{
			GUI.Box(windowRect,"Cannot create window. WindowFunc = " + windowFunc + " @ windowTitle: " + windowTitle);	
		}
	}
#endregion
	//We have to initialize the Window! Giving it importand information.
	protected void initializeWindow (string window_title,Rect window_Rect,int window_id,GUI.WindowFunction window_Func,float transparantcy_Alpha = 0.90f,bool show_window = true ,KeyCode show_Hide_Key = KeyCode.None)
	{
		windowTitle = window_title;
		windowRect = window_Rect;
		windowId = window_id;
		hideShowKey = show_Hide_Key;
		windowFunc = window_Func;
		bShowWindow = show_window;
		transparantcyAlpha = transparantcy_Alpha;
	}
	
	protected void useCloseButton()
	{
		if(GUI.Button(new Rect((windowRect.width - 25) - xySpacing.x,xySpacing.y,25,25),"X"))
		{
			bShowWindow = false;
		}
	}
#endregion
#region public functions
	public void EnableWindow()
	{
		bShowWindow = true;
	}
	public void DisableWindow()
	{
		bShowWindow = false;
	}
#endregion
#region private functions
#endregion
}