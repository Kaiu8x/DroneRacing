using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {


  	private List<GameObject> playerScoreboards;
  	GameObject[] tempPlayerScoreboard;
  	//private int counter;


	// Use this for initialization
	void Start () {
		playerScoreboards = new List<GameObject>();
		//tempPlayerScoreboard = GameObject.FindGameObjectsWithTag("ScoreBoard");
		//counter = 0;

		showResults();
		FindObjectOfType<AudioManager>().Play("Scores");
	}


	void showResults(){
		for( int i = 0; i < 4 ; i++){
			if(i < ScoreHolder.numPlayers) {
				Text nameText = GameObject.Find("NameText"+i.ToString()).GetComponent<Text>();
				nameText.text = "Player"+(i+1).ToString();
				Text scoreText = GameObject.Find("ScoreText"+i.ToString()).GetComponent<Text>();
				scoreText.text = ScoreHolder.scores[i].ToString();
				Text timeText = GameObject.Find("TimeText"+i.ToString()).GetComponent<Text>();
				timeText.text = ScoreHolder.totalLapTime[i].ToString();
				Text finalScoreText = GameObject.Find("FinalScoreText"+i.ToString()).GetComponent<Text>();
				finalScoreText.text = ScoreHolder.totalScore[i].ToString();
				Sprite placeSprite = GameObject.Find("PlaceSprite"+i.ToString()).GetComponent<Image>().sprite;
				Debug.Log(ScoreHolder.positions[i]);
				switch(ScoreHolder.positions[i]+1){
					case 0:
						Debug.Log("Case 0: " + placeSprite);
						break;
					case 1:
						placeSprite  = Resources.Load<Sprite>("HudAssets/Places/1st") as Sprite;
						Debug.Log("Case 1:" + placeSprite);
						break;
					case 2:
						placeSprite  = Resources.Load<Sprite>("HudAssets/Places/2nd") as Sprite;
						Debug.Log(placeSprite);
						break;
					case 3:
						placeSprite  = Resources.Load<Sprite>("HudAssets/Places/3rd") as Sprite;
						Debug.Log(placeSprite);
						break;
					case 4:
						placeSprite  = Resources.Load<Sprite>("HudAssets/Places/4th") as Sprite;
						Debug.Log(placeSprite);
						break;
					default:
						//placeSprite  = Resources.Load<Sprite>("HudAssets/Places/1st") as Sprite;
						Debug.Log("Default case: "+ placeSprite);
						break;
				}
				//tempPlayerScoreboard[i].SetActive(true);
			}else {
				GameObject.Find("Player"+(i+1).ToString()).SetActive(false);
			}
			//tempPlayerScoreboard.transform.parent = this.transform.parent;
		}

	}

	public void PlayNextTrack() {
		if(ScoreHolder.isTournament) {
				Debug.Log(ScoreHolder.i);
				SceneManager.LoadScene(ScoreHolder.nextTrack());
		}else {
			SceneManager.LoadScene("Menu");
		}
		FindObjectOfType<AudioManager>().Stop("Scores");
	}

}





































