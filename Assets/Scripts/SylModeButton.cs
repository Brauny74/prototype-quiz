using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SylModeButton : MonoBehaviour {

	public Text hint;
	public string hintText;

	public void OnHover()
	{
		hint.text = hintText;
	}

	public void OnClick()
	{
		SettingsManager.settings.SylMode = true;
		SceneManager.LoadScene("Level_SylM0", LoadSceneMode.Single);
	}
}
