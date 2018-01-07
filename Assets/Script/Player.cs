using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float walkSpeed;
	private Animator animator;
	public Camera playerCamera;
	void Start () {
		animator = GetComponent<Animator>();
	}

	void Update () {
		float _moveH;
		#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
			_moveH = MobileInput();
		#else
			_moveH = DeskopInput();
		#endif


		//player不能超出左邊界
		if (gameObject.transform.position.x < -6) {
			gameObject.transform.position = new Vector3 (-5, -2,100);
		} else {
			gameObject.transform.position += new Vector3(_moveH*0.9f,0,0);
			//player.transform.Translate (new Vector2(_moveH, 0));
		}
		//camera不能超出左邊界		
		//playerCamera.transform.Translate (new Vector2 (_moveH * 5*Time.deltaTime * walkSpeed, 0));
		playerCamera.transform.position += new Vector3(_moveH*0.8f, 0,0);
		if (playerCamera.transform.position.x < 0) 
			playerCamera.transform.position = new Vector3(0,0,0);
		
		//player左右轉向
		if(_moveH != 0.0){
			animator.SetBool ("walking", true);
			if (_moveH < 0.0) {
				gameObject.transform.localScale = new Vector3 (-1.5f, 1.5f, 0);
			} else {
				gameObject.transform.localScale = new Vector3 (1.5f, 1.5f,0);
			}
		}else{
			animator.SetBool ("walking", false);
		}

	}

	float MobileInput(){
		if (Input.touches.Length <= 0)
			return 0;
		if (Input.touches.Length == 1) {
			if (Input.touches [0].phase == TouchPhase.Ended) {
				return Input.touches [0].deltaPosition.x * walkSpeed * Time.deltaTime;
			}
		} 
		return 0;
	}

	float DeskopInput(){
		float _moveH = Input.GetAxis("Horizontal") * walkSpeed * Time.deltaTime;
		return _moveH;
	}

}
