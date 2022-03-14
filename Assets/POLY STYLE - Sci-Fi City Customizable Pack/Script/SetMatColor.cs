using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMatColor : MonoBehaviour {

	public Material mat1;
	public Material mat2;
	//public Color color = new Color(0.2F, 0.3F, 0.4F, 0.5F);
	public Slider targetSliderOject;
	public Color color1;
	public Color color2;
	public Color color3;
	public Color color4;
	public Color color5;
	public Color color6;
	public Color color7;
	public Color color8;
	public Color color9;
	public Color color10;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onValueChanged (){
		if (targetSliderOject.value == 0) {
			mat1.SetColor("_Color", color1);
			mat2.SetColor("_Color", color2);
		}
		if (targetSliderOject.value == 1) {
			mat1.SetColor("_Color", color3);
			mat2.SetColor("_Color", color4);
		}
		if (targetSliderOject.value == 2) {
			mat1.SetColor("_Color", color5);
			mat2.SetColor("_Color", color6);
		}
		if (targetSliderOject.value == 3) {
			mat1.SetColor("_Color", color7);
			mat2.SetColor("_Color", color8);
		}
		if (targetSliderOject.value == 4) {
			mat1.SetColor("_Color", color9);
			mat2.SetColor("_Color", color10);
		}
	}

}
