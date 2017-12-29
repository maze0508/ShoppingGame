using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowFigure : MonoBehaviour {

	public Image Img_Easy,Img_Normal,Img_Difficult;
	Image BG;
	Text loading;
	string[]AllFruit,AllFruitCN,AllVeg,AllVegCN;
	int difficultLevel,_i;
	string s_letter;
	SQLDB db;

	void Awake () {
		difficultLevel = GlobalVariables.difficultLevel;
		BG = GetComponentsInChildren<Image> () [0];
		loading = GetComponentsInChildren<Text> () [0];
		db = gameObject.AddComponent<SQLDB> ();

		AllFruit = db.getFVByLevel (difficultLevel,2);
		AllFruitCN = db.getCNByFV (difficultLevel,2);
		AllVeg = db.getFVByLevel (difficultLevel,1);
		AllVegCN = db.getCNByFV (difficultLevel,2);

		_i = 0;
		s_letter = "";
		InvokeRepeating ("letter",0.2f,0.2f);
		StartCoroutine (showTable());
	}

	IEnumerator showTable(){

		Text t_allfruitname;
		Image TableName = null;

		switch (difficultLevel) {
		case 0:
			TableName = Img_Easy;
			break;
		case 1:
			TableName = Img_Normal;
			break;
		case 2:
			TableName = Img_Difficult;
			break;
		}

		t_allfruitname = TableName.GetComponentInChildren<Text> ();
		for (int i = 0; i < AllFruit.Length; i++) {
			if(i/2>0 && i%2==0){
				t_allfruitname.text += " \n ";
			}
			t_allfruitname.text += AllFruit [i]+" "+ AllFruitCN [i];
			t_allfruitname.text += " \t ";

			//t_allfruitname.text += AllVeg [i]+" "++ AllVegCN [i];
			//t_allfruitname.text += "\t";
		}
		db.closeDBConnecting ();
		TableName.transform.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("main");

	}

	void letter(){

		switch(_i%8){
		case 0:
			s_letter =  "L";
			break;
		case 1:
			s_letter =  "o";
			break;
		case 2:
			s_letter =  "a";
			break;
		case 3:
			s_letter =  "d";
			break;
		case 4:
			s_letter =  "i";
			break;
		case 5:
			s_letter =  "n";
			break;
		case 6:
			s_letter = "g";
			break;
		case 7:
			s_letter = "";
			loading.text = "";
			break;
		}
		_i++;
		loading.text += s_letter;
	}
}
