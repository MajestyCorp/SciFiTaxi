using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legacyCamerSwitch : MonoBehaviour {
	public GameObject camera1;
	public GameObject camera2;
	public GameObject camera3;
	public GameObject camera4;
	public GameObject camera5;
	public GameObject camera6;
	public GameObject camera7;
	public GameObject camera8;
	public GameObject camera9;
	public GameObject camera10;


	void Update (){
	    if (Input.GetKeyDown ("1")) {
			onActiveFalse ();
			camera1.active = true;
		}
		if (Input.GetKeyDown ("2")) {
			onActiveFalse ();
			camera2.active = true;
		}
		if (Input.GetKeyDown ("3")) {
			onActiveFalse ();
			camera3.active = true;
		}
		if (Input.GetKeyDown ("4")) {
			onActiveFalse ();
			camera4.active = true;
		}
		if (Input.GetKeyDown ("5")) {
			onActiveFalse ();
			camera5.active = true;
		}
		if (Input.GetKeyDown ("6")) {
			onActiveFalse ();
			camera6.active = true;
		}
		if (Input.GetKeyDown ("7")) {
			onActiveFalse ();
			camera7.active = true;
		}
		if (Input.GetKeyDown ("8")) {
			onActiveFalse ();
			camera8.active = true;
		}
		if (Input.GetKeyDown ("9")) {
			onActiveFalse ();
			camera9.active = true;
		}
		if (Input.GetKeyDown ("f")) {
			onActiveFalse ();
			camera10.active = true;
		}
	}

	void onActiveFalse()
	{
		camera1.active=false;
		camera2.active=false;
		camera3.active=false;
		camera4.active=false;
		camera5.active=false;
		camera6.active=false;
		camera7.active=false;
		camera8.active=false;
		camera9.active=false;
		camera10.active=false;
	}
}
