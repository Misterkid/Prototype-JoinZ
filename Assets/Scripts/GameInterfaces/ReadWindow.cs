using UnityEngine;
using System.Collections;

public class ReadWindow :WindowBase 
{
	public Player player;
	private bool isReading = false;
	// Use this for initialization
	protected override void Start () 
	{
		initializeWindow("Readeble",new Rect(0,0,Screen.width,Screen.height),2,ReadableWindowFunc,90);
		DisableWindow();
		base.Start();
	
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
			if(Input.GetKeyUp(KeyCode.E))
	        {
				if(!isReading)
				{
		            RaycastHit  hit;
				    Ray  ray = Camera.main.ScreenPointToRay(new Vector3 (Screen.width / 2, Screen.height / 2,0));//CrossHair
		            if (Physics .Raycast (ray, out hit ))
		            {
						if(hit.collider.gameObject == this.gameObject && Vector3.Distance(this.gameObject.transform.position,player.transform.position) < 1.5f && this.gameObject.GetComponent<Renderer>().isVisible == true)
						{
							EnableWindow();
							isReading = true;
						}
						
					}
				}
				else
				{
					DisableWindow();
					isReading = false;
				}
				
			}
		base.Update();
	}
	void ReadableWindowFunc(int id)
	{
		if(isReading)
			GUI.DrawTexture(new Rect(0,0,windowRect.width,windowRect.height),this.gameObject.GetComponent<Renderer>().material.mainTexture);

	}
}
