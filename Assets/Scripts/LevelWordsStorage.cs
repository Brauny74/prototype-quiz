using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWordsStorage : MonoBehaviour {

	public List<string> words = new List<string>();

	public void Init(){
		//for uniformity, make all letter big
		for (int i = 0; i < words.Count; i++) {
			words [i] = words [i].ToUpper ();
		}
	}

}
