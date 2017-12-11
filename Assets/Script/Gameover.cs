using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameover : MonoBehaviour {
	Text score;

	void Start () {
		score = GetComponentsInChildren<Text> () [1];
		score.text = ""+GlobalVariables.correctCount;
	}

}
