using UnityEngine;
using System.Collections;

//[ExecuteInEditMode()]
public class HealthBar : MonoBehaviour
{
	private Mover[] movers;
	public Player player;
	public Texture healthBarText;
	// Use this for initialization
	void Start () 
	{
		//movers = GameObject.FindObjectsOfType(typeof(Mover)) as Mover[];
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI()
	{
		if(player != null)
		{
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			movers = GameObject.FindObjectsOfType(typeof(Mover)) as Mover[];
			for(int i = 0; i < movers.Length; i++)
			{
				if(movers[i].GetComponent<Renderer>().isVisible)
				{
					if(Vector3.Distance(player.transform.position,movers[i].transform.position) < 35f)
					{
						Vector3 targetDir = movers[i].gameObject.transform.position - player.gameObject.transform.position;
						RaycastHit  hit; // check if it's visible
						if (Physics.Raycast(player.gameObject.transform.position, targetDir, out hit))
						{
							
							Vector3 wantedPos = Camera.main.WorldToScreenPoint(new Vector3(movers[i].transform.position.x - (GetObjectSize(movers[i].gameObject).x/2),movers[i].transform.position.y + (GetObjectSize(movers[i].gameObject).y/2),movers[i].transform.position.z));
			     			//transform.position = wantedPos;
							
							
							int healthBoxes = (int)movers[i].health /10;
							if(healthBoxes != 10)
								Debug.Log(healthBoxes);
							for(int h = 0; h < healthBoxes; h++)
							{
								GUI.DrawTexture(new Rect((wantedPos.x - (120)) +(12 * h),(Screen.height -wantedPos.y),12,12),healthBarText);
							}
						
							
							GUI.Label(new Rect(wantedPos.x - 100,(Screen.height - wantedPos.y),100,32),"Health" + (int)movers[i].health);

						}
					}
				}
			}
		}
	}
	Vector3 GetObjectSize(GameObject moverObject)
	{
		MeshFilter meshFilter = moverObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
		
		if(meshFilter == null)
		{
			//Debug.LogError("meshFilter is null");
			return Vector3.zero;
		}
		Mesh mesh = meshFilter.sharedMesh;
		
		if(mesh == null)
		{
			//Debug.LogError("mesh is null");
			return Vector3.zero;
		}
		Vector3 size  = mesh.bounds.size;
		Vector3 scale = moverObject.transform.lossyScale;
		
		Vector3 ObjectSize = new Vector3(size.x * scale.x,size.y * scale.y,size.z * scale.z);
		return ObjectSize;
	}
}