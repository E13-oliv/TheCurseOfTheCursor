using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPauseDimensionsToggle : MonoBehaviour
{
    private Toggle toggle;

    [Header("World Manager Script")]
    [SerializeField]
    private WorldManager WM;
    private GameManager GM;

    [Header("Toggle Value")]
    [SerializeField]
    private string toggleValue;

    private string activeDimensionMode = "";

    private void OnEnable()
    {
        // fing the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        activeDimensionMode = GM.GetDimensionsMode();

        toggle = GetComponent<Toggle>();

        if (activeDimensionMode == toggleValue)
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
            WM.SetDimensionsMode(toggleValue);
        }
    }

}
