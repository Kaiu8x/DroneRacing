using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using XboxCtrlrInput;

public class SceneSetup : MonoBehaviour
{
  public GameObject checkpoint1;
  public GameObject checkpoint2;
  public GameObject checkpoint3;
  public GameObject checkpoint4;
  public GameObject checkpoint5;
  public GameObject checkpoint6;
  public GameObject checkpoint7;
  public GameObject checkpoint8;
  public GameObject checkpoint9;
  public GameObject checkpoint10;
  public GameObject checkpoint11;
  public GameObject checkpoint12;
  public GameObject[] checkpoints;
  //public OrderedDictionary visitedCheckPointDict;
  //public Dictionary<int, GameObject> checkPointObjectDict;


  List<GameObject> players;
  public int numPlayers;
  public int offset;
  public GameMaster gameMaster;
  public Sprite[] sprites;
  // Use this for initialization
  void Start()
  {
    numPlayers = XCI.GetNumPluggedCtrlrs();
    SetupGateArr();
    SetupGateList();
    SetupPositionImgArr();
    Debug.Log(numPlayers);
    gameMaster = (GameMaster)gameObject.GetComponent<GameMaster>();
    createPlayers();
    createCameras();
    setupControllers();
  }
  private void setupControllers()
  {
    for (int i = 0; i < players.Count; i++)
    {
      players[i].GetComponentInChildren<XboxDroneControl2>().xboxController = (XboxController)(i + 1);
      Debug.Log((XboxController)(i + 1));
    }
  }
  private void createCameras()
  {
    int xCount = 0;
    int yCount = 0;
    float xOffset;
    float yOffset;
    Rect tmpRect = new Rect(0, 0, 0, 0); ;
    if (numPlayers > 1)
    {
      foreach (GameObject player in players)
      {
        Camera tmpCam = findCamera(player.GetComponentsInChildren<Camera>());
        if (numPlayers == 2)
        {
          yOffset = yCount % 2 == 0 ? -0.5f : 0.5f;
          xOffset = 0;
          yCount++;
        }
        else
        {
          yOffset = yCount % 2 == 0 ? -0.5f : 0.5f;
          xOffset = xCount < 2 ? -0.5f : 0.5f;
          yCount++;
          xCount++;
        }
        tmpCam.rect = new Rect(xOffset, yOffset, 1, 1);
      }
    }
  }

  private Camera findCamera(Camera[] cameras)
  {
    foreach (Camera camera in cameras)
    {
      if (camera.name == "Drone Cam")
      {
        return camera;
      }
    }
    return null;
  }

  // Update is called once per frame
  private void createPlayers()
  {
    players = new List<GameObject>();
    //Vector3 basis = new Vector3(145, 15, -65);
    for (int i = 0; i < numPlayers; i++)
    {
      GameObject tmpPlayer = (GameObject)Instantiate(Resources.Load("Prefabs/Player"), new Vector3(145 + i * offset, 15, -65), Quaternion.identity);
      players.Add(tmpPlayer);
      DroneGame tmpDrone = (DroneGame)tmpPlayer.GetComponentInChildren<DroneGame>();
      tmpDrone.positionSprites = sprites; 
      tmpDrone.updatePosition(i+1);
      tmpDrone.checkpointArr = checkpoints;
      tmpDrone.visitedCheckpointDict = SetupGateList();
      //tmpDrone.checkPointObjectDict = checkPointObjectDict;
      gameMaster.drones.Add(tmpDrone);
    }
  }
  void SetupGateArr()
  {
    checkpoints = new GameObject[] {
      checkpoint1,checkpoint2,checkpoint3,
      checkpoint4,checkpoint5,checkpoint6,
      checkpoint7,checkpoint8,checkpoint9,
      checkpoint10,checkpoint11,checkpoint12
      };
  }
  public  OrderedDictionary SetupGateList()
  {
    OrderedDictionary visitedCheckPointDict= new OrderedDictionary();
    foreach (GameObject checkpoint in checkpoints)
    {
      visitedCheckPointDict.Add(checkpoint.GetInstanceID(), false);
      Debug.Log("Checking: "+visitedCheckPointDict[(object)checkpoint.GetInstanceID()]==null);
    }
    return visitedCheckPointDict;
  }
  void SetupPositionImgArr(){
    sprites = new Sprite[4];
    Debug.Log(Resources.Load<Sprite>("HudAssets/Places/1st") ==null);
    sprites[0] = Resources.Load<Sprite>("HudAssets/Places/1st") as Sprite;
    sprites[1] = Resources.Load<Sprite>("HudAssets/Places/2nd") as Sprite;
    sprites[2] = Resources.Load<Sprite>("HudAssets/Places/3rd") as Sprite;
    sprites[3] = Resources.Load<Sprite>("HudAssets/Places/4th") as Sprite;
  }
}
