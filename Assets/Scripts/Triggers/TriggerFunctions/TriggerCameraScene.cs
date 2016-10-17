using UnityEngine;
using System.Collections;

public class TriggerCameraScene : TriggerBaseFunction 
{
    public float rotationSpeed = 0.5f;
    public float moveSpeed = 2;
    public float wayPointRange = 2;
    public GameObject[] Waypoints;
    public float waitTimeEachWaypoint = 5;
    public float timeToDestroyAtEnd = 3;
    public GameObject cameraToMove;
    public GameObject playerCamera;
    private int destinationPoint = 0;
    private bool isActivated = false;
    private bool canMove = true;
    private Timer waitTimer;
    private Timer destroyTimer;
	// Use this for initialization
	void Start () 
    {
        ActivateCamera(false);

        waitTimer = this.gameObject.AddComponent<Timer>();
        destroyTimer = this.gameObject.AddComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isActivated)
        {
            cameraToMove.transform.rotation = Quaternion.Slerp(cameraToMove.transform.rotation, Quaternion.LookRotation(Waypoints[destinationPoint].transform.position - cameraToMove.transform.position), rotationSpeed * Time.deltaTime);
            float distanceToWaypoint = Vector3.Distance(Waypoints[destinationPoint].transform.position, cameraToMove.transform.position);
            if (distanceToWaypoint < wayPointRange)
            {
                if (destinationPoint == (Waypoints.Length - 1))
                {
                    if (!destroyTimer.IsStarted())
                    {
                        destroyTimer.StartTimer(timeToDestroyAtEnd, 0, "OnDestroyTimerEnd");
                        // isActivated = false;
                        Debug.Log("Time To Destroy!");
                    }
                }
                else
                {
                    destinationPoint++;
                }
                canMove = false;
                waitTimer.StartTimer(waitTimeEachWaypoint, 0, "OnWaitTimerEnd");
            }

            if (canMove)
            {
                cameraToMove.transform.position += cameraToMove.transform.forward * moveSpeed * Time.deltaTime; 
            }
            //ToDo
        }
	} 

    public override void DoAction(Mover mover)
    {
        ActivateCamera(true);
        isActivated = true;
        base.DoAction(mover);
    }
    private void OnDestroyTimerEnd()
    {
        ActivateCamera(false);
        Destroy(this);
    }
    private void OnWaitTimerEnd()
    {
       // ActivateCamera(false);
        canMove = true;
    }

    private void ActivateCamera(bool activate)
    {
        cameraToMove.GetComponent<AudioListener>().enabled = activate;
        cameraToMove.GetComponent<Camera>().enabled = activate;

        switch (activate)
        {
            case true:
                playerCamera.gameObject.GetComponent<AudioListener>().enabled = false;
                playerCamera.gameObject.GetComponent<Camera>().enabled = false;
                break;
            case false:
                playerCamera.gameObject.GetComponent<AudioListener>().enabled = true;
                playerCamera.gameObject.GetComponent<Camera>().enabled = true;
                break;
        }
    }
}
