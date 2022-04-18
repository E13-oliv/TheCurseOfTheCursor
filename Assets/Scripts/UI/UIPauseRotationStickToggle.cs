using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseRotationStickToggle : MonoBehaviour
{
    private Toggle toggle;

    private GameManager GM;

    [Header("Toggle Value")]
    [SerializeField]
    private bool toggleValue;

    private bool rotationStickState = false;

    private void OnEnable()
    {
        // fing the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        rotationStickState = GM.GetRotationStick();

        toggle = GetComponent<Toggle>();

        if (rotationStickState == toggleValue)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged();
        });
    }

    private void ToggleValueChanged()
    {
        if (toggle.isOn == true)
        {
            GM.SetRotationStick(toggleValue);
        }
    }

}
