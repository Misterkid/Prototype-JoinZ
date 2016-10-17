/*
Author: Eduard Meivogel
Website: http://eddymeivogel.com
Contact: Eddymeivogel@gmail.com
*/

using UnityEngine;
using System.Collections;

public class LoadLevelProgressScreen : MonoBehaviour 
{
	
	public Texture2D progressBarEmpty;//empty bar
	public Texture2D progressBarFull;//Full bar
	public Texture2D screenBackground;
	private string loadLevelName;
	private int loadLevelId = -1;
	private AsyncOperation asyncLevel;
	private bool startLoadLevel = false;
	
	float progress;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(startLoadLevel)
		{
			if(asyncLevel == null)
			{
				Debug.LogError("Error LevelName Not Found Loading Menu instead: " + loadLevelName);
				startLoadLevel = false;
				loadMenu();
			}
			progress = Mathf.Floor(asyncLevel.progress * 100f);
		}
	}
	
	void OnGUI()
	{
		if(startLoadLevel)
		{
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),screenBackground);
			GUI.DrawTexture(new Rect((Screen.width/2) -(progressBarEmpty.width/2), (Screen.height - progressBarEmpty.height ), progressBarEmpty.width, progressBarEmpty.height), progressBarEmpty);
			GUI.BeginGroup(new Rect ((Screen.width/2) -(progressBarEmpty.width/2), (Screen.height - progressBarEmpty.height ), progressBarEmpty.width* Mathf.Clamp01((progress / 100f)), progressBarEmpty.height));
			GUI.DrawTexture(new Rect(0, 0, progressBarFull.width, progressBarFull.height), progressBarFull);
			GUI.EndGroup();
		}
	}
	
	public void LoadLevel(string LevelName)
	{
		DestroyAllGUI();
		loadLevelName = LevelName;
		StartCoroutine("StartLoading");
	}
	public void LoadLevel(int LevelId)
	{
		DestroyAllGUI();
		loadLevelId = LevelId;
		StartCoroutine("StartLoading");
	}
	private void loadMenu()
	{
		DestroyAllGUI();
		loadLevelName = "MainManu";
		StartCoroutine("StartLoading");
	}
	IEnumerator StartLoading() 
	{
		if(loadLevelId == -1)
			asyncLevel = Application.LoadLevelAsync(loadLevelName);
		else
			asyncLevel = Application.LoadLevelAsync(loadLevelId);
		
		startLoadLevel = true;
		yield return asyncLevel;
		Debug.Log("Loading complete");
		startLoadLevel = false;
    }
	private void DestroyAllGUI()
	{
		Object[] objects = GetComponents (typeof(Component ));
		for(int i = 0; i < objects.Length; i++)
		{
			if(objects[i] != this && objects[i] != gameObject.transform)
			{
				Destroy(objects[i]);
			}
		}
	}
}
