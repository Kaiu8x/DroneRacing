using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.SceneManagement;

public class DroneGame : MonoBehaviour
{
  private bool start;

  private bool start2;

  public int lap;
  public int points;
  public TextMeshProUGUI scoreText;
  public TextMeshProUGUI lapText;
  public float[] lapTime;
  public int checkpoints;
  private int gates;
  private int totalGates;
  private float startTime;
  private HashSet<int> visited;
  //public Text lapText;
  public int numCheckPoints;

  private float hideSeconds;
  private int itemNumber;
  private Rigidbody ourDrone;
  private bool shock;
  private float factorBigger;
  private bool shieldUp;
  private GameObject obstacle;
  private List<GameObject> obstacles;
  private int i;
  private XboxController xboxController;

  //For  calculating position
  public OrderedDictionary visitedCheckpointDict;
  public Dictionary<int, GameObject> checkPointObjectDict;
  public GameObject nextCheckpoint;
  public GameObject[] checkpointArr;
  public float distanceNextCheckpoint;
  public int nextCheckpointIndex;
  public int position;

  public Sprite[] positionSprites;
  public Sprite[] itemSprites;
  public GameObject positionHolder;
  public GameObject itemHolder;

  public GameObject shield;
  public GameObject interference;
  //private Scene currentScene;

  public  TextMeshProUGUI timerText;


  void Awake()
  {

  }

  void Start()
  {
    lap = 1;
    points = 00000000;
    scoreText.text = points.ToString();
    lapText.text = lap.ToString()+"/3";
    visited = new HashSet<int>();
    lapTime = new float[4];
    itemNumber = 0;
    hideSeconds = 6;
    ourDrone = GetComponent<Rigidbody>();
    shock = false;
    factorBigger = 2.0f;
    shieldUp = false;
    obstacle = new GameObject();
    obstacles = new List<GameObject>();
    i = 0;
    start = false;
    numCheckPoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
    xboxController = GetComponent<XboxDroneControl2>().xboxController;
    if(!start2){
      startRace();
    }

  }


  void FixedUpdate()
  {
    Shock();
    UseItem();
    Shield();

    float t = Time.time - startTime;

    string minutes = ((int)t / 60).ToString ();

    if (t % 60 < 9.9999) {
      string seconds = (t % 60).ToString ("f2");
      timerText.text  = (minutes + ":0" + seconds);
    } else {
      string seconds = (t % 60).ToString ("f2");
      timerText.text = (minutes + ":" + seconds);
    }

  }

  void startLap()
  {
    checkpoints = 0;
    gates = 0;
    //startTime = Time.time;
    start = true;
    visited.Clear();
    resetCheckpointsVisited();

    //currentScene = SceneManager.GetActiveScene();
    

  }

	public bool finished(){
		return lap>3;
	}

  void countLap()
  {
    //Debug.Log("Count Lap");
    if (checkLap())
    {
      lapTime[lap-1] = Time.time - startTime;
      //scoreText.text = points.ToString();
      //Debug.Log("Lap: " + lap + " points: " + gates + " time: " + lapTime[lap]);
      lap++;
      if(lap<4) {
        lapText.text = lap.ToString()+"/3";
      }else{
        lapText.text = "3/3";
      }
      
      totalGates += gates;
      startLap();
    }
    if(lap>3)
    {
      finishTrack();
    }
  }

  bool checkLap()
  {
    return checkpoints == numCheckPoints;
  }

  void Shock()
  {
    if (shock)
    {
      ourDrone.transform.position = new Vector3(ourDrone.transform.position.x + Random.Range(-0.5f, 0.5f), ourDrone.transform.position.y + Random.Range(-0.5f, 0.5f), ourDrone.transform.position.z + Random.Range(-0.5f, 0.5f));
    }
  }

  void Shield()
  {
    shield.SetActive(shieldUp); 
  }

  void OnTriggerEnter(Collider other)
  {
    //Debug.Log("Trigger: " + other.tag + " Instance ID: " + other.GetInstanceID()+ " Visited?: " + visited.Contains(other.GetInstanceID()));
    if (other.tag.Equals("Start_Gate"))
    {
      if (!start)
      {
        startLap();
      }
      else
      {
        countLap();
      }
    }
    if (!visited.Contains(other.GetInstanceID()))
    {
      visited.Add(other.GetInstanceID());
      if (other.tag.Equals("Gate"))
      {
        gates++;
        points += 5 * gates;
        scoreText.text = points.ToString();
        //Debug.Log("Gates: " + gates);
      }
      else if (other.tag.Equals("Checkpoint"))
      {
        checkpoints++;
        visitedCheckpointDict[(object)other.gameObject.GetInstanceID()] = true;
        FindObjectOfType<AudioManager>().Play("checkpoint");
        //Debug.Log("Checkpoints: " + checkpoints + " Time: "+ (Time.time-startTime));
      }
    }

    //Esta parte es del Item
    if (other.tag.Equals("Item"))
    {
      if(itemNumber == 0)
      {
        itemNumber = Random.Range(1,7);
        itemHolder.GetComponent<Image>().sprite = itemSprites[itemNumber];
      }
      other.gameObject.SetActive(false);
      StartCoroutine(TimeItemRespawn(other));
    }

    if (other.tag.Equals("ControllInvert"))
    {
      //other.gameObject.SetActive(false);
      Destroy(other.gameObject);
      if (!shieldUp)
      {
        ourDrone.GetComponent<XboxDroneControl2>().enabled = false;
        ourDrone.GetComponent<XboxDroneControl>().enabled = true;
        StartCoroutine(TimeItemControllInvert());
      }
      else
      {
        shieldUp = false;
      }
    }

    if (other.tag.Equals("Shock"))
    {
      //Place HUD shock mode
      Destroy(other.gameObject);
      if (!shieldUp)
      {
        shock = true;
        FindObjectOfType<AudioManager>().Play("Shock");
        StartCoroutine(TimeItemShock());
      }
      else
      {
        shieldUp = false;
      }
    }


    if (other.tag.Equals("GetBigger"))
    {
      Destroy(other.gameObject);
      if (!shieldUp)
      {
        ourDrone.transform.localScale = new Vector3(1.18f * factorBigger, 1.18f * factorBigger, 1.18f * factorBigger);
        FindObjectOfType<AudioManager>().Play("Bigger");
        if (GetComponent<XboxDroneControl2>().enabled)
        {
          GetComponent<XboxDroneControl2>().up = 8.0f * factorBigger;
          GetComponent<XboxDroneControl2>().foward = 11.0f * factorBigger;
        }
        if (GetComponent<XboxDroneControl>().enabled)
        {
          GetComponent<XboxDroneControl>().up = 8.0f * factorBigger;
          GetComponent<XboxDroneControl>().foward = 11.0f * factorBigger;
        }
        StartCoroutine(TimeItemGetBigger());
      }
      else
      {
        shieldUp = false;
      }
    }

    if (other.tag.Equals("VisualInterference"))
    {
      //Just HUD
      Destroy(other.gameObject);
      if (!shieldUp)
      {
        interference.SetActive(true);
        StartCoroutine(TimeItemVisualInterference());
      }
      else
      {
        shieldUp = false;
      }
    }

    if (other.tag.Equals("Boost"))
      {
        Boost();
      }
  }

  void UseItem()
  {
    if(XCI.GetButton(XboxButton.X, xboxController) && itemNumber != 0)
    {
      //Debug.Log(itemNumber);
      //Debug.Log("ButtonX");
      switch (itemNumber)
      {
        //hacer cambio en HDU
        case 1:
          //Debug.Log("Use Boost");
          Boost();
          break;
        case 2:
          //Debug.Log("Use ControllInvert");
          obstacle = Instantiate(Resources.Load("Prefabs/ControllInvertPrefab")) as GameObject;
          obstacles.Add(obstacle);
          obstacles[i].transform.position = new Vector3(ourDrone.transform.position.x, ourDrone.transform.position.y - 8, ourDrone.transform.position.z - 13);
          i++;
          break;
        case 3:
          //Debug.Log("Use Shock");
          obstacle = Instantiate(Resources.Load("Prefabs/ShockPrefab")) as GameObject;
          obstacles.Add(obstacle);
          obstacles[i].transform.position = new Vector3(ourDrone.transform.position.x, ourDrone.transform.position.y - 8, ourDrone.transform.position.z - 13);
          i++;
          break;
        case 4:
          //Debug.Log("Use GetBigger");
          obstacle = Instantiate(Resources.Load("Prefabs/GetBiggerPrefab")) as GameObject;
          obstacles.Add(obstacle);
          obstacles[i].transform.position = new Vector3(ourDrone.transform.position.x, ourDrone.transform.position.y - 8, ourDrone.transform.position.z - 13);
          i++;
          break;
        case 5:
          //Debug.Log("Use VisualInterference");
          obstacle = Instantiate(Resources.Load("Prefabs/VisualInterferencePrefab")) as GameObject;
          obstacles.Add(obstacle);
          obstacles[i].transform.position = new Vector3(ourDrone.transform.position.x, ourDrone.transform.position.y - 8, ourDrone.transform.position.z - 13);
          i++;
          break;
        case 6:
          //Debug.Log("Use Shield");
          shieldUp = true;
          break;
        default:
          //Debug.Log("Use Nothing");
          break;
      }      
      itemNumber = 0;
      itemHolder.GetComponent<Image>().sprite = itemSprites[itemNumber];

    }

  }

  void Boost()
  {
    if(GetComponent<XboxDroneControl2>().enabled)
    {
      GetComponent<XboxDroneControl2>().movementForwardSpeed = 600.0f;
      GetComponent<XboxDroneControl2>().addedSpeedUp = 210.0f; 
      GetComponent<XboxDroneControl2>().addedSpeedDown = 120.0f;
    }
    if(GetComponent<XboxDroneControl>().enabled)
    {
      GetComponent<XboxDroneControl>().movementForwardSpeed = 600.0f;
      GetComponent<XboxDroneControl>().addedSpeedUp = 210.0f; 
      GetComponent<XboxDroneControl>().addedSpeedDown = 120.0f;
    }
    StartCoroutine(TimeItemBoost());
  }

  void UpdateDistanceNextCheckpoint()
  {
    distanceNextCheckpoint= Vector3.Distance(transform.position,
     nextCheckpoint.transform.position);
  }
  
  //O(N)
  void resetCheckpointsVisited()
  {
    for (int i = 0; i<visitedCheckpointDict.Count;i++)
    {
      visitedCheckpointDict[i] = false;
    }
  }


  public void UpdateNextCheckpointIndex(){
    for (int i = 0; i<visitedCheckpointDict.Count;i++){
      if (!(bool)visitedCheckpointDict[i]){
        nextCheckpointIndex = i;
        break;
      }
    }
  }

  public void UpdateNextCheckpoint()
  {
    nextCheckpoint = checkpointArr[nextCheckpointIndex];
  }


  public void updatePositionValues()
  {
    UpdateNextCheckpointIndex();
    UpdateNextCheckpoint();
    UpdateDistanceNextCheckpoint();
  }


  public void updatePosition(int position)
  {
    //Debug.Log("Old position: " +this.position);
    this.position = position;
    //Debug.Log("New position: " +this.position);
    positionHolder.GetComponent<Image>().sprite = positionSprites[position];
  }

  public void printPositionValues()
  {
    //Debug.Log("Laps: "+lap + " Next index: " + nextCheckpointIndex + " Distance next: " + distanceNextCheckpoint+ " Id next: " + nextCheckpoint.GetInstanceID());
  }

  public void startRace()
  {
    GetComponent<XboxDroneControl2>().isOn = false;

    StartCoroutine(StartRaceTimer1());
  }

  void StartTimer() {
    startTime = Time.time;
  }

  IEnumerator StartRaceTimer1()
  {
    interference.SetActive(true);
    interference.GetComponent<Image>().color = new Color32(155,0,0,243);
    FindObjectOfType<AudioManager>().Play("bep");
    yield return new WaitForSeconds(1);
    interference.SetActive(false);
    StartCoroutine(StartRaceTimer2());
  }

  IEnumerator StartRaceTimer2()
  {
    interference.SetActive(true);
    interference.GetComponent<Image>().color = new Color32(255,0,0,243);
    FindObjectOfType<AudioManager>().Play("bep");
    yield return new WaitForSeconds(1);
    interference.SetActive(false);
    StartCoroutine(StartRaceTimer3());
  }

  IEnumerator StartRaceTimer3()
  {
    interference.SetActive(true);
    interference.GetComponent<Image>().color = new Color32(0,155,0,243);
    FindObjectOfType<AudioManager>().Play("beeep");
    yield return new WaitForSeconds(0.5f);
    interference.SetActive(false);
    interference.GetComponent<Image>().color = new Color32(0,0,0,255);
    GetComponent<XboxDroneControl2>().isOn = true;
    start2 = true;
    StartTimer();
    //Set timer in this moment
  }


  public void finishTrack()
  {
    GetComponent<XboxDroneControl2>().enabled = false;
    GetComponent<XboxDroneControl>().enabled = false;
    interference.SetActive(true);
    positionHolder.transform.localPosition = new Vector3(360,200,0);
    positionHolder.transform.localScale = new Vector3(3,3,1);
    Debug.Log("FinishedTrack");
  }

  IEnumerator TimeItemRespawn(Collider other)
  {
    yield return new WaitForSeconds(hideSeconds);
    other.gameObject.SetActive(true);
  }

  IEnumerator TimeItemControllInvert()
  {
    yield return new WaitForSeconds(5);
    ourDrone.GetComponent<XboxDroneControl2>().enabled = true;
    ourDrone.GetComponent<XboxDroneControl>().enabled = false;
  }

  IEnumerator TimeItemShock()
  {
    yield return new WaitForSeconds(3);
    shock = false;
  }

  IEnumerator TimeItemGetBigger()
  {
    yield return new WaitForSeconds(5);
    ourDrone.transform.localScale = new Vector3(1.18f,1.18f,1.18f);
    if(GetComponent<XboxDroneControl2>().enabled)
    {
      GetComponent<XboxDroneControl2>().up = 8.0f;
      GetComponent<XboxDroneControl2>().foward = 11.0f;
    }
    if(GetComponent<XboxDroneControl>().enabled)
    {
      GetComponent<XboxDroneControl>().up = 8.0f;
      GetComponent<XboxDroneControl>().foward = 11.0f;
    }

  }

  IEnumerator TimeItemVisualInterference()
  {
    yield return new WaitForSeconds(3);
    interference.SetActive(false);
  }

  IEnumerator TimeItemBoost()
  {
    yield return new WaitForSeconds(3);
    if (GetComponent<XboxDroneControl2>().enabled)
    {
      GetComponent<XboxDroneControl2>().movementForwardSpeed = 400.0f;
      GetComponent<XboxDroneControl2>().addedSpeedUp = 190.0f; 
      GetComponent<XboxDroneControl2>().addedSpeedDown = 80.0f;

    }
    if (GetComponent<XboxDroneControl>().enabled)
    {
      GetComponent<XboxDroneControl>().movementForwardSpeed = 400.0f;
      GetComponent<XboxDroneControl>().addedSpeedUp = 190.0f; 
      GetComponent<XboxDroneControl>().addedSpeedDown = 80.0f;

    }
  }

}


