using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public int LevelNumber = 0;
	public LevelWordsStorage Storage;
	public RectTransform SlotsContent;
	public RectTransform LettersPool;
	public RectTransform LettersInputField;

	public GameObject SlotPrefab;
	public GameObject LetterPrefab;
	public GameObject TickPrefab;

	public GameObject FinishLevelPopup;

	public bool IsPlayable = true;

	public string CurrentInputWord;
	public float CurrentInputPos = -165f;
	public List<Letter> CurrentLettersInInput = new List<Letter> ();

	public List<string> currentWords = new List<string> ();
	public List<string> remainingWords = new List<string> ();
	private List<Letter> currentLettersPool = new List<Letter>();
	private Dictionary<string, List<Slot>> currentSlotsDict = new Dictionary<string, List<Slot>> ();

	private bool EasyMode;

	//this function is used to create and initialize a pool of letters to enter
	void InitializeLetters(){
		
		foreach (Transform child in LettersPool.transform) {
			Destroy (child.gameObject);
		}

		List<char> letters = new List<char> ();
		float CX = -363f;
		float CY = 50f;
		//real letters from current words
		foreach (string word in currentWords) {			
			foreach(char c in word){
				letters.Add (c);
			}
		}
		//fake letters to throw off player
		while (letters.Count < 38) {
			string st = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
			char c = st [Random.Range (0, st.Length)];
			letters.Add (c);
		}
		//mix letters
		for (int i = 0; i < letters.Count; i++) {
			char temp = letters[i];
			int randomIndex = Random.Range(i, letters.Count);
			letters[i] = letters[randomIndex];
			letters[randomIndex] = temp;
		}
		//create pool of letters to input words from
		foreach (char c in letters) {
			Letter TLet = Instantiate (LetterPrefab, LettersPool).GetComponent<Letter> ();
			TLet.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (CX, CY);
			TLet.Init (c, this);
			currentLettersPool.Add (TLet);
			CX += 39.4f;
			if (CX >= 363f) {
				CX = -363f;
				CY -= 39.4f;
			}
		}
	}

	//This function initialize hints for current words at the top of game field.
	void InitializeSlots(){

		currentSlotsDict.Clear ();

		foreach (Transform child in SlotsContent.transform) {
			Destroy (child.gameObject);
		}

		float CX = -160f;
		float CY = 50f;
		foreach (string word in currentWords)
		{			
			string TWord = word.ToUpper ();
			currentSlotsDict.Add (TWord, new List<Slot> ());
			CX = -165f;
			for (int i = 0; i < TWord.Length; i++)
			{
				GameObject TSlot = Instantiate (SlotPrefab, SlotsContent);
				RectTransform TSlotTr = TSlot.GetComponent<RectTransform> ();
				TSlot.GetComponent<Slot> ().content = TWord[i].ToString().ToUpper();
				TSlotTr.anchoredPosition = new Vector2 (CX, CY);
				currentSlotsDict [TWord].Add (TSlot.GetComponent<Slot>());
				CX += 45f;
			}
			CY -= 45f;
		}
		SlotsInited = false;
	}

	void Start ()
	{
		EasyMode = SettingsManager.settings.EasyMode;//in easy mode, we can see first letter of a word in slots
		FinishLevelPopup.SetActive (false);
		Storage.Init ();
		for (int i = 0; i < 3; i++) {
			currentWords.Add (Storage.words[i]);
		}
		for(int i = 3; i < Storage.words.Count; i++){
			remainingWords.Add (Storage.words [i]);
		}
		SettingsManager.settings.SylMode = false;
		if (SettingsManager.settings.LoadedGame)
		{
			currentWords = SettingsManager.settings.GetCurrentWords();
			remainingWords = SettingsManager.settings.GetRemainingWords();
			LevelNumber = SettingsManager.settings.GetLevelNumber();
			SettingsManager.settings.LoadedGame = false;

		}

		InitializeSlots ();
		InitializeLetters ();
	}

	private bool SlotsInited = false;
	void Update () {
		if (Input.GetKeyUp (KeyCode.S)) {
			if (LevelNumber < 9) {
				int ln = LevelNumber + 1;
				SceneManager.LoadScene ("Level" + ln, LoadSceneMode.Single);
			}
		}
		if (EasyMode) {     
			if (!SlotsInited) {
				//basically, we wait a frame after creating slots, and then show first letter in a word
				foreach (KeyValuePair<string, List<Slot>> word in currentSlotsDict) {
					word.Value[0].ShowContent ();
				}
				SlotsInited = true;
			}
		}
	}

	public void ClearInput(bool destroyLetters = false){
		CurrentInputWord = "";
		CurrentInputPos = -165f;
		foreach(Letter letr in CurrentLettersInInput){
			if (destroyLetters) {
				Destroy (letr.gameObject);
			} else {
				letr.ReturnToPool ();
			}
		}
		CurrentLettersInInput.Clear ();
	}

	//in this function, we compare word in input field with current words
	//if player entered right word, we moved to next one, or to next level, if it was the last one
	public void CompareInput(){
		for (int i = 0; i < currentWords.Count; i++) {
			if (currentWords[i] == CurrentInputWord) {
				Instantiate (TickPrefab, Storage.transform);
				currentWords.RemoveAt (i);
				if (remainingWords.Count > 0) {
					currentWords.Add (remainingWords[0]);
					remainingWords.RemoveAt (0);
				}
				foreach (Slot s in currentSlotsDict[CurrentInputWord]) {
					s.ShowContent ();
				}
				IsPlayable = false;
				SettingsManager.settings.Save();
				StartCoroutine (ChangeWords());
				break;
			}
		}
	}

	IEnumerator ChangeWords(){
		yield return new WaitForSeconds(0.6f);
		ClearInput (true);
		InitializeSlots ();
		InitializeLetters ();
		FinishLevel ();
		IsPlayable = true;
	}

	public void FinishLevel(){
		if (currentWords.Count <= 0) {
			FinishLevelPopup.SetActive (true);
		}
	}
}
