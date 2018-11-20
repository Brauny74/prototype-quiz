using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour {

	private LevelManager l_m;
	// Use this for initialization
	void Start () {
		l_m = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager>();
	}

	public void OnClick(){
		int ln = l_m.LevelNumber + 1;
		SceneManager.LoadScene ("Level" + ln, LoadSceneMode.Single);
	}
}
