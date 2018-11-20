using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SettingsManager : MonoBehaviour
{

	public static SettingsManager settings;

	public bool EasyMode;
	public bool SylMode;

	public bool LoadedGame = false;
	private GameData dataToLoad;

	void Awake()
	{
		dataToLoad = new GameData();

		if (settings == null)
		{
			DontDestroyOnLoad(gameObject);
			settings = this;
		}
		else
		{
			if (settings != this)
			{
				Destroy(gameObject);
			}
		}
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
		GameData data = new GameData();
		data.EasyMode = EasyMode;
		if (SylMode)
		{
			LevelManagerSyl l_m = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerSyl>();
			data.LevelName = "Level_SylM" + l_m.LevelNumber;
			data.currentWords = l_m.currentWords;
			data.remainingWords = l_m.remainingWords;
			data.currentOpenedSlots = l_m.currentOpenedSlots;
		}
		else
		{
			LevelManager l_m = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
			data.LevelName = "Level" + l_m.LevelNumber;
			data.currentWords = l_m.currentWords;
			data.remainingWords = l_m.remainingWords;
		}
		bf.Serialize(file, data);
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			GameData data = (GameData)bf.Deserialize(file);
			file.Close();
			dataToLoad.remainingWords = data.remainingWords;
			dataToLoad.currentWords = data.currentWords;
			dataToLoad.LevelName = data.LevelName;
			dataToLoad.currentOpenedSlots = data.currentOpenedSlots;
			EasyMode = data.EasyMode;
			LoadedGame = true;

			SceneManager.LoadScene(dataToLoad.LevelName, LoadSceneMode.Single);
		}
	}

	public List<string> GetCurrentWords()
	{
		return dataToLoad.currentWords;
	}

	public List<string> GetRemainingWords()
	{
		return dataToLoad.remainingWords;
	}

	public Dictionary<string, int> GetOpenedSlot()
	{
		return dataToLoad.currentOpenedSlots;
	}

	public int GetLevelNumber()
	{
		int res;
		string t;
		if (SylMode)
		{ 
			t = dataToLoad.LevelName.Replace("Level_SylM", "");
		}
		else
		{
			t = dataToLoad.LevelName.Replace("Level", "");
		}
		res = Convert.ToInt32(t);
		return res;
	}
}

[Serializable]
class GameData
{
	public string LevelName;
	public bool EasyMode;
	public List<string> currentWords;
	public List<string> remainingWords;
	public Dictionary<string, int> currentOpenedSlots;
}
