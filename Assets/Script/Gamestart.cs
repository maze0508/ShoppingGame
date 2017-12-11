using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamestart : MonoBehaviour {
	Button btn_Start;
	// Use this for initialization
	void Start () {
		UIManager.Instance.ShowPanel("GameMenu");
		btn_Start = GetComponentsInChildren<Button> () [0];
		btn_Start.onClick.AddListener (showAnimation);
	}

	void showAnimation(){
		if (UIManager.Instance.IsUILive ("GameMenu")) {
			UIManager.Instance.ClosePanel ("GameMenu");
		}
		UIManager.Instance.ShowPanel("PreAnimation");

	}
}
