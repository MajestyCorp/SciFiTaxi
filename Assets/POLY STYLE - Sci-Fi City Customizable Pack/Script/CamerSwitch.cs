using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerSwitch : MonoBehaviour {
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


	void setCamera01 (){
		onActiveFalse ();
		camera1.active=true;
	}
	void setCamera02 (){
		onActiveFalse ();
		camera2.active=true;
	}
	void setCamera03 (){
		onActiveFalse ();
		camera3.active=true;
	}
	void setCamera04 (){
		onActiveFalse ();
		camera4.active=true;
	}
	void setCamera05 (){
		onActiveFalse ();
		camera5.active=true;
	}
	void setCamera06 (){
		onActiveFalse ();
		camera6.active=true;
	}
	void setCamera07 (){
		onActiveFalse ();
		camera7.active=true;
	}
	void setCamera08 (){
		onActiveFalse ();
		camera8.active=true;
	}
	void setCamera09 (){
		onActiveFalse ();
		camera9.active=true;
	}
	void setCamera10 (){
		onActiveFalse ();
		camera10.active=true;
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
