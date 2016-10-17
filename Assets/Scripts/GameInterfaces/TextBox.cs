using UnityEngine;
using System.Collections;

public class TextBox : WindowBase 
{
    public string npcText;
	// Use this for initialization
    protected override void Start()
    {
        initializeWindow("Text Box", new Rect(0, Screen.height - 200, 400,200), 3, TextBoxWindowFunc, 90);
        base.Start();

    }
    void TextBoxWindowFunc(int id)
    {
        GUI.Box(new Rect(xySpacing.x, xySpacing.y, (windowRect.width - (xySpacing.x * 2)), (windowRect.height - (xySpacing.y * 2))), npcText);
        GUI.DragWindow();
    }
}
