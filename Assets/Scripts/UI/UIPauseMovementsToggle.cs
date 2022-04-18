using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMovementsToggle : MonoBehaviour
{
    private Toggle toggle;

    [Header("Movement Manager Script")]
    [SerializeField]
    private MovementsManager MM;
    private GameManager GM;

    [Header("Toggle Value")]
    [SerializeField]
    private string toggleValue;

    private string activeMovementsMode = "";

    private void OnEnable()
    {
        // fing the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        activeMovementsMode = GM.GetMovementsMode();

        toggle = GetComponent<Toggle>();

        if (activeMovementsMode == toggleValue)
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
            MM.SetMovementMode(toggleValue);
        }
    }

}
