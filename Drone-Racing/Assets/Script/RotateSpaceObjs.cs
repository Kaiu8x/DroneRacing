using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpaceObjs : MonoBehaviour {
	public int speed = 5;

	// Update is called once per frame
	void Update () {
    	transform.Rotate(0,speed*Time.deltaTime, 0);
	}
}
