using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWorldManager : UIManager
{
    [Header("Managers")]
    [SerializeField]
    private WorldManager WM;
    private ControlsManager CM;

    [Header("UI")]
    [SerializeField]
    private GameObject InGamePlayerUI;

    [Header("Controllers (pads)")]
    [SerializeField]
    private GameObject RightController;
    [SerializeField]
    private GameObject LeftController;
    [SerializeField]
    private GameObject LaserPointer;
    [SerializeField]
    private GameObject LaserBeam;

    [Header("Main Menu")]
    [SerializeField]
    private string mainMenuSceneName;

    [Header("SaveButton Anim")]
    [SerializeField]
    private GameObject saveButton;

    private void OnEnable()
    {
        CM = WM.GetComponent<ControlsManager>();
        OVRInput.Update();
    }

    private void OnDisable()
    {
        OVRInput.Update();
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch))
        {
            showPanel("right");
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch))
        {
            showPanel("left");
        }

        CM.SetCanvasSliding(isShowPanelCoroutineActive);
    }

    // PUBLIC METHODS
    public void showInGamePlayerUI(bool state)
    {
        InGamePlayerUI.SetActive(state);
    }

    public void ShowControllers(bool state)
    {
        RightController.SetActive(state);
        LeftController.SetActive(state);
        LaserPointer.SetActive(state);
        LaserBeam.SetActive(state);
    }

    public void SetCurrrentAndNextPanel(GameObject thisPanel)
    {
        currentPanel = thisPanel;
        nextPanel = thisPanel;
    }

    public void SaveGame()
    {
        // button animation
        StartCoroutine(SaveAnimCoroutine());

        // set save type as 2 (player save)
        GM.SetSaveType(2);

        // dimensions and colors modes are saved when changed
        // movement mode is saved in MovementManager

        // set player position and rotation
        GM.SetPlayerPosition(WM.GetPlayerPosition());
        GM.SetPlayerRotation(WM.GetPlayerRotation());

        GM.SaveFile("map");
        GM.SaveFile("save");
    }

    private IEnumerator SaveAnimCoroutine()
    {
        saveButton.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        saveButton.GetComponent<Animator>().SetBool("saveAnim", true);

        yield return new WaitForSecondsRealtime(1f);

        saveButton.GetComponent<Animator>().SetBool("saveAnim", false);
    }

    public void GoToMainMenu()
    {
        AM.PauseModeMusicCrossFade(false);
        GM.SetAudioMode("Beeps");
        StartCoroutine(ChangeSceneCoroutine(mainMenuSceneName));
    }
}
