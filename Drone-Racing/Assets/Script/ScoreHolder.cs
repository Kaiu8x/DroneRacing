using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreHolder {
	public static int[] totalScore = new int[4];
	public static float[] totalLapTime = new float[4];

	public static int[] scores = new int[4];
	public  static float[] lapTime = new float[4];
	public static int[] positions = new int[4];
	public static int numPlayers;

	public static bool isTournament = false;
	public static string[] tracks = new string[4];
	public static int i;

	public static void setTracks(string[] _tracks) {
		tracks = _tracks;
	}

	public static void setIsTournament(bool _isTournament) {
		isTournament = _isTournament;
	}

	public static void setTrackNumber(int _i) {
		i = _i;
	}

	public static string nextTrack(){
		i++;
		return tracks[i-1];
	}

	public static void setScores(int[] _scores) {
		scores = _scores;
		for (int i = 0; i<numPlayers;i++) {
			totalScore [i] += scores [i];
		}
	}

	public static void setPositions(int[] _positions) {
		positions = _positions;
	}

	public static void setLapTime(float[] _lapTime) {
		lapTime = _lapTime;
		for (int i = 0; i<numPlayers;i++) {
			totalLapTime[i] += lapTime[i];
		}
	}
}
