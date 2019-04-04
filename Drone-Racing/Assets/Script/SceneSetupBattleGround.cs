using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class SceneSetupBattleGround : MonoBehaviour
{

  List<GameObject> players;
  public int numPlayers;
  //public GameMaster gameMaster;
  public Sprite[] sprites;
  public Sprite[] itemSprites;

  public float x, y, z;
  private Scene currentScene;
  // Use this for initialization
  void Start()
  {
    numPlayers = XCI.GetNumPluggedCtrlrs();

    SetupItemImgArr();
    //Debug.Log(numPlayers);
    //gameMaster = (GameMaster)gameObject.GetComponent<GameMaster>();
    createPlayers();
		createCameras();
		setupControllers();

    currentScene = SceneManager.GetActiveScene();
    //Debug.Log("Active scene is '" + currentScene.name + "'.");

    FindObjectOfType<AudioManager>().Play("BattleGround");
    
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

  private void createPlayers()
  {
    players = new List<GameObject>();
    //Vector3 basis = new Vector3(145, 15, -65);
    for (int i = 0; i < numPlayers; i++)
    {
      x = Random.Range(-180.0f, 10.0f);
      y = Random.Range(60.0f, 260.0f);
      z = Random.Range(-150.0f, 55.0f);
      GameObject tmpPlayer = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerBattle"), new Vector3(x, y, z), Quaternion.identity);
      players.Add(tmpPlayer);
      BattleGame tmpDrone = (BattleGame)tmpPlayer.GetComponentInChildren<BattleGame>();
      tmpDrone.itemSprites = itemSprites; 

      //BattleMaster.drones.Add(tmpDrone);
    }
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

}
