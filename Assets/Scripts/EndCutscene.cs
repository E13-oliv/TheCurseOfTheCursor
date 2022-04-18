using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCutscene : CutscenesManager
{
    [Header("Managers")]
    [SerializeField]
    private UIWorldManager UIWM;

    [Header("Contract")]
    [SerializeField]
    private GameObject contractPanel;

    [Header("Credits")]
    [SerializeField]
    private GameObject creditsMode;
    [SerializeField]
    private GameObject creditsText;
    [SerializeField]
    private GameObject pauseMode;
    [SerializeField]
    private GameObject skipText;

    private float creditsHeight = 3400;
    private float creditsBaseY = 375;
    private float creditsFInalY = 3025;

    [Header("Player")]
    [SerializeField]
    private GameObject playerTrackingSpaceGO;
    [SerializeField]
    private GameObject playerView;

    [Header("Dialogue")]
    [SerializeField]
    private AudioClip lastDialogue;

    private AudioSource voicesAudioSource;

    private void Update()
    {
        if (endCutsceneChoiceButton != "None")
        {
            voicesAudioSource = WM.GetVoicesAudioSource();

            CM.SetEndCutsceneActive(false);

            StartCoroutine(CreditsLaunchCoroutine());

            endCutsceneChoiceButton = "None";
        }

        if (GM.GetCanSkipCredits() == true && creditsButton == true)
        {
            StartCoroutine(PostCreditsLaunchCoroutine());
        }
    }

    public void StartEndCutscene()
    {
        contractPanel.SetActive(true);

        StartCoroutine(ActionsDelayCoroutine());
    }

    private IEnumerator ActionsDelayCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        CM.SetEndCutsceneActive(true);
    }

    private IEnumerator CreditsLaunchCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        float voiceDialogueDuration = lastDialogue.length;
        voicesAudioSource.clip = lastDialogue;
        voicesAudioSource.Play();

        yield return new WaitForSecondsRealtime(voiceDialogueDuration + .2f);

        GM.SetPostCredits(true);
        GM.NewSaveLevel6();

        playerView.GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 1);

        contractPanel.SetActive(false);
        creditsMode.SetActive(true);

        UIWM.ShowControllers(false);

        Vector3 creditsPlayerLocation = new Vector3(0, -4000, 0);
        Vector3 creditsPlayerRotation = new Vector3(0, 0, 0);

        playerTrackingSpaceGO.transform.position = creditsPlayerLocation;
        playerTrackingSpaceGO.transform.eulerAngles = creditsPlayerRotation;

        StartCoroutine(CreditsRollCoroutine());

        yield return new WaitForSecondsRealtime(3.0f);

        skipText.SetActive(true);
        GM.SetCanSkipCredits(true);
    }

    private IEnumerator CreditsRollCoroutine()
    {
        float currentCreditsPosY = creditsText.GetComponent<RectTransform>().localPosition.y;

        float rollSpeed = 1.6f;
        float newY;

        yield return new WaitForSecondsRealtime(2.5f);

        for (int i = 0; currentCreditsPosY < creditsFInalY; i++)
        {
            newY = currentCreditsPosY + rollSpeed;

            creditsText.GetComponent<RectTransform>().localPosition = new Vector3(0, newY, 0);

            yield return new WaitForSecondsRealtime(0.01f);

            currentCreditsPosY = newY;
        }

        yield return new WaitForSecondsRealtime(1.5f);

        StartCoroutine(PostCreditsLaunchCoroutine());
    }

    private IEnumerator PostCreditsLaunchCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        SceneManager.LoadScene("WorldScene");
    }
}
