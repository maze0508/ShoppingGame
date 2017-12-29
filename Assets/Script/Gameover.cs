﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameover : MonoBehaviour {
	Text score;
	GameObject list;
	string[] shoppingList;
	void Start () {
		shoppingList = GlobalVariables.shoppingList;
		list = Resources.Load ("Object/Basket_Fruit", typeof(GameObject))as GameObject;

		score = GetComponentsInChildren<Text> () [1];
		score.text = ""+GlobalVariables.correctCount;
		showList ();
	}
	void showList(){
		GameObject g_list;
		for(int i =0;i<shoppingList.Length;i++){
			g_list = Instantiate (list);
			g_list.transform.SetParent (gameObject.transform);
			g_list.name = shoppingList [i];
			if (i / 5 < 1) {
				g_list.transform.localPosition = new Vector3 (-80.0f, 180.0f - (110.0f * i), 0.0f);
			} else {
				g_list.transform.localPosition = new Vector3 (280.0f, 180.0f - (110.0f * (i%5)), 0.0f);
			}
			g_list.transform.localScale = Vector3.one;
			g_list.GetComponentsInChildren<Image>()[1].sprite =  Resources.Load ("ImgFruit/" + shoppingList [i], typeof(Sprite))as Sprite;
			g_list.GetComponentInChildren<Text>().text = shoppingList[i];
		}
	}

}
