using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    // Managers
    private GameManager GM;

    [Header("Modes")]
    [SerializeField]
    private Text debugText;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips;

    [Header("Grid 2D")]
    [SerializeField]
    private GameObject Grid2DUI;
    [SerializeField]
    private Image[] arrows;
    [SerializeField]
    private GameObject[] northLetter;

    [Header("UI Actions")]
    [SerializeField]
    private GameObject action;
    [SerializeField]
    private GameObject actionShadow;
    [SerializeField]
    private GameObject help;

    [Header("Dialogues")]
    [SerializeField]
    private GameObject textDialogues;
    [SerializeField]
    private Text textDialoguesText;
    [SerializeField]
    private GameObject continueInput;

    private void OnEnable()
    {
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();
    }

    private void Update()
    {
        string audioMode = GM.GetAudioMode();

        switch (audioMode)
        {
            case "Bit8":
                audioSource.clip = audioClips[1];
                break;
            case "HD":
                audioSource.clip = audioClips[2];
                break;
            // case "Beeps"
            default:
                audioSource.clip = audioClips[0];
                break;
        }
    }

    // ALL MODES PUBLIC METHODS
    public void SetDebugText(string text)
    {
        debugText.text = text;
    }

    public void ShowHideAllModeUI(bool visible)
    {
        Grid2DUI.SetActive(visible);
    }

    public void ShowHideGrid2DUI(bool visible)
    {
        Grid2DUI.SetActive(visible);
    }

    // GRID 2D MOVEMENTS MODE PUBLIC METHODS
    public void ShowArrows(bool[] casesAccessible, int cardinalPoint)
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            int j = i + cardinalPoint;

            if (j >= 4)
            {
                j -= 4;
            }

            if (casesAccessible[j] == true)
            {
                arrows[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                arrows[i].GetComponent<Image>().color = new Color(1, 1, 1, .33f);
            }
        }

        SetNorthPosition(cardinalPoint);
    }
    public void HideArrows(int cardinalPoint)
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].GetComponent<Image>().color = new Color(1, 1, 1, .33f);
        }

        SetNorthPosition(cardinalPoint);
    }

    private void SetNorthPosition(int cardinalPoint)
    {
        for (int i = 0; i < northLetter.Length; i++)
        {
            if (cardinalPoint == i)
            {
                northLetter[i].SetActive(true);
            }
            else
            {
                northLetter[i].SetActive(false);
            }
        }
    }

    public void DisplayAction(string actionText)
    {
        action.GetComponent<Text>().text = actionText;
        actionShadow.GetComponent<Text>().text = actionText;
        action.SetActive(true);
        actionShadow.SetActive(true);
    }

    public void HideAction()
    {
        if (action != null)
        {
            action.GetComponent<Text>().text = "";
            action.SetActive(false);
        }
        if (actionShadow != null)
        {
            actionShadow.GetComponent<Text>().text = "";
            actionShadow.SetActive(false);
        }
    }

    public void ShowHideTextDialogues(bool state)
    {
        textDialogues.SetActive(state);
    }

    public void ShowHideContinue(bool state)
    {
        continueInput.SetActive(state);
    }
    public void ShowHideHelp(bool state)
    {
        help.SetActive(state);
    }

    public void SetDialogueText(string dialogue)
    {
        textDialoguesText.text = dialogue;
    }

    private IEnumerator displayFirstHelpCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        ShowHideHelp(true);
    }

    public void displayFirstHelp()
    {
        StartCoroutine(displayFirstHelpCoroutine());
    }

    public AudioSource GetTickSource()
    {
        return audioSource;
    }

}
