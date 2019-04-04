using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class BattleMaster : MonoBehaviour {

  	public List<BattleGame> drones;
  	static public BattleMaster instance;
  	public int numOfPlayers;

  	//public float[] totalLapTime;
  	//public int[] totalScore;
  	public int c;
  	public bool flag;

	void Start() {
		c = 0;
		flag = false;
		//numOfPlayers = drones.Count();
		
		//Debug.Log("num of players "+numOfPlayers);

	}

	void FixedUpdate() {
		if (allFinished()){
				GoMenu();
			}
  	} 

	bool allFinished() {
		if(drones == null) {
			//Debug.Log (drones.Count);
			return false;
			}else{
				//Debug.Log("Enter finished");
				foreach(BattleGame drone in drones) {
					//Debug.Log("drone.finished "+ drone.finished);
				if (!drone.finished()) return false;
					//Debug.Log("drone.finished "+ drone.finished);
				}
			return true;
			}
	}

	void GoMenu() {
		FindObjectOfType<AudioManager>().Stop("BattleGround");
		SceneManager.LoadScene ("Menu");
	}
}
