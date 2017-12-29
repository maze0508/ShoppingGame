using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShoppingList : MonoBehaviour {
	
	public Button btn_Back;
	public GameObject listcontent;

	Image context;
	Text Name,HintInfo;
	SQLDB db;
	string[] q_list,shoppingList;

	void Start () {
		context = Resources.Load ("Object/Context",typeof(Image)) as Image;
		db = gameObject.AddComponent<SQLDB> ();
		q_list = GlobalVariables.q_list;
		shoppingList = GlobalVariables.shoppingList;

		for (int i = 0; i < q_list.Length; i++) {
			if (shoppingList [i] != null) {
				Image newObj = Instantiate (context);
				newObj.transform.SetParent (listcontent.transform);
				newObj.name = shoppingList [i];
				newObj.transform.localPosition = new Vector3 (0.0f, -50.0f - (90.0f * i), 0.0f);
				newObj.transform.localScale = Vector3.one;
				Text newText = newObj.GetComponentsInChildren<Text> () [0];
				newText.text = q_list [i];
				//Debug.Log ("checkoutList: "+GlobalVariables.checkoutList[i]);

				HintInfo = newText.GetComponentsInChildren<Text> () [1];
				HintInfo.text = db.getFVshortInfo (newObj.name);
			}
		}
		btn_Back = GetComponentsInChildren<Button>()[0];
		btn_Back.onClick.AddListener (backMain);

	}

	void backMain(){
		if (UIManager.Instance.IsUILive ("ShoppingList")) {
			UIManager.Instance.ClosePanel ("ShoppingList");	
			GlobalVariables.flag = true;
		}
	}
}
