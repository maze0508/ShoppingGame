using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIAdd : MonoBehaviour {
	public Button btn_BackInfo,btn_submit,btn_reset,Btn_BGW,Btn_BGP,Btn_BGB,Btn_BGG,Btn_BGY;
	public string fruitName;
	Image UI_Feedback,fruitImg;
	int i_btnNum =4;//按鈕物件數量
	int currentQue,NameLength,currentCount,itemNum;//目前指向第幾個的g_Que物件;
	Sprite[] alphabet,FruitAlphabet ;
	Button[] btnObj;
	GameObject[] totalOption,totalQuestion;
	int[] ranOption;//存放每次隨機排序的字母ASCII
	string userAns;
	string [] shoppingList,q_List,basket;

	void Awake () {
		if (!UIManager.Instance.IsUILive ("AddToBasket")) {
			UIManager.Instance.ShowPanel ("AddToBasket");
			UI_Feedback = GetComponentsInChildren<Image> () [19];
			//Debug.Log (UI_Feedback.name);
			UI_Feedback.enabled = false;

			fruitImg = GetComponentsInChildren<Image> () [9];
			btn_BackInfo = GetComponentsInChildren<Button> () [2];
			btn_reset = GetComponentsInChildren<Button> () [3];
			btn_submit = GetComponentsInChildren<Button> () [4];
			btn_BackInfo.onClick.AddListener (backInfo);
			btn_reset.onClick.AddListener (reset);
			btn_submit.onClick.AddListener (submit);

			alphabet = Resources.LoadAll<Sprite>("alphabet");//讀取multi sprite的方式讀取字母圖案，alphabet[0]~alphabet[25]:A~Z
		}
	}
	void Start(){
		itemNum = GlobalVariables.itemNumber;
		basket = new string[itemNum*2];
		currentCount = GlobalVariables.currentCount;
		shoppingList = GlobalVariables.shoppingList;
		q_List = GlobalVariables.q_list;

		//載入按鈕物件
		Btn_BGW = Resources.Load ("Button/Btn_BGW", typeof(Button)) as Button;
		Btn_BGP = Resources.Load ("Button/Btn_BGP", typeof(Button)) as Button;
		Btn_BGB = Resources.Load ("Button/Btn_BGB", typeof(Button)) as Button;
		Btn_BGG = Resources.Load ("Button/Btn_BGG", typeof(Button)) as Button;
		Btn_BGY = Resources.Load ("Button/Btn_BGY", typeof(Button)) as Button;

		btnObj = new Button[]{Btn_BGP,Btn_BGB,Btn_BGG,Btn_BGY};
		init ();
		createButton ();
	}

	void init(){
		userAns = "";
		currentQue = 0;
		NameLength = fruitName.Length;
		FruitAlphabet = createFSprite (NameLength);
		totalOption = new GameObject[NameLength];
		totalQuestion = new GameObject[NameLength];

	}

	void clear() {
		for (int i = 0; i < totalOption.Length; ++i) {
			if (totalOption [i] != null) {
				Destroy (totalOption [i].gameObject);
			}
			if (totalQuestion [i] != null) {
				Destroy (totalQuestion [i].gameObject);
			}
		}
	}

	bool checkOdd(){//判斷單字字母奇偶數個
		if (NameLength % 2 > 0) {
			return  true;
		} 
		return  false;
	}

	float setPos(bool odd,int _L){//set button object's position
		if (odd) {
			return 120 * _L;
		} 
		return(110 * _L) + 55;
	}

	void createButton(){//生成按鈕
		int _header = 0, avg = NameLength / 2, _L = avg*-1;//_L表示從最左邊開始生成btn物件
		bool odd =checkOdd ();
		int _i = 0;
		while(NameLength>0){//根據單字字母數量，按btnObg陣列順序生成按鈕物件
			if (_header >= i_btnNum) {//指標紀錄目前array位置
				_header %= i_btnNum;
			}
			createQues (_L,odd,_i);
			Button g_btnObj = Instantiate (btnObj [_header]);//Options
			g_btnObj.transform.SetParent (GameObject.Find ("Content").transform);
			g_btnObj.GetComponentsInChildren<Image> () [1].sprite = FruitAlphabet [_i];
			g_btnObj.transform.localPosition = new Vector3 (setPos(odd,_L), -70.0f,0.0f);
			g_btnObj.transform.localScale = Vector3.one;
			g_btnObj.name = (char)(ranOption[_i]+97) + "";
			g_btnObj.onClick.AddListener (() => showAlphabet(g_btnObj));
			totalOption [_i] = g_btnObj.gameObject;
			_L++;
			_i++;
			_header++;
			NameLength--;

		}
	}

	void createQues(int _L,bool _odd,int count){
		Button g_btnQue = Instantiate (Btn_BGW);//Questions
		g_btnQue.transform.SetParent (GameObject.Find ("Questions").transform);
		g_btnQue.transform.localPosition = new Vector3 (setPos(_odd,count), 0.0f,0.0f);
		//g_btnQue.transform.localPosition = new Vector3 (setPos(_odd,_L)+220.0f, 0.0f,0.0f);
		g_btnQue.transform.localScale = Vector3.one;
		g_btnQue.name = "q"+count;
		totalQuestion [count] = g_btnQue.gameObject;
	}

	Sprite[]createFSprite(int _NameLength){//create an array to store the fruit's sprite.
		string s_fruitName = fruitName.ToLower ();
		FruitAlphabet = new Sprite[_NameLength];//隨機後的字母圖片
		ranOption = new int[_NameLength];//隨機後的字母ASCII碼
		int ran;
		for(int i =0;i<_NameLength;i++){

			ran = Random.Range (0, (s_fruitName.Length-1));
			int temp = ((int)s_fruitName [ran])%97;
			ranOption [i] = temp;
			FruitAlphabet[i] = alphabet [temp];

			string _str = "";
			for (int j = 0; j < ran; j++) {
				_str += s_fruitName [j];
			}
			for (int j = ran+1; j < s_fruitName.Length; j++) {
				_str += s_fruitName [j];
			}
			s_fruitName = _str;//將每次random所提出的字母存入FruitAlphabet陣列，並從s_fruitName中拿掉
		}
		return FruitAlphabet;
	}

	void showAlphabet(Button _trigger){
		userAns+=_trigger.name;//將點擊的選項存入usrAns
		int _temp = ((int)_trigger.name[0])%97;
		GameObject.Find ("q"+currentQue).GetComponentsInChildren<Image> () [1].sprite = alphabet [_temp];
		for (int i = 0; i < totalOption.Length; i++) {
			if (totalOption [i] == _trigger.gameObject)
				totalOption [i] = null;
		}
		currentQue++;//第幾個空格的指標
		Destroy(_trigger.gameObject);//按鈕點擊後消失
	}


	void reset(){
		clear ();
		init ();
		createButton();
	}

	void submit(){
		string s_text;
		bool flag = false;
		if (userAns.CompareTo (fruitName.ToLower())==0) {
			s_text = "Succeed to add!";
			flag = true;
			StartCoroutine (showfeedback(s_text,flag));
		} else {
			s_text = "Fail to add!";
			StartCoroutine (showfeedback(s_text,flag));
		}
	}

	IEnumerator showfeedback(string _text,bool _correct){
		UI_Feedback.enabled = true;//show UI
		UI_Feedback.GetComponent<Animator> ().enabled = true;//show Animation
		Text feedback = UI_Feedback.GetComponentInChildren<Text> ();
		feedback.text = _text;
		yield return new WaitForSeconds (1f);
		if (_correct) {
			success ();
		} else {
			UI_Feedback.enabled = false;
			UI_Feedback.GetComponent<Animator> ().enabled = false;
			feedback.text = "";
		}
	}

	void success(){//如果回答正確，則返回UI_FruitInfo
		for(int i =0;i<shoppingList.Length;i++){//如果此水果是清單上的水果
			if(shoppingList[i]==fruitName){
				q_List [i] = fruitName;
			}
		}
		setBasket (fruitName);//加入購物籃
		showBasket ();
	}

	public void setBasket(string _fName){
		for(int i = 0; i<currentCount;i++){
			if (basket[i]==_fName) {//此水果已存在購物籃內
				break;
			}
		}
		basket[currentCount] = _fName;//目前購物籃內並沒有重複的此水果
		setGlobalVar ();
	}
		
	void setGlobalVar(){

		GlobalVariables.basket[currentCount] = basket[currentCount];
		Debug.Log (GlobalVariables.basket[0] );
		GlobalVariables.currentCount = ++currentCount;

	}

	public void setFruitName(string s_fruitname){
		fruitName = s_fruitname;
		fruitImg.sprite = Resources.Load ("ImgFruit/"+fruitName, typeof(Sprite)) as Sprite;//change Img
	}
	void backInfo (){
		if (UIManager.Instance.IsUILive ("AddToBasket")) {
			UIManager.Instance.ClosePanel ("AddToBasket");
			UIManager.Instance.TogglePanel ("FruitInfo",true);
		}
	}
	void showBasket(){
		if (UIManager.Instance.IsUILive ("AddToBasket")) {
			UIManager.Instance.CloseAllPanel ();
			UIManager.Instance.ShowPanel ("Basket");
		}
	}
}
