using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour {
	private int difficultLevel,itemNumber ;
	private string[]shoppingList,q_list,checkoutList,AllFruit,AllVeg;

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
			itemNumber = 10;//fruit*5 + vegeteble*5
			cloze = 3;

		}else if(_level==1){
			itemNumber = 12;
			cloze = 4;
		}else if(_level==2){
			itemNumber = 14;
			cloze = 5;
		}
		setList ();
		setGlobalvar ();
		showTable ();
	}



	//從該難易度的水果，隨機存入清單中
	public void setList(){
		shoppingList = new string[itemNumber];
		AllFruit = db.getFVByLevel (difficultLevel,2);
		AllVeg = db.getFVByLevel (difficultLevel,1);
		int ran =0;
		int[] frantemp = new int[itemNumber];

		for (int pointer = 0; pointer < itemNumber/2; pointer++) {
			ran = ranNumber (AllFruit.Length - 1, pointer, frantemp);
			shoppingList [pointer] = AllFruit [ran];
			frantemp [pointer] = ran;
		}

		/*Vegetable*/
		int []vrantemp = new int[itemNumber];
		int ran_pointer = 0;
		  for (int pointer = itemNumber/2; pointer < itemNumber; pointer++) {
			ran = ranNumber (AllVeg.Length - 1, ran_pointer, vrantemp);
			//Debug.Log ("Veg"+AllVeg.Length);
			shoppingList [pointer] = AllVeg [ran];
			//Debug.Log ("ran"+ran);
			vrantemp [ran_pointer] = ran;
			ran_pointer++;
		}


		db.closeDBConnecting ();
		setCloze ();
	}

	//根據克漏字數量挖空
	public void setCloze(){
		q_list = new string[itemNumber];
		checkoutList = new string[itemNumber];
		string s_temp;
		for(int i = 0;i<shoppingList.Length;i++){
			if (shoppingList [i] != null) {
				q_list [i] = shoppingList [i];
				int[] rantemp = new int[cloze];
				int ran = 0;
				for (int j = 0; j < cloze; j++) {
					if(j == q_list[i].Length){
						break;
					}
					s_temp = q_list [i];
					ran = ranNumber (s_temp.Length - 1, j- 1, rantemp);
					rantemp [j] = ran;
					//int ran = Random.Range (0, s_temp.Length - 1);
					//Debug.Log ("ran "+ran+",word: "+s_temp);
					s_temp = s_temp.Remove (ran, 1);
					//Debug.Log ("afterremove: "+s_temp);
					s_temp = s_temp.Insert (ran, "_");
					//Debug.Log (ran+" afterinsert:"+s_temp);
					q_list [i] = s_temp;
				}
				checkoutList [i] = q_list [i];
			}
		}
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

	void showTable(){
		if (UIManager.Instance.IsUILive ("ChooseLevel")) {
			UIManager.Instance.ClosePanel ("ChooseLevel");
		}
		UIManager.Instance.ShowPanel ("Figure");
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
