using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPauseColorsToggle : MonoBehaviour
{
    private Toggle toggle;

    [Header("World Manager Script")]
    [SerializeField]
    private WorldManager WM;
    private GameManager GM;

    [Header("Toggle Value")]
    [SerializeField]
    private string toggleValue;

    private string activeColorsMode;

    private void OnEnable()
    {
        // fing the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        activeColorsMode = GM.GetColorsMode();

        toggle = GetComponent<Toggle>();

        if (activeColorsMode == toggleValue)
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
            WM.SetColorsMode(toggleValue);
        }
    }
}
