using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITitleManager : UIManager
{
    // save check
    private int saveType;

    [Header("Panels")]
    [SerializeField]
    private GameObject splashPanel;
    [SerializeField]
    private GameObject titlePanel;

    [Header("Scenes")]
    [SerializeField]
    private string introSceneName;
    [SerializeField]
    private string worldSceneName;

    [Header("Intro Timing [s]")]
    [SerializeField]
    private float splashScreenDuration = 3.0f;

    [Header("Title Panel")]
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private GameObject newGameWarningPanel;

    [Header("EnterCode Panel")]
    [SerializeField]
    private Text codeText;

    [SerializeField]
    private Button codeOKButton;

    private string whereFromItComes;

    private void Start()
    {
        GM.LoadFile("save");
        saveType = GM.GetSaveType();

        if (saveType == 2)
        {
            continueButton.interactable = true;
        }

        // set active panel
        currentPanel = titlePanel;

        // splash screen start
        StartCoroutine(SplashScreenCoroutine(splashPanel, titlePanel, splashScreenDuration));

        // remove pause when coming from game
        GM.RemovePause();
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

        OVRInput.Update();

        if (codeText.text == "1337")
        {
            codeOKButton.interactable = true;
        }
        else
        {
            codeOKButton.interactable = false;
        }
    }

    // PUBLIC METHODS
    public void panelComesFrom(string whereFrom)
    {
        whereFromItComes = whereFrom;
    }

    public void EnterGame(bool onTitlePanel)
    {
        if (saveType == 2 && onTitlePanel == true)
        {
            ShowWarning(newGameWarningPanel);
        }
        else
        {
            StartCoroutine(ChangeSceneCoroutine(introSceneName));
        }
    }

    public void StartNewGame()
    {
        GM.SetContinueGame(false);
        GM.SetNewGameWithCode(false);

        GM.NewSave();

        AM.PlayStopAllMusicSource(false);

        EnterGame(true);
    }

    public void ContinueGame()
    {
        GM.SetContinueGame(true);

        StartCoroutine(ChangeSceneCoroutine(worldSceneName));
    }

    public void EnterGameWithCode()
    {
        GM.SetNewGameWithCode(true);

        StartCoroutine(ChangeSceneCoroutine(worldSceneName));
    }

    public void EnterCode(string value)
    {
        if (codeText.text.Length < 4)
        {
            codeText.text += value;
        }
    }

    public void DeleteCode()
    {
        if (codeText.text.Length > 0)
        {
            codeText.text = codeText.text.Substring(0, codeText.text.Length - 1);
        }
    }
}
