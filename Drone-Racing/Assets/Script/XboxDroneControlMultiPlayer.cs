using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class XboxDroneControlMultiPlayer : MonoBehaviour
{

  public XboxController controller;
  private static bool didQueryNumOfCtrlrs = false;


  Rigidbody ourDrone;
  float gravity = 9.81f;
  //public float positionX;
  //public float positionY;
  //public float positionZ;

  void Awake()
  {
    //ourDrone = GetComponent<Rigidbody>();
    //droneSound = gameObject.transform.Find("DroneSound").GetComponent<AudioSource>();
    switch(controller)
    {
      case XboxController.First: ourDrone = GetComponent<Rigidbody>(); break;
      case XboxController.Second: ourDrone = GetComponent<Rigidbody>(); break;
      case XboxController.Third: ourDrone = GetComponent<Rigidbody>(); break;
      case XboxController.Fourth: ourDrone = GetComponent<Rigidbody>(); break;
    }

    if(!didQueryNumOfCtrlrs)
    {
        didQueryNumOfCtrlrs = true;
        
        int queriedNumberOfCtrlrs = XCI.GetNumPluggedCtrlrs();
        
        if(queriedNumberOfCtrlrs == 1)
        {
          Debug.Log("Only " + queriedNumberOfCtrlrs + " Xbox controller plugged in.");
        }
        else if (queriedNumberOfCtrlrs == 0)
        {
          Debug.Log("No Xbox controllers plugged in!");
        }
        else
        {
          Debug.Log(queriedNumberOfCtrlrs + " Xbox controllers plugged in.");
        }
        
        XCI.DEBUG_LogControllerNames();
          // This code only works on Windows
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("Windows Only:: Any Controller Plugged in: " + XCI.IsPluggedIn(XboxController.Any).ToString());
            Debug.Log("Windows Only:: Controller 1 Plugged in: " + XCI.IsPluggedIn(XboxController.First).ToString());
            Debug.Log("Windows Only:: Controller 2 Plugged in: " + XCI.IsPluggedIn(XboxController.Second).ToString());
            Debug.Log("Windows Only:: Controller 3 Plugged in: " + XCI.IsPluggedIn(XboxController.Third).ToString());
            Debug.Log("Windows Only:: Controller 4 Plugged in: " + XCI.IsPluggedIn(XboxController.Fourth).ToString());
        }
    }

  }

  void FixedUpdate()
  {
    MovementUpDown();
    Rotation();
    MovementForward();
    Swerer();

    ClampingSpeedValues();
    //CameraFollow();
    //DroneSound();


    //if(XCI.GetAxis(XboxAxis.LeftStickY)==0 && XCI.GetAxis(XboxAxis.LeftStickX)==0 && XCI.GetAxis(XboxAxis.RightStickY)==0 && XCI.GetAxis(XboxAxis.RightStickX)==0)
    //Hover();

    ourDrone.AddRelativeForce(Vector3.up * upForce);
    ourDrone.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways));
    //Debug.Log(XCI.GetAxis(XboxAxis.LeftStickY));

  }

  public float upForce;
  public float addedSpeedUp = 120.0f; //change to private
  public float addedSpeedDown = 70.0f; //change to private
  void MovementUpDown()
  {

    if (XCI.GetAxis(XboxAxis.LeftStickX, controller) != 0 || XCI.GetAxis(XboxAxis.LeftStickY, controller) != 0)
    {
      if (XCI.GetAxis(XboxAxis.RightStickY, controller) == 0 && XCI.GetAxis(XboxAxis.RightStickX, controller) == 0)
      {
        ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
        //ourDrone.velocity = new Vector3(Mathf.Lerp(ourDrone.velocity.x, 0, Time.deltaTime * 5), Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);

        //upForce = 281.0f;
        //Debug.Log("l.45");
      }

      if (XCI.GetAxis(XboxAxis.RightStickX, controller) != 0)
      {
        //upForce = 410.0f;
      }
    }

    if (XCI.GetAxis(XboxAxis.RightStickY, controller) > 0)
    {
      upForce = XCI.GetAxis(XboxAxis.RightStickY, controller) * addedSpeedUp;
    }
    else if (XCI.GetAxis(XboxAxis.RightStickY, controller) < 0)
    {
      upForce = XCI.GetAxis(XboxAxis.RightStickY, controller) * addedSpeedDown;
    }
    else if (XCI.GetAxis(XboxAxis.RightStickY, controller) == 0)
    {
      upForce = ourDrone.mass * gravity + Random.Range(-3.0f, 3.0f);
    }
  }

  private float currentYRotation;
  private float wantedYRotation;
  private float rotationYVelocity;
  public float rotateAmountByKey = 0.5f;
  void Rotation()
  {
    if (XCI.GetAxis(XboxAxis.RightStickX, controller) * -1 > 0)
    {
      wantedYRotation -= rotateAmountByKey;
    }
    if (XCI.GetAxis(XboxAxis.RightStickX, controller) * -1 < 0)
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
    if (XCI.GetAxis(XboxAxis.LeftStickY, controller) != 0)
    {
      ourDrone.AddRelativeForce(Vector3.forward * XCI.GetAxis(XboxAxis.LeftStickY, controller) * movementForwardSpeed);
      tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 10 * XCI.GetAxis(XboxAxis.LeftStickY, controller), ref tiltVelocityForward, 0.1f);
    }
  }

  public float sideMovementAmount = 15.0f;
  private float tiltAmountSideways;
  private float tiltVelocitySideways;
  void Swerer()
  {
    if (XCI.GetAxis(XboxAxis.LeftStickX, controller) != 0)
    {
      ourDrone.AddRelativeForce(Vector3.right * XCI.GetAxis(XboxAxis.LeftStickX, controller)  * sideMovementAmount);
      tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -15 * XCI.GetAxis(XboxAxis.LeftStickX, controller), ref tiltVelocitySideways, 0.1f);
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
    if (XCI.GetAxis(XboxAxis.LeftStickY, controller) != 0 && XCI.GetAxis(XboxAxis.LeftStickX, controller) != 0)
    {
      ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }

    if (XCI.GetAxis(XboxAxis.LeftStickY, controller) != 0 && XCI.GetAxis(XboxAxis.LeftStickX, controller) == 0)
    {
      ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }

    if (XCI.GetAxis(XboxAxis.LeftStickY, controller) == 0 && XCI.GetAxis(XboxAxis.LeftStickX, controller) != 0)
    {
      ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }

    if (XCI.GetAxis(XboxAxis.LeftStickY, controller) == 0 && XCI.GetAxis(XboxAxis.LeftStickX, controller) == 0)
    {
      ourDrone.velocity = Vector3.SmoothDamp(ourDrone.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.59f);
    }
  }

  //solo esta para pruebas
  void CameraFollow()
  {
    Vector3 moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
    float bias = 0.96f;
    Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
    Camera.main.transform.LookAt(transform.position + transform.forward * 30.0f);
  }

  //void Hover(){
  //}

  //private AudioSource droneSound;
  //void DroneSound(){
  //	droneSound.pitch = 1 + (ourDrone.velocity.magnitude / 100);
  //}

}







































