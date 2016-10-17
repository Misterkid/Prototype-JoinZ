using UnityEngine;
using System.Collections;
public class Zombie : Mover 
{
	public float attackDelay = 2;
	public Player player;
    public AudioClip zombieSoundClip;
   // protected bool canMove = true;
	private bool sawPlayer = false;
	private Timer seeTimer;
	protected Timer attackTimer; 
	// Use this for initialization
	protected override void Start () 
	{
		seeTimer = this.gameObject.AddComponent(typeof(Timer)) as Timer;
		attackTimer = this.gameObject.AddComponent(typeof(Timer)) as Timer;
        if(zombieSoundClip != null)
            AudioSource.PlayClipAtPoint(zombieSoundClip, this.transform.position);

		base.Start();
	}
	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
	}
	// Update is called once per frame
	protected override void Update () 
	{
		if(player != null)
		{
            if (canMove)
            {
                UpdateMovement();
            }

			UpdateAttack();
		}
		base.Update();
		
	}
	protected virtual void UpdateAttack()
	{
		if(Vector3.Distance(this.gameObject.transform.position,player.transform.position) < 1f)
		{
			if(!attackTimer.IsStarted())
			{
				//this.DoAttack(player);
				attackTimer.StartTimer(attackDelay,0,"attackTimerEnd");
			}
		}
		else if(attackTimer.IsStarted())
		{
			attackTimer.StopTimer();
			attackTimer.ResetTimer();
		}
	}
    protected virtual void UpdateMovement()
	{
		if(Vector3.Distance(this.gameObject.transform.position,player.transform.position) < 35f)
		{
			Vector3 targetDir = player.gameObject.transform.position - this.gameObject.transform.position;
			float angle = Vector3.Angle(targetDir, this.gameObject.transform.forward);
			if (angle <= 145/2)
			{ // target in view angle
				RaycastHit  hit; // check if it's visible
				if (Physics.Raycast(this.gameObject.transform.position, targetDir, out hit))
				{
					if(hit.collider.gameObject == player.gameObject)
					{
                        if (!sawPlayer)
                        {
                            sawPlayer = true;
                        }
					}
					else
					{
						if(!seeTimer.IsStarted())
					 		seeTimer.StartTimer(2,0,"sawTimerEnd");	
					}
				}
				else
				{
					if(!seeTimer.IsStarted())
					 	seeTimer.StartTimer(2,0,"sawTimerEnd");
				}
			}
			else
			{
				if(!seeTimer.IsStarted())
				 	seeTimer.StartTimer(2,0,"sawTimerEnd");
			}
		}
		else
		{
			if(!seeTimer.IsStarted())
				 seeTimer.StartTimer(2,0,"sawTimerEnd");
		}
		if(sawPlayer)
		{
			this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,player.transform.position,walkingSpeed * Time.deltaTime);
			this.gameObject.transform.LookAt(new Vector3(player.transform.position.x,this.gameObject.transform.position.y,player.transform.position.z));
		}
	}
	protected override void OnCollisionEnter(Collision collision) 
	{
		base.OnCollisionEnter(collision);
		if( collision.rigidbody != null)
		{
			if(collision.gameObject.GetComponent(typeof(Mover)) == null)
			{
				float impactForce = collision.relativeVelocity.magnitude;
				if(impactForce > 5)
				{
					impactForce = impactForce * collision.rigidbody.mass;
                    ApplyDamage(impactForce * 5);
					this.gameObject.GetComponent<Rigidbody>().AddForce(collision.relativeVelocity,ForceMode.Impulse);
				}
			}
		}
	}
    protected virtual void sawTimerEnd()
	{
		if(sawPlayer)
		{
			sawPlayer = false;
		}
	}
    protected virtual void attackTimerEnd()
	{
        player.ApplyDamage(10);
	}
}
