using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCheckPoint : MonoBehaviour {

	// Use this for initialization
	public int id;
	void Start () {
	id = gameObject.GetInstanceID();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
