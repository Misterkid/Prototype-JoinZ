using UnityEngine;
using System.Collections;
public class Player : Mover 
{
    public float runningStaReduce = 1;
    public float runningStaRegen = 0.5f;
    public MouseLook mouseLook;
    public float hunger = 100;
    public float thirst = 100;

    public float hungerReduceRate = 1f;
    public float thirstReduceRate = 1f;

    public float maxHunger = 100;
    public float maxThirst = 100;
    public CrossHair crossHair;

    private bool isRunning = false;
    private bool canRun = true;
    private Timer staminaTimer;

    private Timer hungerTimer;
    private Timer thirstTimer;
    private float startFov;
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
        startFov = Camera.main.fieldOfView;
        staminaTimer = this.gameObject.AddComponent<Timer>();

        hungerTimer = this.gameObject.AddComponent<Timer>();
        thirstTimer = this.gameObject.AddComponent<Timer>();

        hungerTimer.StartTimer(hungerReduceRate, 0, "DoHungerTimer");
        thirstTimer.StartTimer(thirstReduceRate, 0, "DoThirstTimer");
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
        UpdateActions();
        if (mouseLook.isEnabled)
        {
            if(canMove)
                UpdateMovement();

            if (Input.GetMouseButtonDown(1))
            {
                if(holdingItem != null)
                {
                    GunBase holdingGun = holdingItem as GunBase;
                    if (holdingGun != null)
                        holdingGun.Zoom(crossHair);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                Camera.main.fieldOfView = startFov;
                crossHair.ResetCrosshair();
                mouseLook.sensitivityX = 15;
                mouseLook.sensitivityY = 15;
            }
        }
		base.Update();
	}
	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
	}
	//Player Moving
	void UpdateMovement()
    {

        //canRun = true;

        if (stamina < 1 && canRun)
            canRun = false;

        if (Input.GetKeyUp(KeyCode.LeftShift) && stamina > 0)
        {
            canRun = true;
        }

		float x = 0;
		float z = 0;
        if (Input.GetKey(KeyCode.LeftShift) && canRun)
		{
			x = Input.GetAxis("Horizontal") * Time.deltaTime * runningSpeed;
		
			if(!MOVER_FLAGS.isLadderMoveUp && !MOVER_FLAGS.isLadderMoveDown)
				z = Input.GetAxis("Vertical") * Time.deltaTime * runningSpeed;

            if (!isRunning)
            {
                if (stamina > 0)
                    stamina -= 1;

                isRunning = true;
            }
            if (!staminaTimer.IsStarted())
            {
                staminaTimer.StartTimer(runningStaReduce, 0, "DoStaminaTimer");
            }

		}
		else
		{
			x = Input.GetAxis("Horizontal") * Time.deltaTime * walkingSpeed;
		
			if(!MOVER_FLAGS.isLadderMoveUp && !MOVER_FLAGS.isLadderMoveDown)
				z = Input.GetAxis("Vertical") * Time.deltaTime * walkingSpeed;

            if (isRunning)
            {
                if (stamina < 100)
                    stamina += 1;
                isRunning = false;
            }
            if (!staminaTimer.IsStarted())
            {
                staminaTimer.StartTimer(runningStaRegen, 0, "DoStaminaTimer");
            }


		}
		this.gameObject.transform.Translate(new Vector3(x,0,z));
		
		if(MOVER_FLAGS.isClimbingLadder)
		{
			if(Input.GetAxis("Vertical") > 0)
			{
				MOVER_FLAGS.isLadderMoveUp = true;
				MOVER_FLAGS.isLadderMoveDown = false;
			}
			if(Input.GetAxis("Vertical") < 0)
			{
				MOVER_FLAGS.isLadderMoveUp = false;
				MOVER_FLAGS.isLadderMoveDown = true;
			}
			if(Input.GetAxis("Vertical") == 0)
			{
				MOVER_FLAGS.isLadderMoveUp = false;
				MOVER_FLAGS.isLadderMoveDown = true;
			}
		}
	}
	void UpdateActions()
	{
        //In Gui mode
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            mouseLook.isEnabled = false;
            crossHair.showGameCursor = true;

            Cursor.visible = true;
            Screen.lockCursor = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            mouseLook.isEnabled = true;
            crossHair.showGameCursor = false;

            Screen.lockCursor = true;
        }

        if (mouseLook.isEnabled)
        {
            //Player Pickup Objects
            if (Input.GetKeyDown(KeyCode.E))
            {
                MOVER_FLAGS.isUsingHoldKey = true;
                MOVER_FLAGS.isPickUpItem = true;
            }
            if (Input.GetKeyUp(KeyCode.E) && !MOVER_FLAGS.isClimbingLadder)
            {
                MOVER_FLAGS.isUsingHoldKey = false;
            }
            if (Input.GetKeyUp(KeyCode.E) && MOVER_FLAGS.isClimbingLadder)
            {
                MOVER_FLAGS.isClimbingLadder = false;
            }
            if (Input.GetMouseButtonDown(0) && MOVER_FLAGS.isUsingHoldKey)
            {
                MOVER_FLAGS.isThrowObject = true;
                MOVER_FLAGS.isLadderMoveUp = false;
                MOVER_FLAGS.isLadderMoveDown = false;
            }
            //Player Shoot
            if (Input.GetMouseButtonDown(0) && !MOVER_FLAGS.isShooting && !MOVER_FLAGS.isUsingHoldKey && !MOVER_FLAGS.isClimbingLadder)
            {
                MOVER_FLAGS.isShooting = true;
            }
            if (Input.GetMouseButtonUp(0) && MOVER_FLAGS.isShooting)
            {
                MOVER_FLAGS.isShooting = false;
            }
            if (Input.GetKeyDown(KeyCode.R) && !MOVER_FLAGS.isReloading)
            {
                MOVER_FLAGS.isReloading = true;
            }

            //Player Jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (MOVER_FLAGS.isGrounded)
                    MOVER_FLAGS.isJumping = true;
            }
        }
	}
    void DoStaminaTimer()
    {

        if (isRunning)
        {
            if (stamina > 0)
                stamina -= 1;
        }
        else
        {
            if (stamina < 100)
                stamina += 1;
        }
    }
    protected override void Die()
    {
        base.Die();

        mouseLook.isEnabled = false;
        crossHair.showGameCursor = true;
        Screen.lockCursor = false;
        Cursor.visible = true;

        Application.LoadLevel(0);
    }
    void DoHungerTimer()
    {

        if (hunger <= 0)
        {
            ApplyDamage(this.health);
        }
        else
        {
            hunger -= 1;
            hungerTimer.StartTimer(hungerReduceRate, 0, "DoHungerTimer");
        }

    }
    void DoThirstTimer()
    {

        if (thirst <= 0)
        {
            ApplyDamage(this.health);
        }
        else
        {
            thirst -= 1;
            thirstTimer.StartTimer(hungerReduceRate, 0, "DoThirstTimer");

        }
    }
	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		float collisionPain = collision.relativeVelocity.magnitude;
		if(collisionPain > 8f)
		{
            this.ApplyDamage((int)collisionPain * 2);
		}
	}
}
