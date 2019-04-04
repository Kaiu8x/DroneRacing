using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

//FindObjectOfType<AudioManager>().Play("name of song");

public class AudioManager : MonoBehaviour {
	

	public Sound[] sounds;

	public static AudioManager instance;
	private Scene currentScene;

	void Awake () {

		if(instance == null){
			instance = this;
		}else{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;

		}
	}

	void Star() {
		currentScene = SceneManager.GetActiveScene();
	}

	public void Play (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			Debug.Log("Sound: "+ name + " not found");
			return;
		}
		s.source.Play();
	}

	public void Pause (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			Debug.Log("Sound: "+ name + " not found");
			return;
		}
		s.source.Pause();
	}

	public void Stop (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			Debug.Log("Sound: "+ name + " not found");
			return;
		}
		s.source.Stop();
	}

	public AudioSource getAudio(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			Debug.Log("Sound: "+ name + " not found");
			return null;
		}
		return s.source;
	}




}


































