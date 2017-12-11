using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour {
	private int difficultLevel,itemNumber ;
	private string[]shoppingList,q_list,checkoutList;
	public Image Img_Easy,Img_Normal,Img_Difficult;
	Button btn_Easy,btn_Normal,btn_Difficult;
	SQLDB db;
	int cloze;

	void Start () {
		
		difficultLevel = GlobalVariables.difficultLevel;
		itemNumber = GlobalVariables.itemNumber;
		shoppingList = GlobalVariables.shoppingList;
		q_list = GlobalVariables.q_list;
		checkoutList = GlobalVariables.checkoutList;

		db = gameObject.AddComponent<SQLDB> ();
		cloze = 0;//挖空多少字母

		btn_Easy = GetComponentsInChildren<Button>()[0];
		btn_Normal = GetComponentsInChildren<Button>()[1];
		btn_Difficult = GetComponentsInChildren<Button>()[2];
		btn_Easy.onClick.AddListener (()=>setLevel(0));
		btn_Normal.onClick.AddListener (()=>setLevel(1));
		btn_Difficult.onClick.AddListener (()=>setLevel(2));
	}

	//設定難易度，會影響清單的品項數量,克漏字數量,圖字表
	public void setLevel(int _level){
		difficultLevel = _level;
		if(_level==0){
			itemNumber = 5;
			cloze = 3;
			Img_Easy.transform.gameObject.SetActive (true);
		}else if(_level==1){
			itemNumber = 6;
			cloze = 4;
			Img_Normal.transform.gameObject.SetActive (true);
		}else if(_level==2){
			itemNumber = 7;
			cloze = 5;
			Img_Difficult.transform.gameObject.SetActive (true);
		}
		setList ();
		setGlobalvar ();
		StartCoroutine (waitload());
	}

	//從該難易度的水果，隨機存入清單中
	public void setList(){
		shoppingList = new string[itemNumber];
		string []AllFruit = db.getFruitByLevel (difficultLevel);
		int ran =0;
		int[] rantemp = new int[itemNumber];

		for (int pointer = 0; pointer < shoppingList.Length; pointer++) {
			ran = ranNumber (AllFruit.Length - 1, pointer, rantemp);
			shoppingList [pointer] = AllFruit [ran];
			rantemp [pointer] = ran;
		}
		setCloze ();
	}

	//根據克漏字數量挖空
	public void setCloze(){
		q_list = new string[itemNumber];
		string s_temp;
		for(int i = 0;i<shoppingList.Length;i++){
			q_list [i] = shoppingList [i];
			for (int j = 1; j <= cloze; j++) {
				s_temp = q_list [i];
				int ran = Random.Range(0,s_temp.Length-1);
				s_temp = s_temp.Replace (s_temp [ran],'_');//與s_temp [ran]相同的字元取代成底線
				q_list [i] = s_temp;
				//Debug.Log ("ran: "+ran+" s_temp: "+s_temp);
			}
		}
		checkoutList = q_list;
	}

	//檢查ran是否重複
	int ranNumber(int rangeMax,int pointer,int []rantemp){
		int i_ran;
		bool flag = false;
			i_ran = Random.Range (0,rangeMax);
			for (int i = 0; i < pointer; i++) {
				if (rantemp [i] == i_ran) {
					flag = true;
					break;
				} 
			}
		if (flag) {
			return ranNumber (rangeMax, pointer, rantemp);	
		}
		return i_ran;
	}

	IEnumerator waitload(){
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("main");
		Destroy (gameObject);
	}

	//結束後要存回全域變數
	public void setGlobalvar(){
		GlobalVariables.difficultLevel = difficultLevel;
		GlobalVariables.itemNumber = itemNumber;
		GlobalVariables.shoppingList = shoppingList;
		GlobalVariables.q_list = q_list;
		GlobalVariables.checkoutList = checkoutList;
		GlobalVariables.basket = new string[itemNumber*2];
	}
}
