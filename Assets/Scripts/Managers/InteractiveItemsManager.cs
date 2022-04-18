using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveItemsManager : ItemsManager
{
    [Header("managers")]
    [SerializeField]
    private UIInGame UIIG;

    [Header("Items Pick Up")]
    [SerializeField]
    private GameObject pickUpItemImageIcon;

    [Header("Interactive Items")]
    [SerializeField]
    private GameObject[] interactivesItems;

    [Header("Landmark Objects")]
    [SerializeField]
    private GameObject[] landmarkObjects;

    //QUEST START AND CHANGE
    private int[] questsToStart = new int[0];
    private int[] questToChangePhase = new int[0];
    private bool questPhaseChangeAfterCutscene;

    // ACTIONS & ACTIONS BUTTONS
    private actions action;
    private GameObject actionObject;

    private actionButtons actionButton;
    private actionButtons buttonDown;

    // WORLD CHANGES
    private bool worldChange;
    private worldChangeTypes worldChangeType;
    private int landmarkObjectToChangeID;
    private int newLandmarkObjectID;

    // MODE CHANGE
    private bool modeChange;
    private bool modeChangeAfterCutscene;

    // GAME PHASE CHANGES
    private bool gamePhaseChange;
    private int newGamePhase;

    // HELP
    [Header("Help UI")]
    [SerializeField]
    private GameObject helpUI;
    private actionButtons helpButton = actionButtons.B;
    private bool helpIsAvailable = false;
    private bool helpisDisplayed = false;

    private GameObject helpObject;

    private colorsModes colorsMode;

    private void Start()
    {
        // enable all interactives items
        EnableAllItemsAndLandmarks();
    }

    private void Update()
    {
        // if mode change -> check item display
        colorsModes currentColorMode = (colorsModes)System.Enum.Parse(typeof(colorsModes), GM.GetColorsMode());

        if (colorsMode != currentColorMode)
        {
            colorsMode = currentColorMode;

            EnableAllItemsAndLandmarks();
        }

        // show or hide help when available regarding other interactions
        if (helpIsAvailable == true && helpisDisplayed == true)
        {
            helpUI.SetActive(true);
        }
        else
        {
            helpUI.SetActive(false);
        }

        // start help cutscene
        if (buttonDown == helpButton && helpIsAvailable)
        {
            StartCutscene(helpObject);

            buttonDown = actionButtons.None;
        }

        // start all other actions
        else if (actionButton == buttonDown && actionButton != actionButtons.None && helpisDisplayed == false)
        {
            // if end of the game
            if (action == actions.StartEndCutscene)
            {
                actionObject.GetComponent<EndCutscene>().StartEndCutscene();
            }
            else
            {
                if (action == actions.StartFailCutscene)
                {
                    actionObject.GetComponent<Cutscenes>().SetFailCutsceneActive(true);
                    StartFailCutscene(actionObject);
                }
                else
                {
                    if (actionObject.GetComponent<Cutscenes>())
                    {
                        actionObject.GetComponent<Cutscenes>().SetFailCutsceneActive(false);
                    }

                    if (questToChangePhase.Length > 0)
                    {
                        if (questPhaseChangeAfterCutscene == true)
                        {
                            actionObject.GetComponent<Cutscenes>().SetQuestPhaseChangeAfterCutscene(true);
                        }
                        else
                        {
                            QuestPhaseChange(actionObject, false);
                        }
                    }

                    if (modeChange == true)
                    {
                        if (modeChangeAfterCutscene == true)
                        {
                            actionObject.GetComponent<Cutscenes>().SetModeChangeAfterCutscene(true);
                        }
                        else
                        {
                            ModeChange(actionObject);
                        }
                    }

                    switch (action)
                    {
                        case actions.PickUp:
                            ItemPickUp();
                            break;
                        case actions.Use:
                            ItemUse();
                            break;
                        case actions.StartCutscene:
                            StartCutscene(actionObject);
                            break;
                        // case None
                        default:
                            break;
                    }

                    if (questsToStart.Length > 0)
                    {
                        for (int i = 0; i < questsToStart.Length; i++)
                        {
                            GM.SetQuestPhase(questsToStart[i], 1);
                        }
                    }

                    if (gamePhaseChange == true)
                    {
                        GM.SetGamePhase(newGamePhase);
                    }

                    if (worldChange == true)
                    {
                        switch (worldChangeType)
                        {
                            case worldChangeTypes.Show:
                                ShowHideLandmark(landmarkObjectToChangeID, true);
                                break;
                            case worldChangeTypes.Hide:
                                ShowHideLandmark(landmarkObjectToChangeID, false);
                                break;
                            case worldChangeTypes.Swap:
                                ShowHideLandmark(landmarkObjectToChangeID, false);
                                ShowHideLandmark(newLandmarkObjectID, true);
                                break;
                            // case None
                            default:
                                break;
                        }
                    }
                }
            }



            // reset action text and state
            actionButton = actionButtons.None;
            ResetAction("");
            ResetQuestStartAndPhaseChange();
            ResetWorldChange();
            ResetGamePhase();

            // new hide and show on all items & landmarks
            EnableAllItemsAndLandmarks();
        }
    }

    private void EnableAllItemsAndLandmarks()
    {
        // i = 1 beacause arrays[0] is empty
        for (int i = 1; i < interactivesItems.Length; i++)
        {
            if (interactivesItems[i].activeSelf == false)
            {
                interactivesItems[i].SetActive(true);
            }
            else
            {
                interactivesItems[i].GetComponent<InteractiveItems>().CheckDisplay();
            }
        }

        for (int i = 1; i < landmarkObjects.Length; i++)
        {
            if (landmarkObjects[i].activeSelf == false)
            {
                landmarkObjects[i].SetActive(true);
            }
        }
    }

    private void ItemPickUp()
    {
        // get item ID
        int itemInventoryID = actionObject.GetComponent<InteractiveItems>().GetItemInventoryID();

        StartCoroutine(ItemPickUpIconCoroutine());

        // add item to inventory
        GM.UpdateInventory(itemInventoryID, 1);
        GM.UpdateInventoryDiscovered(itemInventoryID, 1);
    }

    private IEnumerator ItemPickUpIconCoroutine()
    {
        AudioSource pickUpAudioSource = WM.GetPickUpAUdioSource();

        pickUpAudioSource.Play();

        Sprite itemIcon = actionObject.GetComponent<InteractiveItems>().GetItemIconSprite();

        Debug.Log(itemIcon);

        pickUpItemImageIcon.SetActive(true);
        pickUpItemImageIcon.transform.GetChild(0).GetComponent<Image>().sprite = itemIcon;

        yield return new WaitForSecondsRealtime(2.0f);

        pickUpItemImageIcon.transform.GetChild(0).GetComponent<Image>().sprite = null;
        pickUpItemImageIcon.SetActive(false);
    }

    private void ItemUse()
    {
        // get item ID
        int itemToUseID = actionObject.GetComponent<InteractiveItems>().GetItemToUseID();

        bool removeFromInventory = actionObject.GetComponent<InteractiveItems>().GetRemoveFromInventory();

        if (removeFromInventory == true)
        {
            // remove item to inventory
            GM.UpdateInventory(itemToUseID, 0);
        }
    }

    public void StartCutscene(GameObject cutSceneObject)
    {
        cutSceneObject.GetComponent<Cutscenes>().StartCutscene(cutSceneObject);
    }

    public void StartFailCutscene(GameObject cutSceneObject)
    {
        cutSceneObject.GetComponent<FailCutscenes>().StartCutscene(cutSceneObject);
    }

    private void ShowHideLandmark(int toSHowID, bool state)
    {
        landmarkObjects[toSHowID].SetActive(state);
    }

    // PUBLIC GENERAL METHODS
    public void SetAction(string actionName, string buttonName, GameObject actionItem)
    {
        if (actionName == "Help")
        {
            helpObject = actionItem;
            helpIsAvailable = true;
        }
        else
        {
            action = (actions)System.Enum.Parse(typeof(actions), actionName);
            actionButton = (actionButtons)System.Enum.Parse(typeof(actionButtons), buttonName);
            actionObject = actionItem;
        }
    }

    public void DisplayAction(string actionText)
    {
        UIIG.DisplayAction(actionText);
    }

    public void ResetAction(string actionName)
    {
        if (actionName != "Help")
        {
            UIIG.HideAction();
            SetAction("None", "None", null);
            SetModeChange(false, false);
            buttonDown = actionButtons.None;
        }
    }

    public void SetWorldChange(bool worldChangeState, string changeType, int ojbectID, int newObjectID)
    {
        worldChange = worldChangeState;
        worldChangeType = (worldChangeTypes)System.Enum.Parse(typeof(worldChangeTypes), changeType);
        landmarkObjectToChangeID = ojbectID;
        newLandmarkObjectID = newObjectID;
    }

    public void ResetWorldChange()
    {
        SetWorldChange(false, "None", 0, 0);
    }

    public void QuestPhaseChange(GameObject actionObject, bool fromCutScene)
    {
        int[] questToChangePhase = actionObject.GetComponent<InteractiveItems>().GetQuestIDToChangePhase();
        int[] questNewPhase = actionObject.GetComponent<InteractiveItems>().GetNewQuestPhase();

        for (int i = 0; i < questToChangePhase.Length; i++)
        {
            GM.SetQuestPhase(questToChangePhase[i], questNewPhase[i]);
        }

        if (fromCutScene == true)
        {
            EnableAllItemsAndLandmarks();
        }
    }

    public void SetQuestPhaseChange(int[] quest, bool afterCutscene)
    {
        questToChangePhase = quest;
        questPhaseChangeAfterCutscene = afterCutscene;
    }

    public void ResetQuestStartAndPhaseChange()
    {
        questsToStart = new int[0];
        questPhaseChangeAfterCutscene = false;
        questToChangePhase = new int[0];
    }

    public void ModeChange(GameObject actionObj)
    {
        // movement mode
        if (actionObj.GetComponent<InteractiveItems>().GetMovementModeChange() == true)
        {
            string newMovementMode = actionObj.GetComponent<InteractiveItems>().GetNewMovementMode();
            MM.SetMovementMode(newMovementMode);
            GM.SetMovementsMode(newMovementMode);
        }

        // dimension mode
        if (actionObj.GetComponent<InteractiveItems>().GetDimensionModeChange() == true)
        {
            string newDimensionMode = actionObj.GetComponent<InteractiveItems>().GetNewDimensionMode();
            WM.SetDimensionsMode(newDimensionMode);
            GM.SetDimensionssMode(newDimensionMode);
        }

        // colors mode
        if (actionObj.GetComponent<InteractiveItems>().GetColorsModeChange() == true)
        {
            string newColorsMode = actionObj.GetComponent<InteractiveItems>().GetNewColorsMode();
            WM.SetColorsMode(newColorsMode);
            GM.SetColorssMode(newColorsMode);
        }

        // movement mode
        if (actionObj.GetComponent<InteractiveItems>().GetAudioModeChange() == true)
        {
            string newAudioMode = actionObj.GetComponent<InteractiveItems>().GetNewAudioMode();
            AM.SetAudioMode(newAudioMode);
            GM.SetAudioMode(newAudioMode);
        }
    }

    public void SetModeChange(bool state, bool afterCutscene)
    {
        modeChange = state;
        modeChangeAfterCutscene = afterCutscene;
    }

    public void ResetModeChange()
    {
        SetModeChange(false, false);
    }

    public void SetGamePhase(bool phase, int newPhase)
    {
        gamePhaseChange = phase;
        newGamePhase = newPhase;
    }

    public void ResetGamePhase()
    {
        gamePhaseChange = false;
        newGamePhase = 0;
    }

    public void SetQuestsToStart(int[] quests)
    {
        questsToStart = quests;
    }

    public void SetButtonDown(string buttonName)
    {
        buttonDown = (actionButtons)System.Enum.Parse(typeof(actionButtons), buttonName);
    }

    public void SetHelpAvailable(bool state)
    {
        helpIsAvailable = state;
    }

    public void SetHelpDisplayed(bool state)
    {
        helpisDisplayed = state;
    }

    public GameObject GetHelpUI()
    {
        return helpUI;
    }
}
