using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour 
{
    public int triggerCount = 0;
    public int maxTriggerCount = 1;
    //private bool isTrigered = false;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    void OnTriggerEnter (Collider collider)
    {
        if (triggerCount >= maxTriggerCount && maxTriggerCount != 0)
        {
           // Destroy(this.gameObject);
        }
        else
        {
            triggerCount++;
            Mover colliderMover = collider.gameObject.GetComponent<Mover>();
            TriggerBaseFunction[] triggerFunctions = this.gameObject.GetComponents<TriggerBaseFunction>();
            for (int i = 0; i < triggerFunctions.Length; i++)
            {
                triggerFunctions[i].DoAction(colliderMover);
            }
            //Debug.Log("DoThings");
        }
    }
}
