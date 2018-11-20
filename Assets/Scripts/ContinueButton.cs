using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour {

    public Text hint;//UI element, where hint will be shown
    public string hintText;//the text of hint itself

    public void OnHover()
    {
        hint.text = hintText;
    }

    public void OnClick()
    {
        SettingsManager.settings.Load();
    }
}
