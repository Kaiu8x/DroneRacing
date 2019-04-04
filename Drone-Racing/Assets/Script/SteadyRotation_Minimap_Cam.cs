using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteadyRotation_Minimap_Cam : MonoBehaviour {

	public Transform player;
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3(player.position.x, this.transform.position.y, player.position.z);
	}
}
