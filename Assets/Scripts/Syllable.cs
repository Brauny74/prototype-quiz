using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Syllable : MonoBehaviour {

    public Text textField;
    public string content;

    private bool IsInPool;
    private LevelManagerSyl l_m;
    private Vector2 InPoolPos;//Syllable can be in a pool or in input field
    

    public void Init(string S, LevelManagerSyl tl_m)
    {
        l_m = tl_m;
        InPoolPos = GetComponent<RectTransform>().anchoredPosition;
        IsInPool = true;
        textField = GetComponentInChildren<Text>();
        content = S;
        textField.text = content.ToString();
    }

    public void OnClick()
    {
        if (IsInPool && l_m.IsPlayable)
        {
            float CX = l_m.CurrentInputPos;
            GetComponent<RectTransform>().SetParent(l_m.SyllablesInputField);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(CX, 0.0f);
            l_m.CurrentInputPos += 54f;
            l_m.CurrentSyllablesInInput.Add(this);
            l_m.CurrentInputWord += content.ToString();

            l_m.CompareInput();

            IsInPool = false;
        }
    }

    public void ReturnToPool()
    {
        if (!IsInPool)
        {
            GetComponent<RectTransform>().SetParent(l_m.SyllablesPool);
            GetComponent<RectTransform>().anchoredPosition = InPoolPos;
            IsInPool = true;
        }
    }
}
