using UnityEngine;  
using System.Collections;  
using UnityEngine.UI;  
  
public class customizableSliderEvent : MonoBehaviour {  
	public GameObject botton;
	public GameObject central;
	public GameObject top;
	public GameObject B1;
	public GameObject B2;
	public GameObject B3;
	public GameObject B4;
	public GameObject B5;
	public GameObject C1;
	public GameObject C2;
	public GameObject C3;
	public GameObject C4;
	public GameObject C5;
	public GameObject T1;
	public GameObject T2;
	public GameObject T3;
	public GameObject T4;
	public GameObject T5;
  
      
    public Slider targetSliderOject1;
	public Slider targetSliderOject2;
	public Slider targetSliderOject3;
  
   
    void Start () {  
	setBotton ();
	setCentral ();
	setTop ();
	B1.SetActive(true);
	central.transform.localPosition = new Vector3(0, 0.358f, 0);
	C1.SetActive(true);
	top.transform.localPosition = new Vector3(0, 0.358f, 0);
	T1.SetActive(true);
    }  
      
    // Update is called once per frame  
    void Update () {    
        //targetTextObject.text = "" + targetSliderOject.value;

    }  
	void onValueChanged (){
		if(targetSliderOject1.value == 0)
		{
			setBotton ();
			B1.SetActive(true);
			central.transform.localPosition = new Vector3(0, 0.329f, 0);						
		}
		if(targetSliderOject1.value == 1)
		{
			setBotton ();
			B2.SetActive(true);
			central.transform.localPosition = new Vector3(0, 0.394f, 0);
		}
		if(targetSliderOject1.value == 2)
		{
			setBotton ();
			B3.SetActive(true);
			central.transform.localPosition = new Vector3(0, 0.306f, 0);						
		}
		if(targetSliderOject1.value == 3)
		{
			setBotton ();
			B4.SetActive(true);
			central.transform.localPosition = new Vector3(0, 0.414f, 0);
		}
		if(targetSliderOject1.value == 4)
		{
			setBotton ();
			B5.SetActive(true);
			central.transform.localPosition = new Vector3(0, 0.4508f, 0);
		}
		if(targetSliderOject2.value == 0)
		{
			setCentral ();
			C1.SetActive(true);
			top.transform.localPosition = new Vector3(0, 0.658f, 0);						
		}
		if(targetSliderOject2.value == 1)
		{
			setCentral ();
			C2.SetActive(true);
			top.transform.localPosition = new Vector3(0, 0.876f, 0);
		}
		if(targetSliderOject2.value == 2)
		{
			setCentral ();
			C3.SetActive(true);
			top.transform.localPosition = new Vector3(0, 0.469f, 0);						
		}
		if(targetSliderOject2.value == 3)
		{
			setCentral ();
			C4.SetActive(true);
			top.transform.localPosition = new Vector3(0, 0.33f, 0);
		}
		if(targetSliderOject2.value == 4)
		{
			setCentral ();
			C5.SetActive(true);
			top.transform.localPosition = new Vector3(0, 0.799f, 0);
		}
		if(targetSliderOject3.value == 0)
		{
			setTop ();
			T1.SetActive(true);						
		}
		if(targetSliderOject3.value == 1)
		{
			setTop ();
			T2.SetActive(true);
		}
		if(targetSliderOject3.value == 2)
		{
			setTop ();
			T3.SetActive(true);						
		}
		if(targetSliderOject3.value == 3)
		{
			setTop ();
			T4.SetActive(true);
		}
		if(targetSliderOject3.value == 4)
		{
			setTop ();
			T5.SetActive(true);
		}
	}

    void setBotton (){
	central.transform.localPosition = Vector3.zero;
	B1.SetActive(false);
	B2.SetActive(false);
	B3.SetActive(false);
	B4.SetActive(false);
	B5.SetActive(false);
	}
	void setCentral (){
		top.transform.localPosition = Vector3.zero;
		C1.SetActive(false);
		C2.SetActive(false);
		C3.SetActive(false);
		C4.SetActive(false);
		C5.SetActive(false);
	}
	void setTop (){
		T1.SetActive(false);
		T2.SetActive(false);
		T3.SetActive(false);
		T4.SetActive(false);
		T5.SetActive(false);
	}
} 