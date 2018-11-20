using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This is a main component for letter element, which is used by player to form words
public class Letter : MonoBehaviour {

	public Text textField;
	public char content;

	private bool IsInPool;//letter can be either in pool or in input field
	private LevelManager l_m;
	private Vector2 InPoolPos;//I keep its position in a pool to put it back, when I clear Input Field
	// Use this for initialization
	public void Init (char C, LevelManager tl_m) {
		l_m = tl_m; //we get level manager from itself
		InPoolPos = GetComponent<RectTransform> ().anchoredPosition;
		IsInPool = true;
		textField = GetComponentInChildren<Text> ();
		content = C;
		textField.text = content.ToString ();
	}

	public void OnClick(){
		if (IsInPool && l_m.IsPlayable) {
			float CX = l_m.CurrentInputPos;
			GetComponent<RectTransform> ().SetParent (l_m.LettersInputField);
			GetComponent<RectTransform> ().anchoredPosition = new Vector2 (CX, 0.0f);
			l_m.CurrentInputPos += 38.4f;
			l_m.CurrentLettersInInput.Add (this);
			l_m.CurrentInputWord += content.ToString ();

			l_m.CompareInput ();

			IsInPool = false;
		}
	}

	public void ReturnToPool(){
		if (!IsInPool) {
			GetComponent<RectTransform> ().SetParent (l_m.LettersPool);
			GetComponent<RectTransform> ().anchoredPosition = InPoolPos;
			IsInPool = true;
		}
	}
}
