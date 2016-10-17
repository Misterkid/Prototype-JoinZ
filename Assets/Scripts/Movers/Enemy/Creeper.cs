using UnityEngine;
using System.Collections;
using System;
public class Creeper : Zombie 
{
    public float explosionForce = 100f;
    public float explosionRad = 25f;
    private bool isExploded = false;
    public GameObject explosionParticle;
    //public AudioClip explosionClip;
    protected override void Start()
    {
        attackDelay = 0.5f;
        base.Start();
    }
    protected override void  Update()
    {
        base.Update();
    }
    protected override void attackTimerEnd()
    {
        if(this.health > 0)
            doExplosion(this.health);
    }
    protected override void Die()
    {
       // if (this.health == 0)
        //{
           // this.health = 20;
        //}
        try
        {
            doExplosion(this.health);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
        base.Die();
    }
    public void doExplosion(float selfDamage)
    {
        //playExplosion();
       // playSound(explosionClip, AudioRolloffMode.Custom, false);
       // deathAudioClip = explosionClip;
        if (!isExploded)
        {
            /*
            if (this.health < 20)
            {
                selfDamage = 20;
            }
             */
            GameObject newParticle = GameObject.Instantiate(explosionParticle, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;

            if (selfDamage < 1)
            {
                selfDamage = 20;
            }
            else
            {
                ApplyDamage(selfDamage);
            }

            isExploded = true;
            Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, explosionRad);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != this.gameObject)
                {
                    if (colliders[i].GetComponent<Rigidbody>())
                    {
                        colliders[i].gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, this.gameObject.transform.position, explosionRad, 3.0f);
                    }
                    Mover mover = colliders[i].GetComponent<Mover>();
                    if (mover)
                    {
                        float distance = Vector3.Distance(this.gameObject.transform.position, colliders[i].gameObject.transform.position);
                        //mover.ApplyDamage((this.health / distance) + 10);
                        // mover.ApplyDamage((this.health / (distance > 0 ? distance : 1)) + 10);
                        mover.ApplyDamage((distance / explosionRad) * selfDamage);
                       // if(distance != 0)
                           // mover.ApplyDamage((distance > 0 ? selfDamage / distance : 0) + 10);
                    }
                }
            }
        }
    }
    protected override void UpdateAttack()
    {
        if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) < 1f)
        {
            if (!attackTimer.IsStarted())
            {
                attackTimer.StartTimer(attackDelay, 0, "attackTimerEnd");
                canMove = false;
            }
        }
    }
    /*
    protected override void UpdateAttack()
    {
        base.UpdateAttack();
        if(attackTimer.IsStarted() && canMove)
        {
            canMove = false;
        }
    }
     * */
}
