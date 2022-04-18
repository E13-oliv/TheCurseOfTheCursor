using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPauseAudioToggle : MonoBehaviour
{
    private Toggle toggle;

    [Header("World Manager Script")]
    private GameManager GM;

    [Header("Toggle Value")]
    [SerializeField]
    private string toggleValue;

    private string activeAudioMode = "";

    private void OnEnable()
    {
        // fing the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        activeAudioMode = GM.GetAudioMode();

        toggle = GetComponent<Toggle>();

        if (activeAudioMode == toggleValue)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        toggle.group.GetComponent<ToggleGroup>().allowSwitchOff = false;

        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged();
        });
    }

    private void ToggleValueChanged()
    {
        if (toggle.isOn == true)
        {
            GM.SetAudioMode(toggleValue);
        }
    }

}
