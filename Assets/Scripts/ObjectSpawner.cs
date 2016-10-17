using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObjectSpawner : MonoBehaviour 
{
	public int objectCount;
	public GameObject spawnGameObject;
    public bool isPrefebObject;
	public float randomFactor;

    private List<Vector3> olderPositions = new List<Vector3>();
	// Use this for initialization
	void Start () 
	{
        for (int i = 0; i < objectCount; i++)
		{
			float randomX = Random.Range(-randomFactor,randomFactor);
//			float randomY = Random.Range(-randomFactor,randomFactor);
			float randomZ = Random.Range(-randomFactor,randomFactor);
            Vector3 position = new Vector3(this.gameObject.transform.position.x + randomX, this.gameObject.transform.position.y /*+ randomY*/, this.gameObject.transform.position.z + randomZ);
            olderPositions.Add(position);
		//	Quaternion rotation = new Quaternion(zombie.transform.rotation.x+ randomX,zombie.transform.rotation.y,zombie.transform.rotation.z,zombie.transform.rotation.w);
            GameObject gameObjectClone = Instantiate(spawnGameObject, position/*,rotation*/, gameObject.transform.rotation) as GameObject;
            gameObjectClone.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));

            if(!isPrefebObject)
                gameObjectClone.transform.parent = this.gameObject.transform;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
