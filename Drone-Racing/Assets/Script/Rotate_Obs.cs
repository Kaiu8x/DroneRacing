using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Obs : MonoBehaviour {
	public float rotationSpeed = 60.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (rotationSpeed, 0, 0) * Time.deltaTime);
	}
}
