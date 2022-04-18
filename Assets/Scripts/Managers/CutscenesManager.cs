using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenesManager : MonoBehaviour
{
    // MANAGERS
    protected GameManager GM;
    protected AudioManager AM;
    protected UIInGame UIIG;
    protected ControlsManager CM;
    protected WorldManager WM;
    protected InteractiveItemsManager IIM;

    protected static bool cutsceneButton;
    protected static string endCutsceneChoiceButton = "None";
    protected static bool creditsButton;

    protected static bool dialoguesBoxIsDisplayed;

    protected enum cutsceneTypes
    {
        None = 0,
        Dialogues = 1,
        Animation = 2
    }

    private void Start()
    {
        GameObject GMGameObject = GameObject.Find("GameManager");
        GM = GMGameObject.GetComponent<GameManager>();
        AM = GMGameObject.GetComponent<AudioManager>();

        GameObject UIGGGameObject = GameObject.Find("InGameUICanvas");
        UIIG = UIGGGameObject.GetComponent<UIInGame>();

        GameObject WMGameObject = GameObject.Find("WorldManager");
        WM = WMGameObject.GetComponent<WorldManager>();
        CM = WMGameObject.GetComponent<ControlsManager>();
        IIM = WMGameObject.GetComponent<InteractiveItemsManager>();
    }

    public void SetCutsceneButton(bool state)
    {
        cutsceneButton = state;
    }

    public void SetEndCutsceneChoiceButton(string buttonType)
    {
        endCutsceneChoiceButton = buttonType;
    }

    public void SetCreditsButton(bool state)
    {
        creditsButton = state;
    }

    public bool GetDialoguesBoxIsDisplayed()
    {
        return dialoguesBoxIsDisplayed;
    }
}
