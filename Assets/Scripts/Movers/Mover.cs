//Unity Mover class base class for all moving things like NPC ,Monster,Player.
//Author: Eduard Meivogel

using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	public float walkingSpeed = 5;//Speed!
	public float runningSpeed = 10;//Speed when running!
	public MoverFlags MOVER_FLAGS;// Our "flags" :P When are we going to jump etc.
	public float jumpingSpeed = 35;//Jumping speed or "Height"
	public GameObject headObject;//Our Headobject(So we can trhow our object right!)
    public float maxHealth = 100;
	public float health = 100;
    public Item holdingItem;
    public float stamina = 100;
    public Inventory inventory;
    public GameObject gunPosition;
    public GameObject meleePosition;
    public AudioClip deathAudioClip;
    protected bool canMove = true;
	private bool isHoldingRigedbody = false;//Are we holding a rigedbody ?
	private Vector3 holdedRigedScale;//A scale before holding
	private float holdedRigedMass = 0;//The mass of the holding riged body
	private GameObject holdingRigedObject;//The holding Object
	private Transform holdingRigedParent;
    private GameObject objectBelowToFall;
	// Use this for initialization
	protected virtual void Start()
	{
		MOVER_FLAGS = new MoverFlags();
        inventory = this.gameObject.AddComponent<Inventory>();
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
        UseWeapon();
        ReloadGun();
	}
	protected virtual void FixedUpdate()
	{
		ClimbLadder();
		Jump();
        PickUpItem();
		PhysicPickUp();
	}
    private GameObject GetTargetObject()
    {
        RaycastHit  hit;
        Ray  ray = Camera.main.ScreenPointToRay(new Vector3 (Screen.width / 2, Screen.height / 2,0));//CrossHair
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
    private void PickUpItem()
    {
        if (MOVER_FLAGS.isPickUpItem)
        {
            GameObject itemObject = GetTargetObject();
            Item item = itemObject.GetComponent<Item>();

            if (item != null && itemObject != null)
            {
                if (inventory.AddItem(item))
                {
                    AudioSource.PlayClipAtPoint(item.pickUpSound, this.transform.position);
                    item.oldObjectScale = item.gameObject.transform.localScale;
                    item.gameObject.GetComponent<Collider>().enabled = false;
                    item.gameObject.GetComponent<Renderer>().enabled = false;
                    Destroy(item.gameObject.GetComponent<Rigidbody>());
                    Debug.Log("Item added:" + item.itemName);
                }
            }
            MOVER_FLAGS.isPickUpItem = false;
        }
    }
	private void PhysicPickUp()
	{
		if (MOVER_FLAGS.isUsingHoldKey && !isHoldingRigedbody)
        {
            holdingRigedObject = GetTargetObject();
            if (holdingRigedObject == null || holdingRigedObject.GetComponent<Rigidbody>() == null)
            {
                return;
            }
			if(Vector3.Distance(this.gameObject.transform.position,holdingRigedObject.transform.position) < 1.5f && holdingRigedObject.GetComponent<Renderer>().isVisible == true)
			{
				holdedRigedScale = holdingRigedObject.transform.localScale;
				holdingRigedParent = holdingRigedObject.transform.parent;
				holdingRigedObject.transform.parent = headObject.transform;
				holdedRigedMass = holdingRigedObject.GetComponent<Rigidbody>().mass;
				Destroy(holdingRigedObject.GetComponent<Rigidbody>());
				isHoldingRigedbody = true;
			}
        }
		if(!MOVER_FLAGS.isUsingHoldKey && isHoldingRigedbody)
		{
			PhysicThrow();
		}
		if(MOVER_FLAGS.isThrowObject && isHoldingRigedbody)
		{
			PhysicThrow(10);
			MOVER_FLAGS.isThrowObject = false;
			MOVER_FLAGS.isUsingHoldKey = false;
		}
	}
	private void PhysicThrow(float force = 0)
	{
		if(holdingRigedObject.GetComponent<Rigidbody>() == null)
		{
			holdingRigedObject.transform.parent = holdingRigedParent;
			holdingRigedObject.AddComponent(typeof(Rigidbody));
			holdingRigedObject.GetComponent<Rigidbody>().mass = holdedRigedMass;
			holdingRigedObject.transform.localScale = holdedRigedScale;
			if(force != 0)
				holdingRigedObject.GetComponent<Rigidbody>().AddForce(headObject.transform.forward * force,ForceMode.Impulse);
			
			isHoldingRigedbody = false;
		}
	}
	private void ClimbLadder()
	{
		if(MOVER_FLAGS.isClimbingLadder)
		{
			if(MOVER_FLAGS.isLadderMoveDown)
			{
				this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, -3, 0);
				//this.gameObject.rigidbody.MovePosition(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y -(1f * Time.deltaTime), this.gameObject.transform.position.z));
			}
			if(MOVER_FLAGS.isLadderMoveUp)
			{
				this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 3, 0);
				//this.gameObject.rigidbody.MovePosition(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + (1f* Time.deltaTime), this.gameObject.transform.position.z));
			}
		}
	}
    private void UseWeapon()
    {
        if (holdingItem != null && MOVER_FLAGS.isShooting)
        {
            switch (holdingItem.GetType().ToString())
            {
                case "GunBase":
                    GunBase holdingGun = holdingItem as GunBase;
                        holdingGun.Shoot();
                    break;

                case "MeleeBase":
                    MeleeBase holdingMelee = holdingItem as MeleeBase;
                        holdingMelee.Hit();
                    break;
            }

        }
    }
    private void ReloadGun()
    {
        if (holdingItem != null && MOVER_FLAGS.isReloading)
        {
            if (holdingItem.GetType().ToString() == "GunBase")
            {
                GunBase holdingGun = holdingItem as GunBase;
                holdingGun.Reload();
                MOVER_FLAGS.isReloading = false;
            }
        }
    }
    /*
    public void playSound(AudioClip soundClip, AudioRolloffMode rollOfMode = AudioRolloffMode.Logarithmic, bool loop = false, int maxDistance = 500)
    {
        AudioSource.PlayClipAtPoint(impactSound, this.gameObject.transform.position);
        audioSource.Stop();
        if (soundClip == null)
            return;
        this.audioSource.rolloffMode = rollOfMode;
        audioSource.clip = soundClip;
        audioSource.loop = loop;
        audioSource.maxDistance = maxDistance;
        audioSource.Play();
    }
     */
    public void ApplyDamage(float amount)
    {
        this.health -= amount;
        if (this.health <= 0)
        {   
            Die();
        }
        
    }
    protected virtual void Die()
    {
        this.gameObject.GetComponent<Renderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject.GetComponent<Rigidbody>());

        if(deathAudioClip != null)
            AudioSource.PlayClipAtPoint(deathAudioClip, this.gameObject.transform.position);

        foreach (Component moverComponent in this.gameObject.GetComponents<MonoBehaviour>())
        {
            Destroy(moverComponent);
        }
        Destroy(this.gameObject);
    }
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.GetComponent(typeof(Ladder)) != null)
		{
			MOVER_FLAGS.isClimbingLadder = true;
		}
	}
	
	void OnTriggerExit(Collider other) 
	{
		if(other.gameObject.GetComponent(typeof(Ladder)) != null)
		{
			MOVER_FLAGS.isLadderMoveDown = false;
			MOVER_FLAGS.isLadderMoveUp = false;
			MOVER_FLAGS.isClimbingLadder = false;
		}
	}
    private void Jump()
    {
        if (MOVER_FLAGS.isJumping && MOVER_FLAGS.isGrounded)
        {
            MOVER_FLAGS.isGrounded = false;
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpingSpeed, 0), ForceMode.Impulse);
        }
        /*
        if (MOVER_FLAGS.isJumping)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, Vector3.down, out hit))
            {
                objectBelowToFall = hit.collider.gameObject;
            }
        }
         * */
    }
	protected virtual void OnCollisionEnter(Collision collision) 
	{
        // MOVER_FLAGS.isJumping = false;
        // MOVER_FLAGS.isGrounded = true;
        //  Debug.Log("Jumped");
	}
    protected virtual void OnCollisionStay(Collision collision)
    {
        if (!MOVER_FLAGS.isGrounded && !MOVER_FLAGS.isClimbingLadder)//Are we in the air and not climbin a ladder or the colliding object is not us?
        {
            for (int i = 0; i < collision.contacts.Length; i++)//Get all contact points
            {
                if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) < 60)//In what angle are we hitting ?(How is that damn object rotated)
                {
                    MOVER_FLAGS.isJumping = false;
                    MOVER_FLAGS.isGrounded = true;
                    //canMove = true;
                    //this.rigidbody.AddForce(collision.contacts[i].normal * -10, ForceMode.Impulse);
                    // Debug.Log("Jumped");
                }
                else//if it is higher then 60 degrees we will fall and be unable to move :o
                {
                    //canMove = false;
                    this.GetComponent<Rigidbody>().AddForce(collision.contacts[i].normal /2  /*/ 10 */, ForceMode.Impulse);
                    //Debug.Log(Vector3.Angle(collision.contacts[i].normal, Vector3.up));
                    //Debug.Log(collision.contacts[i]);
                   // Debug.Log("Mover is unable to move: " + Vector3.Angle(collision.contacts[i].normal, Vector3.up));
                }
               // Debug.Log(collision.contacts[i].normal);
                //Debug.Log(collision.contacts[i].point);
            }
        }
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        MOVER_FLAGS.isGrounded = false;
    }
}

public class MoverFlags
{
    //Throwing and holding
	public bool isUsingHoldKey = false;
	public bool isThrowObject = false;
	//Jumping
	public bool isJumping = true;
	public bool isGrounded = false;
	//Ladder
	public bool isClimbingLadder = false;
	public bool isLadderMoveUp = false;
	public bool isLadderMoveDown = false;
    //Shooting & reloading
    public bool isShooting = false;
    public bool isReloading = false;
    //Picking up Items
    public bool isPickUpItem = false;

}
