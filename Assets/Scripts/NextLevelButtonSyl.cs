using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButtonSyl : MonoBehaviour {
    private LevelManagerSyl l_m;
    // Use this for initialization
    void Start()
    {
        l_m = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerSyl>();
    }

    public void OnClick()
    {
        int ln = l_m.LevelNumber + 1;
        SceneManager.LoadScene("Level_SylM" + ln, LoadSceneMode.Single);
    }
}
