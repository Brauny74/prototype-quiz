using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a tick, shown when player gets the word right
public class Tick : MonoBehaviour {

	void Start () {
		StartCoroutine (Die());
	}

	IEnumerator Die(){
		yield return new WaitForSeconds(0.6f);
		Destroy (gameObject);
	}
}
