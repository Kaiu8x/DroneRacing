using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour {

	private float startTime;
	public  TextMeshProUGUI timerText;

	void Start() {

		startTime = Time.time;
		timerText = GetComponent<TextMeshProUGUI>();

	}

	void Update() {
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
}