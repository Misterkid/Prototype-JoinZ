using UnityEngine;
using System.Collections;

public class BulletBase : MonoBehaviour 
{
    public float bulletDmg = 10f;
    public float bulletSpeed = 10f;
    public GameObject bulletParticle;

    public bool isExplosive = false;
    public float explosionForce = 100f;
    public float explosionRad = 25f;
  //  public GameObject explosionParticle;

    public AudioClip impactSound;

    private bool impacted = false;
    private Vector3 target;
    private GameObject targetObject;
    private bool move = false;
    private Mover gunHolder;
	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!impacted)
        {

            if (targetObject == null)
            {
                Destroy(this.gameObject);
            }
            if (move)
            {
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, target, bulletSpeed * Time.deltaTime);
            }
            if (Vector3.Distance(this.gameObject.transform.position, target) < 1f)
            {
                if (targetObject == null)
                {
                    return;
                }
                if (isExplosive)
                {
                    Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, explosionRad);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i].gameObject != this.gameObject)
                        {
                            if (colliders[i].GetComponent<Rigidbody>())
                            {
                                colliders[i].gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, this.gameObject.transform.position, explosionRad, 3.0f);
                            }
                            Mover hurtMover = colliders[i].GetComponent<Mover>();
                            if (hurtMover)
                            {
                                float distance = Vector3.Distance(this.gameObject.transform.position, colliders[i].gameObject.transform.position);
                                hurtMover.ApplyDamage((distance > 0 ? bulletDmg / distance : 0) + 10);
                            }
                        }
                    }
                }

                Mover mover = targetObject.GetComponent<Mover>();
                if (mover != null)
                {
                    // if (!isExplosive)
                    //{
                    if(mover.GetType().ToString() != "Player")
                        mover.gameObject.transform.LookAt(gunHolder.gameObject.transform);

                    mover.ApplyDamage(bulletDmg);
                    //}
                }
                if (targetObject.GetComponent<Rigidbody>() != null)
                {
                    if (!isExplosive)
                    {
                        targetObject.GetComponent<Rigidbody>().AddForce((this.gameObject.transform.forward * bulletDmg), ForceMode.Impulse);
                    }
                }

                if (bulletParticle != null)
                {
                    GameObject newParticle = GameObject.Instantiate(bulletParticle, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
                }
                //Destroy(this.gameObject);
                //Die();
                impacted = true;
                if(impactSound != null)
                    AudioSource.PlayClipAtPoint(impactSound, this.gameObject.transform.position);

                Destroy(this.gameObject.GetComponent<Renderer>());
                Destroy(this.gameObject.GetComponent<Collider>());
                Destroy(this.gameObject);
                Destroy(this);
            }
        }
    }
    /*
    IEnumerator Die()
    {
        //this.gameObject.renderer.enabled = false;
       // this.gameObject.collider.enabled = false;
        Destroy(this.gameObject.renderer);
        Destroy(this.gameObject.collider);
        if (impactSound != null)
        {
            if (isExplosive)
            {
               audioSource.rolloffMode = AudioRolloffMode.Custom;
            }
            audioSource.clip = impactSound;
            audioSource.Play();
        }
        if (audioSource.clip != null)
        {
            yield return new WaitForSeconds(audioSource.clip.length);

        }
        Destroy(this.gameObject);
        Destroy(this);
    }
    */
    public void Move(float spread,Mover holder)
    {
        gunHolder = holder;
        RaycastHit hit;
        //Ray  ray = Camera.main.ScreenPointToRay(new Vector3 (Screen.width / 2, Screen.height / 2,0));//CrossHair This is cheating!
       // if (Physics.Raycast(ray, out hit))
        if (Physics.Raycast(gunHolder.holdingItem.gameObject.transform.position, gunHolder.headObject.gameObject.transform.forward, out hit))//Improve this!
        {
            target = hit.point;//Target
            target = new Vector3(target.x + Random.Range(-spread, spread), target.y + Random.Range(-spread, spread), target.z  + Random.Range(-spread, spread));//Add some spread
            this.gameObject.transform.LookAt(target);//Where does the bullet go?

            gunHolder.GetComponent<Collider>().enabled = false;//To ignore raycast for this single object
            //Time to find out our hit
            if (Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hit))
            {
                targetObject = hit.collider.gameObject;//target object
                target = hit.point;//new target
            }
            this.gameObject.transform.LookAt(target);
            gunHolder.GetComponent<Collider>().enabled = true;//To ignore raycast for this single object
            move = true;//Bullet can move
        }
    }
}
