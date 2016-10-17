//Based on one of the unity standard mouselook script.
//touch control added by Eddy Meivogel. Http://www.Eddymeivogel.com
#define __HEAD_BASED
//#define __NEW_VERSION
using UnityEngine;
using System.Collections;


#if !__NEW_VERSION
public class MouseLook : MonoBehaviour
{
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

	public bool isEnabled = false;
	public GameObject headObject;
	//public GameObject cameraObject
	void Update ()
	{
#if !UNITY_ANDROID && !UNITY_IPHONE
		if(isEnabled)
		{
			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
#if __HEAD_BASED
				headObject.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
				transform.localEulerAngles = new Vector3(0,rotationX, 0);
#else
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
#endif
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}
#else
		/*touch control */
		if (Input.touchCount > 0)//If we touch :O
		{
			Touch fingerTouch = Input.GetTouch (0);//our first finger tough :o
	        if(Input.touchCount == 1)//If only 1 finger touches hehe 
			{
				switch(fingerTouch.phase)//Our finger phase ;)
				{
					case TouchPhase.Moved://ohhhhhh I moved ;p
						float rotationX = transform.localEulerAngles.y + fingerTouch.deltaPosition.x * (sensitivityX / 15);
						rotationY += fingerTouch.deltaPosition.y * (sensitivityY / 15);
						rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
						transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
					break;
					
					default: break;
				}
				
			}
			
		}
#endif
	}
}
#else
public class MouseLook : MonoBehaviour
{
    public GameObject headObject;
    void Update()
    {
        Vector2 MousePosition = Input.mousePosition;
        MousePosition.x = (Screen.height / 2) - Input.mousePosition.y;
        MousePosition.y = -(Screen.width / 2) + Input.mousePosition.x;
       // headObject.transform.Rotate(MousePosition * Time.deltaTime * 1, Space.Self);

        headObject.transform.localEulerAngles = new Vector3(-MousePosition.y, 0, 0);
        transform.localEulerAngles = new Vector3(0, MousePosition.x, 0);
        //headObject.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
       // transform.localEulerAngles = new Vector3(0, rotationX, 0);
    }
}

#endif

