using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour {
	

	void Star() {
		FindObjectOfType<AudioManager>().Play("Menu");
	}

	public void PlayTrack1(){
		ScoreHolder.setIsTournament(false);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);

	}
	public void PlayTrack2(){
		ScoreHolder.setIsTournament(false);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 2);

	}

	public void PlayTrack3(){
		ScoreHolder.setIsTournament(false);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 3);

	}

	public void PlayTrack4(){
		ScoreHolder.setIsTournament(false);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 4);

	}

	string[] tracks;
	int ranInt;
	public void Tournament(){
		tracks = new string[4];
		for(int i = 0; i < 3;i++) {
			ranInt = Random.Range(0,4);
			Debug.Log(ranInt);
			switch (ranInt) {
				case 0:
					tracks[i] = "Track_1";
					break;
				case 1:
					tracks[i] = "Track_2";
					break;
				case 2:
					tracks[i] = "Track_3";
					break;
				case 3:
					tracks[i] = "Track_4";
					break;
				default:
					tracks[i] = "Track_4";
					break;
			}
		}
		tracks[3] = "Menu";
		ScoreHolder.setIsTournament(true);
		ScoreHolder.setTracks(tracks);
		ScoreHolder.setTrackNumber(0);
		SceneManager.LoadScene (ScoreHolder.nextTrack());
	}

	public void Battle(){
		ScoreHolder.setIsTournament(false);
		SceneManager.LoadScene ("BattleGround");
	}
		


}
