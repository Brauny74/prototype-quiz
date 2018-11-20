using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour {

	public bool EasyMode = false;
	public Text hint;
	public string hintText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick()
	{
		SettingsManager.settings.EasyMode = EasyMode;
		SettingsManager.settings.SylMode = false;
		SceneManager.LoadScene ("Level0", LoadSceneMode.Single);
	}

	public void OnHover(){
		hint.text = hintText;
	}
}
