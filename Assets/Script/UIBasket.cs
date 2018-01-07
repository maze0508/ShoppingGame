using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBasket : MonoBehaviour {
	GameObject BasketFruit;
	string []basket;
	int pointer;
	Button btn_Back;

	void Start () {
		basket = GlobalVariables.basket;
		pointer = GlobalVariables.currentCount;//當前存放數量
		BasketFruit = Resources.Load("Object/Basket_Fruit",typeof(GameObject))as GameObject;
		btn_Back = GetComponentsInChildren<Button> () [0];
		btn_Back.onClick.AddListener (backMain);
		createObj ();
	}



	public void createObj(){//根據basket陣列，生成此水果的圖文
		for (int i = 0; i < pointer; i++) {
			GameObject fruitObj = Instantiate (BasketFruit);
			fruitObj.transform.SetParent (gameObject.transform);

			if (i % 3 == 0) {
				fruitObj.transform.localPosition = new Vector3 (-250.0f, (i / 3)*(-100.0f),0.0f);
			} else if(i % 3 == 1) {
				fruitObj.transform.localPosition = new Vector3 (0.0f, (i / 3)*(-100.0f),0.0f);
			}else{
				fruitObj.transform.localPosition = new Vector3 (250.0f, (i / 3)*(-100.0f),0.0f);
			}
			fruitObj.transform.localScale = new Vector3 (0.6f,0.6f,0.6f);
			fruitObj.GetComponentsInChildren<Image> () [1].sprite = Resources.Load ("ImgFruit/" + basket[i], typeof(Sprite))as Sprite;
			fruitObj.GetComponentsInChildren<Text> () [0].text = basket[i];
			//Debug.Log (fruitObj.GetComponentsInChildren<Image> () [1].sprite );

		}
	}

	void backMain(){
		if (UIManager.Instance.IsUILive ("Basket")) {
			UIManager.Instance.ClosePanel ("Basket");
			GlobalVariables.flag = true;
		}
	}

}
