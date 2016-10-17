using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BreakableObjects : MonoBehaviour 
{
	public float health;
	public GameObject brokenObject;
    public AudioClip hitAudioClip;
    public AudioClip breakAudioClip;
	private bool collided = false;
	private bool isDead = false;
	private float impactForce;
	private GameObject brokenObjectClone;
	private Vector3 forceDirection;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
		
	}
    private void Die()
    {
        AudioSource.PlayClipAtPoint(breakAudioClip, this.gameObject.transform.position);

        brokenObjectClone = Instantiate(brokenObject, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
        for (int i = 0; i < /*brokenObjectClone.transform.GetChildCount();*/brokenObjectClone.transform.childCount; i++)
        {
            GameObject brokenObjectCloneChild = brokenObjectClone.transform.GetChild(i).gameObject as GameObject;
            if (brokenObjectCloneChild.GetComponent<Rigidbody>() != null)
            {
                brokenObjectCloneChild.GetComponent<Rigidbody>().AddForce(forceDirection * 0.075f, ForceMode.Impulse);
            }
        }
        isDead = true;
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.GetComponent<Renderer>().enabled = false;

        Destroy(this.gameObject);
        Destroy(this);
    }
	void FixedUpdate()
	{
		if(collided)
		{
			health -= impactForce * 2;
			collided = false;

            AudioSource.PlayClipAtPoint(hitAudioClip, this.gameObject.transform.position);
		}
		if(health <= 0 && !isDead )
		{
            Die();
		}
	}
	void OnCollisionEnter(Collision collision) 
	{
		impactForce = collision.relativeVelocity.magnitude;
		forceDirection = collision.relativeVelocity;
		if(impactForce > 0)
		{
			collided = true;
		}
	}
}