using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
	
	public AudioSource doorOpenSound;
	public AudioSource doorCloseSound;
	public GameObject doorModel;
	public Player player;
	public float doorOpenCloseSpeed = 50;
	private bool doorIsOpen = false;
	private bool isOpeningDoor = false;
	private bool isClosingDoor = false;
	//private Timer timer;
	// Use this for initialization
	void Start () 
	{
		//timer = this.gameObject.AddComponent(typeof(Timer)) as Timer;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Vector3.Distance(doorModel.transform.position,player.transform.position) < 1f && doorModel.GetComponent<Renderer>().isVisible == true)
		{	
			if(Input.GetKeyUp(KeyCode.E))
			{
				if(!doorIsOpen)
					isOpeningDoor = true;
				else
					isClosingDoor = true;
			}
		}
		if(isOpeningDoor)
		{
			if(this.transform.eulerAngles.y != 90)
				this.gameObject.transform.eulerAngles = Vector3.MoveTowards(this.gameObject.transform.eulerAngles,new Vector3(0,90,0),doorOpenCloseSpeed * Time.deltaTime);
			
			if(this.transform.eulerAngles.y >= 90)
			{
				isOpeningDoor = false;	
				doorIsOpen = true;
				//timer.StartTimer(1.5f,0f,"OnTimerEnd");

			}
		}
		if(isClosingDoor)
		{
			if(this.transform.eulerAngles.y != 0)
				this.gameObject.transform.eulerAngles = Vector3.MoveTowards(this.gameObject.transform.eulerAngles,new Vector3(0,0,0),doorOpenCloseSpeed * Time.deltaTime);
			
			if(this.transform.eulerAngles.y <= 0)
			{
				isClosingDoor = false;
				doorIsOpen = false;
			}
			
		}
	}
	void DoDoorOpen()	
	{
		
		this.gameObject.transform.Rotate(new Vector3(0,90,0));
	}
	void OnTimerEnd()
	{
		isClosingDoor = true;
	}
}
