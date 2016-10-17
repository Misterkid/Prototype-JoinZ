using UnityEngine;
using System.Collections;

public class MeleeBase : WeaponBase 
{
    public float damage;
    public float distance = 0.5f;
    public float delay = 0.5f;
    public AudioClip hitSound;
    private Timer hitTimer;
    private bool canHit = true;
	// Use this for initialization
	void Start ()
    {
        hitTimer = this.gameObject.AddComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () 
    {

	}
    public void Hit()//Douchebags.
    {
        if (canHit)
        {
            RaycastHit hit;
            Ray  ray = Camera.main.ScreenPointToRay(new Vector3 (Screen.width / 2, Screen.height / 2,0));//CrossHair
            if (Physics.Raycast(ray, out hit))
            {
                if(Vector3.Distance(this.gameObject.transform.position,hit.collider.gameObject.transform.position) < distance)
                {
                    Mover hitMover = hit.collider.gameObject.GetComponent<Mover>();
                    if (hitMover)
                    {
                        hitMover.ApplyDamage(damage);
                    }
                    if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
                    {
                        hit.collider.gameObject.GetComponent<Rigidbody>().AddForce((this.gameObject.transform.forward * damage), ForceMode.Impulse);
                    }
                }
            }
            canHit = false;
        }
        if (!hitTimer.IsStarted())
        {
            hitTimer.StartTimer(delay, 0, "HitIt");
        }
    }
    private void HitIt()
    {
        canHit = true;
    }
    public override void UseItem(Mover mover)
    {
        mover.holdingItem = this;
        this.gameObject.transform.parent = mover.meleePosition.transform;
        this.gameObject.transform.position = mover.meleePosition.transform.position;
        this.gameObject.transform.rotation = mover.meleePosition.transform.rotation;

        base.UseItem(mover);
    }
}
