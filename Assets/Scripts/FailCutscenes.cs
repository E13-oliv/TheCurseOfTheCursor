using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailCutscenes : CutscenesManager
{
    [Header("Player")]
    [SerializeField]
    private bool stopPlayer;

    private float defaultPlayerSpeed = 2.0f;

    [Header("Object")]
    [SerializeField]
    private bool removeObjectAfterCutscene;

    [Header("CutScene Type")]
    [SerializeField]
    private cutsceneTypes cutsceneType;

    [Header("Text Dialogues")]
    [SerializeField]
    private string[] textDialogues;

    [Header("Audio Dialogues")]
    [SerializeField]
    private AudioClip[] audioDialogues8bit;
    [SerializeField]
    private AudioClip[] audioDialoguesHD;

    private int activeDialogue = 0;
    private Coroutine textDisplayingCoroutine;
    private Coroutine dialoguePlayingCoroutine;
    private bool dialogueDisplayingOrPlaying = false;

    private AudioSource voicesAudioSource;
    private string audioMode;

    [Header("Animation")]
    [SerializeField]
    private Animator cutsceneAnimator;
    [SerializeField]
    private AnimationClip cutsceneAnimationClip;

    private float animationDuration;
    private Coroutine animationPlayingCoroutine;

    private GameObject flat2DObject;
    private string dimensionMode;

    private bool playerIsIn;
    private string action;

    private bool modeChangeAfterCutscene;
    private bool questPhaseChangeAfterCutscene;
    private bool displayFirstHelp;

    private GameObject actionObject;

    private void Update()
    {
        audioMode = WM.GetAudioMode();
        dimensionMode = WM.GetDimensionsMode();

        playerIsIn = this.GetComponent<InteractiveItems>().GetPlayerIsIn();
        action = this.GetComponent<InteractiveItems>().GetAction();
        displayFirstHelp = this.GetComponent<InteractiveItems>().GetDisplayFirstHelp();

        //if ((cutsceneButton == true && playerIsIn == true))
        if ((cutsceneButton == true && playerIsIn == true) || (cutsceneButton == true && action == "Help"))
        {
            if (cutsceneType == cutsceneTypes.Dialogues)
            {
                if (dialogueDisplayingOrPlaying == true)
                {
                    if (audioMode == "Beeps")
                    {
                        DisplayAllTextDialogue();
                    }
                    else
                    {
                        SkipDialogue();
                    }
                }
                else
                {
                    ContinueCutScene();
                }
            }
            else if (cutsceneType == cutsceneTypes.Animation)
            {
                // skip animation
                // not implemented yet
            }

            cutsceneButton = false;
        }
    }

    public void StartCutscene(GameObject cutSceneObject)
    {
        StartCoroutine(StartCutSceneDelayCoroutine(cutSceneObject));
    }

    private IEnumerator StartCutSceneDelayCoroutine(GameObject cutSceneObject)
    {
        yield return new WaitForSecondsRealtime(.2f);
        RealStartCutscene(cutSceneObject);
    }

    public void RealStartCutscene(GameObject cutSceneObject)
    {
        actionObject = cutSceneObject;

        CM.SetCutsceneActive(true);

        if (stopPlayer == true)
        {
            CM.SetPlayerSpeed(0);
            CM.SetPlayerRotation(false);
        }

        if (cutsceneType == cutsceneTypes.Dialogues)
        {
            if (audioMode == "Beeps")
            {
                UIIG.ShowHideTextDialogues(true);
                dialoguesBoxIsDisplayed = true;
            }
            else
            {
                voicesAudioSource = WM.GetVoicesAudioSource();
            }
        }
        else if (cutsceneType == cutsceneTypes.Animation)
        {
            // be ready for animation
            // nothing to do yet to be ready
        }

        ContinueCutScene();

        cutsceneButton = false;
    }

    private void ContinueCutScene()
    {
        if (cutsceneType == cutsceneTypes.Dialogues)
        {
            if (activeDialogue == textDialogues.Length)
            {
                EndCutscene();
            }
            else
            {
                GM.AddToLogText(textDialogues[activeDialogue] + "\n");

                if (audioMode == "Beeps")
                {
                    textDisplayingCoroutine = StartCoroutine(TextDialoguesCoroutine());
                }
                else
                {
                    dialoguePlayingCoroutine = StartCoroutine(VoiceDialogueCoroutine());
                }
            }
        }
        else if (cutsceneType == cutsceneTypes.Animation)
        {
            animationDuration = cutsceneAnimationClip.length;

            animationPlayingCoroutine = StartCoroutine(AnimationCoroutine());
        }

        SetCutsceneButton(false);
    }

    private void EndCutscene()
    {
        if (cutsceneType == cutsceneTypes.Dialogues)
        {
            UIIG.ShowHideTextDialogues(false);
            dialoguesBoxIsDisplayed = false;
        }
        else if (cutsceneType == cutsceneTypes.Animation)
        {
            cutsceneAnimator.SetBool("startAnimation", false);
        }

        if (action == "Help")
        {
            IIM.SetHelpAvailable(false);
            IIM.SetHelpDisplayed(false);
            cutsceneButton = false;
        }
        else
        {
            IIM.SetHelpDisplayed(true);
        }

        if (displayFirstHelp == true)
        {
            UIIG.displayFirstHelp();
        }

        if (modeChangeAfterCutscene == true)
        {
            IIM.ModeChange(actionObject);
        }

        if (questPhaseChangeAfterCutscene == true)
        {
            IIM.QuestPhaseChange(actionObject, true);
        }

        if (removeObjectAfterCutscene == true)
        {
            IIM.SetHelpDisplayed(true);
            this.gameObject.SetActive(false);
        }

        activeDialogue = 0;

        CM.SetCutsceneActive(false);
        CM.SetPlayerSpeed(defaultPlayerSpeed);
        CM.SetPlayerRotation(true);
    }

    private void DisplayAllTextDialogue()
    {
        StopCoroutine(textDisplayingCoroutine);
        UIIG.SetDialogueText(textDialogues[activeDialogue].ToUpper());

        activeDialogue++;
        dialogueDisplayingOrPlaying = false;

        UIIG.ShowHideContinue(true);

        SetCutsceneButton(false);
    }

    private void SkipDialogue()
    {
        voicesAudioSource.Stop();

        StopCoroutine(dialoguePlayingCoroutine);

        activeDialogue++;
        dialogueDisplayingOrPlaying = false;

        UIIG.ShowHideContinue(true);

        SetCutsceneButton(false);

        ContinueCutScene();
    }

    private IEnumerator TextDialoguesCoroutine()
    {
        UIIG.ShowHideContinue(false);

        dialogueDisplayingOrPlaying = true;

        string dialogueText = textDialogues[activeDialogue];
        int dialogueTextLength = dialogueText.Length;

        for (int i = 0; i < dialogueTextLength; i++)
        {
            UIIG.SetDialogueText(dialogueText.Substring(0,i + 1).ToUpper());
            PlaySound();
            yield return new WaitForSecondsRealtime(0.08f);
        }

        activeDialogue++;
        dialogueDisplayingOrPlaying = false;

        UIIG.ShowHideContinue(true);
    }

    private IEnumerator VoiceDialogueCoroutine()
    {
        dialogueDisplayingOrPlaying = true;

        AudioClip voiceDialogue;
        float voiceDialogueDuration;

        if (audioMode == "Bit8")
        {
            voiceDialogue = audioDialogues8bit[activeDialogue];
            voiceDialogueDuration = voiceDialogue.length;
        }
        // if HD
        else
        {
            voiceDialogue = audioDialoguesHD[activeDialogue];
            voiceDialogueDuration = voiceDialogue.length;
        }

        voicesAudioSource.clip = voiceDialogue;
        voicesAudioSource.Play();

        yield return new WaitForSecondsRealtime(voiceDialogueDuration * 2);

        activeDialogue++;
        dialogueDisplayingOrPlaying = false;

        yield return new WaitForSecondsRealtime(.2f);
        ContinueCutScene();
    }

    private IEnumerator AnimationCoroutine()
    {
        if (dimensionMode == "Flat2DCube")
        {
            Animator flatAnimator = flat2DObject.GetComponent<Animator>();
            flatAnimator.SetBool("startAnimation", true);
        }
        else
        {
            cutsceneAnimator.SetBool("startAnimation", true);
        }

        yield return new WaitForSecondsRealtime(animationDuration);

        EndCutscene();
    }

    private void PlaySound()
    {
        AudioSource audioSource = UIIG.GetTickSource();
        audioSource.Play();
    }

    public void SetActiveTextDialogue(int id)
    {
        activeDialogue = id;
    }

    public string[] GetTextDialogues()
    {
        return textDialogues;
    }

    public void SetNewDialogues(string[] newTextDialogues)
    {
        textDialogues = newTextDialogues;
    }

    public void SetModeChangeAfterCutscene(bool state)
    {
        modeChangeAfterCutscene = state;
    }

    public void SetQuestPhaseChangeAfterCutscene(bool state)
    {
        questPhaseChangeAfterCutscene = state;
    }

    public void SetFlat2DObject(GameObject flat2DObj)
    {
        flat2DObject = flat2DObj;
    }
}
