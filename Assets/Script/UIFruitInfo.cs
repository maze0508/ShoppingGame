using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFruitInfo:MonoBehaviour {
	public Button btn_Addto, btn_BackMain; 
	Image fruitSprite;
	Text fruitName,fruitInfo;
	GameObject Fruit;
	SQLDB db ;
	//FruitEventListener Fs;

	void Awake(){//show FruitInfo UI
		db = gameObject.AddComponent<SQLDB> ();
		if (!UIManager.Instance.IsUILive ("FruitInfo") && !UIManager.Instance.IsUILive ("Mask")) {
			UIManager.Instance.ShowPanel ("Mask");
			UIManager.Instance.ShowPanel ("FruitInfo");
			fruitSprite = GetComponentsInChildren<Image> () [8];
			fruitName = GetComponentsInChildren<Text>()[1];
			fruitInfo = GetComponentsInChildren<Text>()[2];
			btn_Addto = GetComponentsInChildren<Button>()[2];
			btn_BackMain = GetComponentsInChildren<Button>()[3];

			btn_Addto.onClick.AddListener (showQus);
			btn_BackMain.onClick.AddListener (backMain);

			//Fs = new FruitEventListener ();
		}
	}
		

	public void setFruitContent(string _fruitName){ //set the target fruit's name and information content.
		fruitName.text = _fruitName;//change name
		fruitSprite.sprite = Resources.Load ("ImgFruit/"+fruitName.text, typeof(Sprite)) as Sprite;//change Img
		fruitInfo.text = db.getFruitInfo(_fruitName);//change information
		db.closeDBConnecting();
	}


	void showQus(){
			UIManager.Instance.TogglePanel ("FruitInfo",false);
			UIAdd info = gameObject.AddComponent<UIAdd> ();
			info.setFruitName (fruitName.text);
	}

	void backMain (){
		if (UIManager.Instance.IsUILive ("FruitInfo") && UIManager.Instance.IsUILive ("Mask")) {
			UIManager.Instance.ClosePanel ("FruitInfo");
			UIManager.Instance.ClosePanel ("Mask");
			GlobalVariables.flag = true;
			//Fs.stop = false;

		}
	}
}
