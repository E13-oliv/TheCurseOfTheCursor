using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseVoicesVolumeToggle : MonoBehaviour
{
    private Toggle toggle;

    private GameManager GM;

    [Header("Toggle Value")]
    [SerializeField]
    private int toggleValue;

    private int voicesVolume;

    private void OnEnable()
    {
        // fing the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        voicesVolume = GM.GetVoicesVolume();

        toggle = GetComponent<Toggle>();

        if (voicesVolume == toggleValue)
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
            GM.SetVoicesVolume(toggleValue);
        }
    }

}
