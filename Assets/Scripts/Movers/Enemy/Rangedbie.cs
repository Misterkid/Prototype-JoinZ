    using UnityEngine;
using System.Collections;

public class Rangedbie : Mover 
{
    public Player player;
    public WeaponBase holdingWeapon;
    private bool sawPlayer = false;
    private Timer seeTimer;
    protected override void Start()
    {
        seeTimer = this.gameObject.AddComponent(typeof(Timer)) as Timer;
        base.Start();
        holdingWeapon = GameObject.Instantiate(holdingWeapon) as WeaponBase;


        Destroy(holdingWeapon.gameObject.GetComponent<Collider>());
        Destroy(holdingWeapon.gameObject.GetComponent<Rigidbody>());

        holdingWeapon.UseItem(this);
        //inventory.RemoveItem(inventory.items[i]);
    }

    protected override void Update()
    {
        ShootOnPlayerSight();
        base.Update();
    }

    private void ShootOnPlayerSight()
    {
        if (player != null || player.gameObject != null)
        {
            if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) < 200f)//Distance
            {
                Vector3 targetDir = player.gameObject.transform.position - this.gameObject.transform.position;
                float angle = Vector3.Angle(targetDir, this.gameObject.transform.forward);
                if (angle <= 145 / 2) // target in view angle
                {
                    RaycastHit hit; // check if it's visible
                    if (Physics.Raycast(this.gameObject.transform.position, targetDir, out hit))
                    {
                        if (hit.collider.gameObject == player.gameObject)
                        {
                            if (!sawPlayer)
                            {
                                sawPlayer = true;
                            }
                        }
                        else
                        {
                            if (!seeTimer.IsStarted())
                                seeTimer.StartTimer(2, 0, "sawTimerEnd");
                        }
                    }
                    else
                    {
                        if (!seeTimer.IsStarted())
                            seeTimer.StartTimer(2, 0, "sawTimerEnd");
                    }
                }
                else
                {
                    if (!seeTimer.IsStarted())
                        seeTimer.StartTimer(2, 0, "sawTimerEnd");
                }
            }
            else
            {
                if (!seeTimer.IsStarted())
                    seeTimer.StartTimer(2, 0, "sawTimerEnd");
            }
            if (sawPlayer)
            {
                // this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, player.transform.position, walkingSpeed * Time.deltaTime);
                MOVER_FLAGS.isShooting = true;
                this.gameObject.transform.LookAt(new Vector3(player.transform.position.x, this.gameObject.transform.position.y, player.transform.position.z));
                this.headObject.transform.LookAt(player.transform.position);
            }
        }
    }

    private void UpdateMovement()
    {

    }

    protected virtual void sawTimerEnd()
    {
        if (sawPlayer)
        {
            MOVER_FLAGS.isShooting = false;
            sawPlayer = false;
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
