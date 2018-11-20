using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//this is basically the same as Level Manager, but for syllable mode
//instead of letters, player form words from short chains of letters
public class LevelManagerSyl : MonoBehaviour {
	
	public int LevelNumber = 0;
	public LevelWordsStorage Storage;
	public RectTransform SlotsContent;
	public RectTransform SyllablesPool;
	public RectTransform SyllablesInputField;

	public GameObject SlotPrefab;
	public GameObject SylPrefab;
	public GameObject TickPrefab;

	public Text RemainingWordsCounterPrefab;
	private Text RemainingWordsCounter;

	public GameObject FinishLevelPopup;

	public bool IsPlayable = true;

	public string CurrentInputWord;
	public float CurrentInputPos = -165f;
	public List<Syllable> CurrentSyllablesInInput = new List<Syllable>();

	public List<string> currentWords = new List<string>();
	public List<string> remainingWords = new List<string>();
	private List<Syllable> currentSyllablesPool = new List<Syllable>();
	private Dictionary<string, List<Slot>> currentSlotsDict = new Dictionary<string, List<Slot>>();
	public Dictionary<string, int> currentOpenedSlots = new Dictionary<string, int>();

	void InitializeSyllables()
	{
		foreach (Transform child in SyllablesPool.transform)
		{
			Destroy(child.gameObject);
		}

		List<string> slbs = new List<string>();

		float CX = -209f;
		float CY = 45f;
		bool afterSngSlb = false;
		foreach (string word in currentWords)
		{
			if (word.Length % 2 == 0)
			{
				for (int k = 0; k < word.Length - 1; k++)
				{
					if (k % 2 == 0)
					{
						slbs.Add(word.Substring(k, 2));
					}
				}
			}
			else
			{
				afterSngSlb = false;
				int sng_slb = Random.Range(0, word.Length);
				while (sng_slb % 2 != 0)
				{
					sng_slb = Random.Range(0, word.Length);
				}

				for (int k = 0; k < word.Length - 1; k++)
				{
					if (k == sng_slb)
					{
						afterSngSlb = true;
						slbs.Add(word[k].ToString());
					}

					if (afterSngSlb)
					{
						if(k % 2 != 0)
							slbs.Add(word.Substring(k, 2));
					}
					else
					{
						if(k % 2 == 0)
							slbs.Add(word.Substring(k, 2));
					}
				}

				if (!afterSngSlb)
				{
					slbs.Add(word[word.Length - 1].ToString());
				}
			}
		}

		while (slbs.Count < 18)
		{
			//here we create fake syllables

			//this algorithm created unnatural syllables
			/*string abc = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
			string st = abc[Random.Range(0, abc.Length)].ToString();
			if (Random.Range(0, 2) == 0)
			{
				st += abc[Random.Range(0, abc.Length)].ToString();
			}*/

			//this is a set of random parts of real words, might be more natural
			string[] st = {"ИЯ","ИД","ЫХ","Х","ОП","ПЕ","М","ОС","ОС","ОВ","НЕ","ИМ","НА","ЩЕ","ЗЛ","СП","ЬТ","СТ","РО","НА","ИЯ","СТ","И","СТ","ЕТ","ТИ","РА","ЛЬ","Ж","МУ","ТО","НН","ИИ","ЛЬ","ГУ","ВА","О","ОВ","УЧ","УТ","ЗВ","ИЕ","ИЙ","НЫ","ЖН","ИГ","СТ","КА","ЮТ","АН","КО","РЕ","РА","ОЖ","ИТ","БЛ","ЭТ","Д","ЕС","Л","Ч","ИР","ЫС","ДЕ","В","НИ","ЗН","РМ","ЛИ","ЁН","ЧИ","БУ","Я","Е","ОВ","ИЧ","РЯ","ФО","РА","ЕД","ИА","ЫМ","А","ВН","ИМ","ЙН","ОК","НА","ЛЬ","АН","ОБ","УЮ","ЫХ","З","ИР","АК","НИ","ИЯ","НЕ","НО","РМ","СО","Я","И","ФО","РА","ДЕ","СУ","С","АЦ","ДА","МО","КО","ЕЛ","ОМ","ФИ","ВЕ","ЕМ","ИД","В","ИХ","ОВ","АЧ","ВА","ШИ","ШЕ","ДЕ","В","ЙШ","ЗВ","ПР","ОЧ","ОБ","КО","ЫХ","ВО","ОЧ","НС","ЕЦ","ОБ","РО","В","ГО","ТЬ","Ш","РУ","О","НС","ТИ","РА","УЛ","ПР","ЕН","ИР","ИИ","ЯТ","ЛЕ","ТР","Ё","Е","ЛИ","ПО","РМ","ЫЕ","ЕВ","ЁТ","ФО","РА","К","ИЕ","ЬН","ИТ","РА","ЗЛ","ЬН","К","ЕЕ","П","ЙШ","ЕН","ЛИ","СЬ","ИТ","ОВ","РА","ПО","МО","НЕ","ИХ","ДА","ЫМ","КО","ДЕ","АЦ","ОЕ","ЫЕ","АД","УЮ","С","З","ТИ","АН","СК","КО","ИО","ШИ","РА","ИН","ТИ","ЫЕ","ВН","ВА","КИ","ТО","ИЧ","РИ","ТИ","ИЯ","ТЬ","РА","ЬН","НЕ","ОС","ИЙ","ПЛ","М","ДА","ЗВ","ЛИ","ТИ","ТЬ","И","П","АГ","ТР","НЕ","Н","ВЫ","ИЗ","БУ","ОП","ПО","ТЬ","ОБ","ИЯ","ИТ","ЗА","АЯ","ОС","РА","ШИ","ОБ","УТ","ФО","УС","ВА","Щ","НЕ","РА","ДЕ","НИ","ТЬ","ОЯ","РА","ЩИ","ЕТ","ИЯ","БЫ","ИЕ","НЕ","С","КЕ","ВО","ОП","ШИ","СН","СТ","ЛИ","ТЕ","ЕН","В","РА","РМ","ЩИ","ОТ","ОВ","ОС","ОЕ","ЧТ","ВА","ИИ","П","ВА","ВА","ЗВ","РМ","РО","ОП","ФО","ДЕ","ВС","УК","ДЕ","ТО","ИЯ","ЕД","ИТ","РЫ","ЕТ","ИТ","ЬТ","МИ","Н","ДИ","М","ГО","ОВ","Я","ГУ","ЧУ","ИЕ","ИЯ","УЧ","КИ","ДЛ","АН","Б","ЫХ","АШ","НС","ИТ","ЕЙ","РМ","ЗВ","ВН","СТ","ОБ","РЕ","ЕЦ","Л","ЗЫ","ВН","ИР","ДЕ","К","АК","АЛ","ЕС","ОТ","УД","ОД","Н","РА","АЦ","РЕ","ПЕ","ЗВ","ЧИ","НИ","СП","И","ОМ","ИН","ЕЛ","НН","ФО","АЙ","АК","ТЕ","ИТ","ПО","УК","ЛЮ","РА","ЫХ","ОЛ","НА","АБ","ИЕ","РЕ","РА","ТИ","ЗВ","Ч","ЛЕ","ТО","ВУ","ВА","ИЯ","АК","СТ","ЗВ","У","РО","ОЧ","КА","ЕЧ","ОБ","ОБ","СТ","ЕТ","ЕЛ","ЕС","П","ЕН","ЗА","ДА","А","У","ВС","ЮТ","Р","РА","СП","РА","Н","ЛЬ","КО","ВА","ЗА","ИЯ","СТ","РУ","ДГ","УЛ","О","ФО","ДА","ОЖ","ЗР","РИ","РМ","ИЙ","ЕТ","ВЛ","С","ПО","ЙШ","ИЯ","ДА","РА","ЕН","ЕС","АЦ","РЕ","ЕГ","СЛ","ЯТ","ЯТ","ПО","ИЯ","И","ЖН","СТ","МУ","ЕД","ЛИ","ЗВ","НН","ТУ","ОЛ","НА","ВА","ИЕ","ЛЬ","ОВ","Я","РЕ","ТО","ЕС","ПО","ИА","Т","ЕТ","ТО","КА","ИИ","РЕ","ИЕ","РЕ","ПР","МО","НЯ","СТ","ИЕ","ЕЁ"};
			
			slbs.Add(st[Random.Range(0, st.Length)]);
		}

		//mix up syllables
		for (int i = 0; i < slbs.Count; i++)
		{
			string temp = slbs[i];
			int randomIndex = Random.Range(i, slbs.Count);
			slbs[i] = slbs[randomIndex];
			slbs[randomIndex] = temp;
		}

		//create a pool of syllables from an array of syllables
		foreach (string s in slbs)
		{
			Syllable TSyl = Instantiate(SylPrefab, SyllablesPool).GetComponent<Syllable>();
			TSyl.GetComponent<RectTransform>().anchoredPosition = new Vector2(CX, CY);
			TSyl.Init(s, this);
			currentSyllablesPool.Add(TSyl);
			CX += 54f;
			if (CX >= 226f)
			{
				CX = -209f;
				CY -= 54f;
			}
		}
	}

	private bool SlotsInited = false;
	void InitializeSlots()
	{

		currentSlotsDict.Clear();

		foreach (Transform child in SlotsContent.transform)
		{
			Destroy(child.gameObject);
		}

		float CX = -160f;
		float CY = 50f;
		foreach (string word in currentWords)
		{
			string TWord = word.ToUpper();
			currentSlotsDict.Add(TWord, new List<Slot>());
			CX = -165f;
			for (int i = 0; i < TWord.Length; i++)
			{
				GameObject TSlot = Instantiate(SlotPrefab, SlotsContent);
				RectTransform TSlotTr = TSlot.GetComponent<RectTransform>();
				TSlot.GetComponent<Slot>().content = TWord[i].ToString().ToUpper();
				TSlotTr.anchoredPosition = new Vector2(CX, CY);
				currentSlotsDict[TWord].Add(TSlot.GetComponent<Slot>());
				if (!currentOpenedSlots.ContainsKey(TWord))
				{
					currentOpenedSlots[TWord] = Random.Range(0, TWord.Length);
				}

				CX += 45f;
			}
			CY -= 45f;
		}
		SlotsInited = false;
	}

	void Start ()
	{
		Storage.Init();
		RemainingWordsCounter = Instantiate(RemainingWordsCounterPrefab, SyllablesInputField.transform).GetComponent<Text>();
		RemainingWordsCounter.rectTransform.anchoredPosition = new Vector2(4f, -50.4f);

		for (int i = 0; i < 3; i++)
		{
			currentWords.Add(Storage.words[i]);
		}
		for (int i = 3; i < Storage.words.Count; i++)
		{
			remainingWords.Add(Storage.words[i]);
		}

		SettingsManager.settings.SylMode = true;
		if (SettingsManager.settings.LoadedGame)
		{
			currentWords = SettingsManager.settings.GetCurrentWords();
			remainingWords = SettingsManager.settings.GetRemainingWords();
			LevelNumber = SettingsManager.settings.GetLevelNumber();
			SettingsManager.settings.LoadedGame = false;
			currentOpenedSlots = SettingsManager.settings.GetOpenedSlot();
			foreach (KeyValuePair<string, List<Slot>> word in currentSlotsDict)
			{
				word.Value[currentOpenedSlots[word.Key]].ShowContent();
			}
		}
		InitializeSyllables();
		InitializeSlots();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.S))
		{
			if (LevelNumber < 9)
			{
				int ln = LevelNumber + 1;
				SceneManager.LoadScene("Level_SylM" + ln, LoadSceneMode.Single);
			}
		}

		if (!SlotsInited)
		{
			foreach (KeyValuePair<string, List<Slot>> word in currentSlotsDict)
			{
				word.Value[currentOpenedSlots[word.Key]].ShowContent();
			}
			SlotsInited = true;
		}

		int t = remainingWords.Count + currentWords.Count;

		RemainingWordsCounter.text = t + "/10";

	}

	public void ClearInput(bool destroyLetters = false)
	{
		CurrentInputWord = "";
		CurrentInputPos = -165f;
		foreach (Syllable slb in CurrentSyllablesInInput)
		{
			if (destroyLetters)
			{
				Destroy(slb.gameObject);
			}
			else
			{
				slb.ReturnToPool();
			}
		}
		CurrentSyllablesInInput.Clear();
	}

	public void CompareInput()
	{
		for (int i = 0; i < currentWords.Count; i++)
		{
			if (currentWords[i] == CurrentInputWord)
			{
				Instantiate(TickPrefab, Storage.transform);
				currentWords.RemoveAt(i);
				if (remainingWords.Count > 0)
				{
					currentWords.Add(remainingWords[0]);
					remainingWords.RemoveAt(0);
				}
				foreach (Slot s in currentSlotsDict[CurrentInputWord])
				{
					s.ShowContent();
				}
				IsPlayable = false;
				StartCoroutine(ChangeWords());
				break;
			}
		}
	}

	IEnumerator ChangeWords()
	{
		yield return new WaitForSeconds(0.6f);
		ClearInput(true);
		InitializeSlots();
		InitializeSyllables();
		FinishLevel();
		IsPlayable = true;
	}

	public void FinishLevel()
	{
		if (currentWords.Count <= 0)
		{
			FinishLevelPopup.SetActive(true);
		}
		else
		{
			SettingsManager.settings.Save();
		}
	}
}
