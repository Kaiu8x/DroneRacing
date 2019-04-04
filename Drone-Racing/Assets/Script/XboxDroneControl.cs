using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class XboxDroneControl : MonoBehaviour
{

  Rigidbody ourDrone;
  float gravity = 9.81f;
  public XboxController xboxController;
  //public float positionX;
  //public float positionY;
  //public float positionZ;

  void Start() {
  }
  void Awake()
  {
    ourDrone = GetComponent<Rigidbody>();
    droneSound = gameObject.transform.Find("DroneSound").GetComponent<AudioSource>();
  }

  void FixedUpdate()
  {
    MovementUpDown();
    Rotation();
    MovementForward();
    Swerer();

    ClampingSpeedValues();
    CameraSwitch();
    //CameraFollow();
    DroneSound();


    //if(XCI.GetAxis(XboxAxis.LeftStickY,xboxController)==0 && XCI.GetAxis(XboxAxis.LeftStickX,xboxController)==0 && XCI.GetAxis(XboxAxis.RightStickY,xboxController)==0 && XCI.GetAxis(XboxAxis.RightStickX,xboxController)==0)
    //Hover();

    ourDrone.AddRelativeForce(Vector3.up * upForce);
    ourDrone.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways));
    //Debug.Log(XCI.GetAxis(XboxAxis.LeftStickY,xboxController));

  }

  public float upForce;
  public float addedSpeedUp = 190.0f; //change to private
  public float addedSpeedDown = 80.0f; //change to private
  void MovementUpDown()
  {

    if (XCI.GetAxis(XboxAxis.RightStickX,xboxController) != 0 || XCI.GetAxis(XboxAxis.RightStickY,xboxController) != 0)
    {
      if (XCI.GetAxis(XboxAxis.LeftStickY,xboxController) == 0 && XCI.GetAxis(XboxAxis.LeftStickX,xboxController) == 0)
      {
        ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
        //ourDrone.velocity = new Vector3(Mathf.Lerp(ourDrone.velocity.x, 0, Time.deltaTime * 5), Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);

        //upForce = 281.0f;
        //Debug.Log("l.45");
      }

      if (XCI.GetAxis(XboxAxis.LeftStickX,xboxController) != 0)
      {
        //upForce = 410.0f;
      }
    }

    if (XCI.GetAxis(XboxAxis.LeftStickY,xboxController) > 0)
    {
      upForce = XCI.GetAxis(XboxAxis.LeftStickY,xboxController) * addedSpeedUp;
    }
    else if (XCI.GetAxis(XboxAxis.LeftStickY,xboxController) < 0)
    {
      upForce = XCI.GetAxis(XboxAxis.LeftStickY,xboxController) * addedSpeedDown;
    }
    else if (XCI.GetAxis(XboxAxis.LeftStickY,xboxController) == 0)
    {
      upForce = ourDrone.mass * gravity + Random.Range(-3.0f, 3.0f);
    }
  }

  private float currentYRotation;
  private float wantedYRotation;
  private float rotationYVelocity;
  public float rotateAmountByKey = 1.8f;
  void Rotation()
  {
    if (XCI.GetAxis(XboxAxis.LeftStickX,xboxController) * -1 > 0)
    {
      wantedYRotation -= rotateAmountByKey;
    }
    if (XCI.GetAxis(XboxAxis.LeftStickX,xboxController) * -1 < 0)
    {
      wantedYRotation += rotateAmountByKey;
    }
    currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
  }

  public float movementForwardSpeed = 400.0f;
  public float tiltAmountForward = 0;
  private float tiltVelocityForward;
  void MovementForward()
  {
    if (XCI.GetAxis(XboxAxis.RightStickY,xboxController) != 0)
    {
      ourDrone.AddRelativeForce(Vector3.forward * XCI.GetAxis(XboxAxis.RightStickY,xboxController) * movementForwardSpeed);
      tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 10 * XCI.GetAxis(XboxAxis.RightStickY,xboxController), ref tiltVelocityForward, 0.1f);
    }
  }

  public float sideMovementAmount = 150.0f;
  private float tiltAmountSideways;
  private float tiltVelocitySideways;
  void Swerer()
  {
    if (XCI.GetAxis(XboxAxis.RightStickX,xboxController) != 0)
    {
      ourDrone.AddRelativeForce(Vector3.right * XCI.GetAxis(XboxAxis.RightStickX,xboxController)  * sideMovementAmount);
      tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -15 * XCI.GetAxis(XboxAxis.RightStickX,xboxController), ref tiltVelocitySideways, 0.1f);
    }
    else
    {
      tiltVelocitySideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltVelocitySideways, 0.1f);
    }
    tiltVelocitySideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltVelocitySideways, 0.1f);
  }

  private Vector3 velocityToSmoothDampToZero;
  void ClampingSpeedValues()
  {
    if (XCI.GetAxis(XboxAxis.RightStickY,xboxController) != 0 && XCI.GetAxis(XboxAxis.RightStickX,xboxController) != 0)
    {
      ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }

    if (XCI.GetAxis(XboxAxis.RightStickY,xboxController) != 0 && XCI.GetAxis(XboxAxis.RightStickX,xboxController) == 0)
    {
      ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }

    if (XCI.GetAxis(XboxAxis.RightStickY,xboxController) == 0 && XCI.GetAxis(XboxAxis.RightStickX,xboxController) != 0)
    {
      ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }

    if (XCI.GetAxis(XboxAxis.RightStickY,xboxController) == 0 && XCI.GetAxis(XboxAxis.RightStickX,xboxController) == 0)
    {
      ourDrone.velocity = Vector3.SmoothDamp(ourDrone.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.59f);
    }
  }

  //solo esta para pruebas
  //void CameraFollow()
  //{
  //  Vector3 moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
  //  float bias = 0.96f;
  //  Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
  //  Camera.main.transform.LookAt(transform.position + transform.forward * 30.0f);
  //}

  public float up = 2.5f;
  public float foward = 11.0f;
  public float x = 19.0f;
  public float bias = 0.25f;
  public bool fpvcam = false;

  void CameraSwitch(){
    if(XCI.GetButtonDown(XboxButton.Start, xboxController)){
      if(fpvcam){
        fpvcam = false;
      }else{
        fpvcam = true;
      }
    }

    if(fpvcam){
      Camera.main.transform.localPosition = new Vector3(0.0f,0.5f,0.4f);
      Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(1.6f, 0, 0));
      }else{
        Vector3 moveCamTo = ourDrone.transform.position - ourDrone.transform.forward * foward + Vector3.up * up;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
        Camera.main.transform.LookAt(ourDrone.transform.position + ourDrone.transform.forward * x);
      }
  }

  //void Hover(){
  //}

  private AudioSource droneSound;
  void DroneSound(){
  	droneSound.pitch = 1 + (ourDrone.velocity.magnitude / 100);
  }

}







































