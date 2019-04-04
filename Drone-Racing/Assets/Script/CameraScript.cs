using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class CameraScript : MonoBehaviour {

	public GameObject drone;
	public float up = 8.0f;
	public float foward = 11.0f;
	public float x = 50.0f;
	public float bias = 0.25f;
	

	void Awake () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		CameraSwitch();

	}

	public bool fpvcam = false;
	void CameraSwitch(){
		if(XCI.GetButtonDown(XboxButton.Start)){
			if(fpvcam){
				fpvcam = false;
			}else{
				fpvcam = true;
			}
		}

		if(fpvcam){
			Camera.main.transform.localPosition = new Vector3(0,9.6f,5.7f);
			Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(0.25f, 0, 0));
			}else{
				Vector3 moveCamTo = drone.transform.position - drone.transform.forward * foward + Vector3.up * up;
				Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
				Camera.main.transform.LookAt(drone.transform.position + drone.transform.forward * x);
			}
	}
}






































