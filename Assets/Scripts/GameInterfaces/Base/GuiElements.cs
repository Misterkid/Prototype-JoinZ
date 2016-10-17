using UnityEngine;
using System.Collections;

public class GuiElements 
{
	// Use this for initialization
	void Start () 
	{
	
	}
}

public class DropDownMenu
{
	private bool buttonPressed = false;
	public int CreateDropDownMenu(Rect rect,string name,string[] listButtonNames)
	{
		
		if(GUI.Button(rect,name))
		{
			buttonPressed = !buttonPressed;
		}
		
		if(buttonPressed)
		{
			for(int i = 0; i < listButtonNames.Length; i++)
			{
				if(GUI.Button(new Rect(rect.x,rect.y +(rect.height * (i + 1)) ,rect.width,rect.height),listButtonNames[i]))
				{
					buttonPressed = false;
					return i;
				}
			}
		}
		return -1;
	}
}
