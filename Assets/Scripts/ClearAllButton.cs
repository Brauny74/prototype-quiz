using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAllButton : MonoBehaviour {

	private LevelManager l_m;
	private LevelManagerSyl l_m_s;//level manager for syllables mode
	// Init level managers
	void Start () {
		l_m = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();
		l_m_s = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerSyl>();
	}

	public void OnClick(){
		if(l_m != null)
			l_m.ClearInput ();
		if(l_m_s != null)
			l_m_s.ClearInput();
	}
}
