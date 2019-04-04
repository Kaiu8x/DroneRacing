using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneMovementScript : MonoBehaviour {
	public Text winText;
	Rigidbody ourDrone;
	public float upForce;
	public float movementFowardSpeed = 500.0f;
	public float tiltAmountFoward = 0;
	private float tiltVelocityFoward;
	private float wantedYRotation;
	private float currentYRotation;
	public float rotateAmountByKey = 2.5f;
	private float rotationYVelocity;
	private Vector3 velocityToSmoothDampToZero;
	public float sideMovementAmount = 300.0f;
	private float tiltAmountSideways;
	private float tiltVelocitySideways;
	
	//private AudioSource droneSound;

	void Start () {
		winText.text = "";
	}
	
	void Awake(){
		ourDrone = GetComponent<Rigidbody>();
		//droneSound = gameObject.transform.Find("drone_sound").GetComponent<AudioSource>();
	}

	void FixedUpdate(){
		MovementUpDown();
		MovementFoward();
		Rotation();
		ClampingSpeedValues();
		Swerer();
		//DroneSound();

		ourDrone.AddRelativeForce(Vector3.up * upForce);
		ourDrone.rotation = Quaternion.Euler(new Vector3(tiltAmountFoward, currentYRotation, tiltAmountSideways));
	}

	
	void MovementUpDown(){
		if((Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)){
			
			//if(Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K)){
			//	ourDrone.velocity = ourDrone.velocity;
			//}

			//en frente
			if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L)){
				ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
				upForce = 281;
			}
			//if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)){
			//	ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
			//	upForce = 110;
			//}
			//s y j
			if(Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)){
				upForce = 410;
			}
		}

		//lateral a y d
		//Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f que no pasa vertical
		//Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f) que si pasa horizontal

		if((Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)){
			upForce = 135;
		}

		//frente atras 110
		//lateral 135
		//up 450 down -200
		//
		if(Input.GetKey(KeyCode.I)){
			upForce = 450;
			//if((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)){
			//	upForce = 500;
			//}
		}else if(Input.GetKey(KeyCode.K)){
			upForce = -200;
		}else if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f )){
			upForce = 98.1f;
		}

		//Debug.Log(upForce);
	}

	void MovementFoward(){
		if(Input.GetAxis("Vertical") != 0){
			ourDrone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical")* movementFowardSpeed);
			tiltAmountFoward = Mathf.SmoothDamp(tiltAmountFoward, 20*Input.GetAxis("Vertical"), ref tiltVelocityFoward, 0.1f);
		}
	}

	void Rotation(){
		if(Input.GetKey(KeyCode.J)){
			wantedYRotation -= rotateAmountByKey;
		}
		if(Input.GetKey(KeyCode.L)){
			wantedYRotation += rotateAmountByKey;
		}

		currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
	}

	//Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f que no pasa vertical
	//Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f) que si pasa horizontal
	// >0.2f si pasa
	// <0.2f no pasa
	void ClampingSpeedValues(){
		if(Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
			ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
		}

		if(Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f){
			ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
		}

		if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
			ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
		}

		if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f){
			ourDrone.velocity = Vector3.SmoothDamp(ourDrone.velocity, Vector3.zero, ref velocityToSmoothDampToZero,0.59f);
		}
	}

	void Swerer(){
		if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
			ourDrone.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * sideMovementAmount);
			tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -20 * Input.GetAxis("Horizontal"), ref tiltVelocitySideways, 0.1f);
		}else{
			tiltVelocitySideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltVelocitySideways, 0.1f);
		}
	}

	//void DroneSound(){
	//	droneSound.pitch = 1 + (ourDrone.velocity.magnitude / 100);
	//}

	void OnTriggerEnter(Collider other){
		
		if(other.gameObject.CompareTag ("Gate")){
			winText.text = "Boost!";
			// La logica del boost va aquí
		}
	}
}
