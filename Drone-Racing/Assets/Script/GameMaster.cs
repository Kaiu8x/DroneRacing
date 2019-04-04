using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

  	public List<DroneGame> drones;
  	static public GameMaster instance;
  	public int numOfPlayers;

  	//public float[] totalLapTime;
  	//public int[] totalScore;
  	public float[] lapTime;
  	public int[] positions;
  	public int[] scores;
  	public int c;
  	public bool flag;

	void Start() {
		InvokeRepeating("calculatePositions",0,.3f);
		//totalLapTime = new float[4];
		//totalScore = new int[4];
		lapTime = new float[4];//[lapnumber][player]
		positions = new int[4];
		scores = new int[4];
		c = 0;
		flag = false;
		//numOfPlayers = drones.Count();
		
		//Debug.Log("num of players "+numOfPlayers);

	}

	void FixedUpdate() {
		if (allFinished()){
				SaveScoreboard();
			}
  	} 

	void calculatePositions() {
		if(drones == null) {
			return;
		}else{
			//Debug.Log("Calculating poistions");
			foreach(DroneGame drone in drones) {
				drone.updatePositionValues();
				drone.printPositionValues();
			}
			 drones = drones.OrderByDescending(x => x.lap)
			 .ThenByDescending(x => x.nextCheckpointIndex)
			 .ThenBy(x => x.distanceNextCheckpoint)
			 .ToList();
			 for (int i = 0; i < drones.Count; i++) {
			 	drones[i].updatePosition(i);
			}
		}
		
		//Debug.Log("Calculating poistions");
	}

	bool allFinished() {
		if(drones == null) {
			Debug.Log (drones.Count);
			return false;
			}else{
				//Debug.Log("Enter finished");
				foreach(DroneGame drone in drones) {
					//Debug.Log("drone.finished "+ drone.finished);
				if (!drone.finished()) return false;
					//Debug.Log("drone.finished "+ drone.finished);
				}
			return true;
			}
	}

	void SaveScoreboard() {
		if(drones == null) {
			Debug.Log (drones.Count);
			return;
		}else{
			Debug.Log("Enter SaveScoreboard");
			foreach(DroneGame drone in drones) {
				//drone.updatePositionValues();
				scores[c] = drone.points;
				positions[c] = drone.position;
				for(int i = 0; i<3; i++) {
					lapTime[i] += drone.lapTime[i];
				}
				c++;
			}
			ScoreHolder.setLapTime(lapTime);
			ScoreHolder.setScores(scores); 
			Debug.Log ("Going to scoreboard");
			FindObjectOfType<AudioManager>().Stop("Track1");
			FindObjectOfType<AudioManager>().Stop("Track2");
			FindObjectOfType<AudioManager>().Stop("Track3");
			FindObjectOfType<AudioManager>().Stop("Track4");
			GotoScoreboard();
		}
	}

	void GotoScoreboard() {
		SceneManager.LoadScene ("Scores");
	}
}
