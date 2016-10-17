using UnityEngine;
using System.Collections;

public class TriggerBaseFunction : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public virtual void DoAction(Mover mover)
    {
        Debug.Log("Something Happends to:" + mover.gameObject.name);
    }
}
