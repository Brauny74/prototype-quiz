using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

	public string content;

	public Text textField; 

	void Start () {
		textField = GetComponentInChildren<Text> ();
		textField.text = content;
		//we hide letters by just making them white
		textField.color = new Color (1f, 1f, 1f, 0f);
	}

	public void ShowContent(){
		textField.color = new Color (0f, 0f, 0f, 1f);
	}
}
