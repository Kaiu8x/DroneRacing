using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class XboxDroneControllerEasy : MonoBehaviour
{

	Rigidbody ourDrone;
	public float speed = 90.0f;
  	//float gravity = 9.81f;

	void Awake ()
	{
		ourDrone = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate()
	{

		MovementForward();
		Rotation();

	}

	void MovementForward()
	{
		ourDrone.transform.position += XCI.GetAxis(XboxAxis.RightTrigger) * transform.forward * Time.deltaTime * speed;
	}

	void Rotation()
	{
		ourDrone.transform.Rotate(XCI.GetAxis( XboxAxis.LeftStickY), XCI.GetAxis(XboxAxis.RightStickX) , -XCI.GetAxis(XboxAxis.LeftStickX));
	}
}
