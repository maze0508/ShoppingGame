using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Checkout : MonoBehaviour {

	public Image ImgQues,fb_correct,fb_wrong;
	public Text Ques;
	public Button Btn_Submit, Btn_Reset;
	public GameObject FruitBasket;

	Button FruitinBasket,Btn_Alphabet;
	Sprite[] alphabet;
	string []basket,checkoutList,shoppingList;
	int Que_point,pointer,correctNum;
	string Quefruitimgname;

	void Awake(){
		pointer = GlobalVariables.currentCount;
		basket = GlobalVariables.basket;
		checkoutList = GlobalVariables.checkoutList;
		shoppingList = GlobalVariables.shoppingList;

		alphabet = Resources.LoadAll<Sprite>("alphabet");
		Btn_Alphabet = Resources.Load ("Button/Btn_BGG", typeof(Button)) as Button;
		createAlphaBtn ();

		FruitinBasket = Resources.Load("Button/Btn_Basket", typeof (Button))as Button;
		createBasket ();

		Que_point = 0 ;
		correctNum = 0;
	}


	void Start () {
		
		Btn_Reset.onClick.AddListener (reset);
		Btn_Submit.onClick.AddListener (submit);
		showQues ();
	}

	void showQues(){
		if (Que_point < checkoutList.Length) {
			Ques.text = checkoutList [Que_point];
			ImgQues.gameObject.SetActive (false);
			ImgQues.GetComponent<Image> ().sprite = null;
			Debug.Log ("題數:"+Que_point);
			Que_point++;
		} else {
			//End
			Debug.Log("Correct count: "+correctNum);
			setGlobalVariables ();
			SceneManager.LoadScene ("gameover");
		}

	}


	void createAlphaBtn(){
		for (int _i = 0; _i < alphabet.Length; _i++) {
			Button btnObj = Instantiate (Btn_Alphabet);//Options
			btnObj.transform.SetParent (GameObject.Find ("AlphabetContent").transform);
			btnObj.GetComponentsInChildren<Image> () [1].sprite = alphabet [_i];
			btnObj.transform.localPosition = new Vector3 (-230.0f+(_i%4)*150.0f, -(_i/4)*120.0f-80.0f, 0.0f);
			btnObj.transform.localScale = Vector3.one;
			btnObj.name = (char)(_i+97)+ "";
			btnObj.onClick.AddListener (() => setAlphabet (btnObj.name));
		}
	}

	void createBasket(){
		for (int i = 0; i < pointer; i++) {
			Button fruitObj = Instantiate (FruitinBasket);
			fruitObj.transform.SetParent (FruitBasket.gameObject.transform);
			if (i / 5 > 0) {
				fruitObj.transform.localPosition = new Vector3 (-190.0f+140.0f*(i%5), -125.0f,0.0f);
			} else {
				fruitObj.transform.localPosition = new Vector3 (-190.0f+140.0f*(i%5), -25.0f,0.0f);
			}
			fruitObj.transform.localScale = Vector3.one;
			fruitObj.name = basket[i];
			fruitObj.GetComponent<Image>().sprite = Resources.Load ("ImgFruit/" + basket[i], typeof(Sprite))as Sprite;
			fruitObj.onClick.AddListener (() => setImage (fruitObj.name ,fruitObj.GetComponent<Image>().sprite));
		}
	}

	void setAlphabet(string name){
		int underline_index = Ques.text.IndexOf ('_');//use alphabet to replace "_"
		if (underline_index != -1) {
			Ques.text = Ques.text.Remove (underline_index, 1);
			if (underline_index == 0) {
				name = name.ToUpper ();
			}
			Ques.text = Ques.text.Insert (underline_index,name);
			//Debug.Log (Ques.text);
		}

	}
	void setImage(string _fname,Sprite _fsprite){
		
		Quefruitimgname = _fname;
		ImgQues.GetComponent<Image> ().sprite = _fsprite;
		ImgQues.gameObject.SetActive (true);
		Debug.Log ("圖片為:"+Quefruitimgname);

	}


	void checkAns(){
		int _Quepoint = Que_point-1;
		if (shoppingList [_Quepoint] == Ques.text) {
			if (shoppingList [_Quepoint] == Quefruitimgname ) {
				correctNum++;
				StartCoroutine (showfeedback(0));
				Debug.Log ("Correct");
			} else {
				StartCoroutine (showfeedback(1));
				Debug.Log ("Wrong Image!");
			}
		} else {
			if (shoppingList [_Quepoint] == Quefruitimgname) {
				StartCoroutine (showfeedback(2));
				Debug.Log ("Wrong name!");
			} else {
				StartCoroutine (showfeedback(3));
				Debug.Log ("Wrong name and image!");
			}
		}
	}

	IEnumerator showfeedback(int _state){
		int _Quepoint = Que_point-1;


		if (_state == 0) {
			fb_wrong.gameObject.SetActive (false);
			fb_correct.gameObject.SetActive (true);
		}else{
			fb_correct.gameObject.SetActive (false);
			fb_wrong.gameObject.SetActive (true);
			fb_wrong.GetComponentsInChildren<Image> () [2].sprite = Resources.Load ("ImgFruit/" + shoppingList [_Quepoint], typeof(Sprite))as Sprite;
			fb_wrong.GetComponentsInChildren<Text>()[0].text = shoppingList[_Quepoint];
			//Debug.Log (fb_wrong.GetComponentsInChildren<Image> () [2]);
		}

		yield return new WaitForSeconds (2.0f);
		fb_correct.gameObject.SetActive (false);
		fb_wrong.gameObject.SetActive (false);
		showQues ();
	}


	void reset(){
		--Que_point;
		showQues ();
	}

	void submit(){
		checkAns ();
	}

	void setGlobalVariables () {
		GlobalVariables.correctCount = correctNum;
	}
}
