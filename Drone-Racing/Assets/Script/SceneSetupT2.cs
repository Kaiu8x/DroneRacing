using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class SceneSetupT2 : MonoBehaviour
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
  public GameObject checkpoint13;
  public GameObject checkpoint14;
  public GameObject checkpoint15;
  public GameObject checkpoint16;
  public GameObject checkpoint17;
  public GameObject checkpoint18;
  public GameObject[] checkpoints;

  List<GameObject> players;
  public int numPlayers;
  public int offset;
  public GameMaster gameMaster;
  public Sprite[] sprites;
  public Sprite[] itemSprites;

  public float x, y, z;
  private Scene currentScene;
  // Use this for initialization
  void Start()
  {
    x = 343.0f;
    y = 60.0f;
    z = 177.0f;
    numPlayers = XCI.GetNumPluggedCtrlrs();
		ScoreHolder.numPlayers = numPlayers;
    SetupGateArr();
    SetupGateList();
    SetupPositionImgArr();
    SetupItemImgArr();
    //Debug.Log(numPlayers);
    gameMaster = (GameMaster)gameObject.GetComponent<GameMaster>();
    createPlayers();
		createCameras();
		setupControllers();
    SetupTextures();
    offset = 5;

    currentScene = SceneManager.GetActiveScene();
    //Debug.Log("Active scene is '" + currentScene.name + "'.");

    FindObjectOfType<AudioManager>().Play("Track2");
    
  }

  private void setupControllers()
  {
    for (int i = 0; i < players.Count; i++) {
      players[i].GetComponentInChildren<XboxDroneControl2>().xboxController = (XboxController)(i+1);
      //Debug.Log((XboxController)(i+1));
    }
  }

  //splitsScreens
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
        Camera tmpCam = findCamera(player.GetComponentsInChildren<Camera>(), "Drone Cam");
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

  private Camera findCamera(Camera[] cameras, string name)
  {
    foreach (Camera camera in cameras)
    {
      if (camera.name == name)
      {
        return camera;
      }
    }
    return null;
  }

  private Image findImage(Image[] images, string name){
    foreach (Image image in images){
      if (image.name == name) {
        return image;
      }
    }
    return null;
  }

  private void createPlayers()
  {
    players = new List<GameObject>();
    //Vector3 basis = new Vector3(145, 15, -65);
    for (int i = 0; i < numPlayers; i++)
    {
      GameObject tmpPlayer = (GameObject)Instantiate(Resources.Load("Prefabs/Player"), new Vector3(x + i * offset, y, z), Quaternion.identity);
      players.Add(tmpPlayer);
      DroneGame tmpDrone = (DroneGame)tmpPlayer.GetComponentInChildren<DroneGame>();
      tmpDrone.positionSprites = sprites;
      tmpDrone.itemSprites = itemSprites; 
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
      //Debug.Log("Checking: "+visitedCheckPointDict[(object)checkpoint.GetInstanceID()]==null);
    }
    return visitedCheckPointDict;
  }

  void SetupPositionImgArr()
  {
    sprites = new Sprite[4];
    //Debug.Log(Resources.Load<Sprite>("HudAssets/Places/1st") ==null);
    sprites[0] = Resources.Load<Sprite>("HudAssets/Places/1st") as Sprite;
    sprites[1] = Resources.Load<Sprite>("HudAssets/Places/2nd") as Sprite;
    sprites[2] = Resources.Load<Sprite>("HudAssets/Places/3rd") as Sprite;
    sprites[3] = Resources.Load<Sprite>("HudAssets/Places/4th") as Sprite;
  }

  void SetupItemImgArr()
  {
    itemSprites = new Sprite[7];
    //Debug.Log(Resources.Load<Sprite>("HudAssets/Items/NoItem") == null);
    itemSprites[0] = Resources.Load<Sprite>("HudAssets/Items/NoItem") as Sprite;
    itemSprites[1] = Resources.Load<Sprite>("HudAssets/Items/Boost") as Sprite;
    itemSprites[2] = Resources.Load<Sprite>("HudAssets/Items/Invert") as Sprite;
    itemSprites[3] = Resources.Load<Sprite>("HudAssets/Items/Shock") as Sprite;
    itemSprites[4] = Resources.Load<Sprite>("HudAssets/Items/Bigger") as Sprite;
    itemSprites[5] = Resources.Load<Sprite>("HudAssets/Items/Interference") as Sprite;
    itemSprites[6] = Resources.Load<Sprite>("HudAssets/Items/Shield") as Sprite;
  }

  void SetupTextures(){
    foreach (GameObject player in players) {
    RenderTexture textureMap = new RenderTexture(256,256,24);
    Camera miniMapCam = findCamera(player.GetComponentsInChildren<Camera>(), "MiniMap Cam");
			//Debug.Log("tmpCam " +  miniMapCam == null);
			//Debug.Log("texture " + textureMap == null);
			miniMapCam.targetTexture = textureMap;
    Material material = new Material(Shader.Find("Unlit/Texture"));
    //Debug.Log("tmpCam " +  camera == null);
    //Debug.Log("texture " + texture == null);
    //Debug.Log("material " +  material == null);
			material.mainTexture = textureMap;
    Image image = findImage(player.GetComponentsInChildren<Image>(),"MiniMap");
    //Debug.Log("image "  +  image == null);
    image.material = material;
  }
    }
}
