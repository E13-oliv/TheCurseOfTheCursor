using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIIntroManager : MonoBehaviour
{
    // MAMANGERS
    private AudioManager AM;

    [Header("Introduction Text")]
    [SerializeField]
    private GameObject introText;

    private string introContent = 
        "ONCE UPON A TIME IN A TEXT-BASED GAME,\n" +
        "A LONELY CURSOR WAS STUCKED ON THE SCREEN\n" +
        "WAITING FOR SOME INPUTS.\n\n" +
        "TIRED OF THIS SITUATION AND EAGER TO\n" +
        "DISCOVER NEW LANDS AND NEW ADVENTURES,\n\n" +
        "THE CURSOR DECIDES TO JUMP FROM THIS \n" +
        "SCREEN TO BECOME A NON-PLAYABLE CHARACTER\n" +
        "IN AN ADVENTURE GAME....\n ";
    private string lastChar;
    private string continueText = "\n\n[A] CONTINUE";

    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private GameObject titleContinueText;

    private Coroutine textDisplayCoroutine;
    private bool textIsDisplaying;
    private bool textIsDisplayed;

    private bool firstWait = true;

    private float delayBetweenCharacters;
    private float defaultDelayBetweenCharacters = .08f;
    private float delayBetweenParagraphs = 1.0f;

    private float delayBeforeContinue;
    private float defaultDelayBeforeContinue = 2.5f;

    private float delayBeforeTitle = 3.0f;

    [Header("Cursor")]
    [SerializeField]
    private GameObject cursor;

    [Header("Sound")]
    [SerializeField]
    private AudioSource tickSource;

    [Header("Scenes")]
    [SerializeField]
    private string gameSceneName;

    private Animator cursorAnimator;

    private void Start()
    {
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        AM = gameManagerGameObject.GetComponentInChildren<AudioManager>();

        delayBetweenCharacters = defaultDelayBetweenCharacters;
        delayBeforeContinue = defaultDelayBeforeContinue;

        textDisplayCoroutine = StartCoroutine(DisplayTextCoroutine());

        cursorAnimator = cursor.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (firstWait == false)
            {
                if (textIsDisplaying == true)
                {
                    DisplayAllText();
                    textIsDisplaying = false;
                    textIsDisplayed = true;
                }
                else if (textIsDisplayed == true)
                {
                    StartCoroutine(DisplayTitle());
                }
                else
                {
                    SceneManager.LoadScene(sceneName: gameSceneName);
                }
            }
        }
    }

    private void DisplayAllText()
    {
        StopCoroutine(textDisplayCoroutine);

        introText.GetComponent<Text>().text = introContent + continueText;
        cursor.SetActive(false);
    }

    private IEnumerator DisplayTitle()
    {
        AM.IntroMusicStart();
        introText.SetActive(false);
        textIsDisplayed = false;

        yield return new WaitForSecondsRealtime(delayBeforeTitle);

        titleText.SetActive(true);

        yield return new WaitForSecondsRealtime(delayBeforeTitle);

        titleContinueText.SetActive(true);
    }

    private IEnumerator DisplayTextCoroutine()
    {
        textIsDisplaying = true;

        yield return new WaitForSecondsRealtime(5.5f);

        firstWait = false;

        cursorAnimator.SetBool("blink", false);

        int charLength = 0;

        for (int i = 0; i < introContent.Length; i++)
        {
            string nextChar = introContent.Substring(i, 1);

            introText.GetComponent<Text>().text = introContent.Substring(0, i);
            yield return new WaitForSecondsRealtime(delayBetweenCharacters);

            UICharInfo[] lastCharInfo = introText.GetComponent<Text>().cachedTextGenerator.GetCharactersArray();
            cursor.transform.localPosition = new Vector3(lastCharInfo[i].cursorPos.x, lastCharInfo[i].cursorPos.y, 0);

            PlaySound();

            if (lastChar == "\n" && nextChar == "\n")
            {
                yield return new WaitForSecondsRealtime(delayBetweenParagraphs);
            }

            lastChar = nextChar;
            charLength++;
        }

        cursorAnimator.SetBool("blink", true);

        yield return new WaitForSecondsRealtime(delayBeforeContinue);

        cursorAnimator.SetBool("blink", false);

        for (int i = 0; i < continueText.Length; i++)
        {
            introText.GetComponent<Text>().text += continueText.Substring(i, 1);
            yield return new WaitForSecondsRealtime(delayBetweenCharacters);

            UICharInfo[] lastCharInfo = introText.GetComponent<Text>().cachedTextGenerator.GetCharactersArray();
            cursor.transform.localPosition = new Vector3(lastCharInfo[charLength].cursorPos.x, lastCharInfo[charLength].cursorPos.y, 0);

            PlaySound();

            charLength++;
        }

        delayBetweenCharacters = defaultDelayBetweenCharacters;
        delayBeforeContinue = defaultDelayBeforeContinue;

        cursorAnimator.SetBool("blink", true);
    }

    private void PlaySound()
    {
        tickSource.Play();
    }
}
