using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMusicVolumeToggle : MonoBehaviour
{
    private Toggle toggle;

    private GameManager GM;

    [Header("Toggle Value")]
    [SerializeField]
    private int toggleValue;

    private int musicVolume;

    private void OnEnable()
    {
        // fing the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        musicVolume = GM.GetMusicVolume();

        toggle = GetComponent<Toggle>();

        if (musicVolume == toggleValue)
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
            GM.SetMusicVolume(toggleValue);
        }
    }

}
