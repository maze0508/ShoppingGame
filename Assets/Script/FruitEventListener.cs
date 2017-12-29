using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//detect the hit on 2D sprite
using UnityEngine.SceneManagement;


public class FruitEventListener : MonoBehaviour {
	Button btn_list,btn_basket;
	Text t_Timer;
	int totalTime,limitTime;
	public bool stop;

	void Start(){
		limitTime = 60;//限定的時間
		totalTime =0;


		t_Timer = GetComponentsInChildren<Text>()[0];
		btn_list = GetComponentsInChildren<Button> () [0];
		btn_basket = GetComponentsInChildren<Button> () [1];
		InvokeRepeating ("timer",1f,1f);

		t_Timer.text = limitTime+"";
		btn_list.onClick.AddListener (showList);
		btn_basket.onClick.AddListener (showBasket);
	}

	void timer(){
		if (GlobalVariables.flag) {
			if (totalTime != limitTime) {
				totalTime += 1;
				t_Timer.text = (limitTime - totalTime) + "";//顯示倒數秒數

			} else {
				//時間到就直接切換結帳場景
				//Debug.Log ("Go to scene3");
				SceneManager.LoadScene("checkout");
			}
		}
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit2D hitFruit = Physics2D.Raycast(ray.origin,ray.direction,10,256);
		RaycastHit2D hitCheckout = Physics2D.Raycast(ray.origin,ray.direction,10,512);
		if(GlobalVariables.flag){
			if (Input.GetMouseButtonDown (0)) {
				if (hitFruit.collider) {//點擊水果
					GlobalVariables.flag = false;
					UIFruitInfo info = gameObject.AddComponent<UIFruitInfo> ();
					info.setFruitContent (hitFruit.transform.name);//set the target fruit's name
				}
				if (hitCheckout.collider) {//點擊
					Debug.Log("Go to scene3");
					SceneManager.LoadScene ("checkout");
				}
			}
		}
	}
		
	void showList(){
		GlobalVariables.flag = false;
		UIManager.Instance.ShowPanel ("ShoppingList");
	}
	void showBasket(){
		GlobalVariables.flag = false;
		UIManager.Instance.ShowPanel ("Basket");
	}
}
