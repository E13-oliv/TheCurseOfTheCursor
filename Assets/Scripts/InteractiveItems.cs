using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItems : ItemsManager
{
    [Header("Dev")]
    [SerializeField]
    private string comment;
    [SerializeField]
    private bool onDbug;

    [Header("Item")]
    [SerializeField]
    private itemTypes itemType;
    [SerializeField]
    private int landmarkID;

    [Header("Inventory")]
    [SerializeField]
    private int itemInventoryID;

    [Header("Pick Up")]
    [SerializeField]
    private Sprite itemSprite;

    [Header("Landmarks")]
    [SerializeField]
    private gamePhases landmarkPopIn;
    [SerializeField]
    private gamePhases landmarkPopOut;

    [Header("Need mode to be displayed")]
    [SerializeField]
    private colorsModes[] colorModesToBeDisplayed;
    private colorsModes colorsMode;
    [SerializeField]
    private audioModes[] audioModesToBeDisplayed;
    private audioModes audioMode;

    [Header("Quest over to be displayed")]
    [SerializeField]
    private quests[] questsOverToBeDisplayed;
    [SerializeField]
    private bool failCutsceneAvailable;
    private bool failCutsceneAction;

    [Space]
    [Header("Quest")]
    [SerializeField]
    private quests quest;
    [SerializeField]
    private int questPhasePopIn;
    [SerializeField]
    private int questPhasePopOut;

    [Header("Quests Start")]
    [SerializeField]
    private quests[] questsStart;

    [Header("Quest Phase change")]
    [SerializeField]
    private bool phaseChangeAfterCutscene;
    [SerializeField]
    private quests[] questNewQuestPhase;
    [SerializeField]
    private int[] newQuestPhase;

    private int questPhase;

    [Space]
    [Header("Action")]
    [SerializeField]
    private actions action;
    private actions noFailAction;
    [SerializeField]
    private actionButtons actionButton;
    [SerializeField]
    private string actionText;

    [Header("Item needed to perform action")]
    [SerializeField]
    private bool itemsNeeded;
    [SerializeField]
    private items[] neededItems;
    private int neededItemsInInventory;

    [Header("Use (or drop)")]
    [SerializeField]
    private items itemToUseID;
    [SerializeField]
    private bool removeFromInvetory;

    [Header("Help display")]
    [SerializeField]
    private float secondsBeforeShowingHelp;

    private float helpCountDown;
    private bool countDownIsRunning;
    private bool newHelpIsSet;
    private Coroutine helpCountDownCoroutine;

    [Header("Point of view to perform action")]
    [SerializeField]
    private bool pointOfViewNeeded;
    [SerializeField]
    private Vector3 neededPointOfView;

    private GameObject playerView;
    private GameObject playerController;
    private Vector3 playerPointOfView;
    private Vector3 playerControllerPointOfView;
    private float xGap;
    private float xMaxGap = 20f;
    private float yGap;
    private float yMaxGap = 12f;
    private bool gapOK;


    [Header("Mode needed to perform action")]
    [SerializeField]
    private bool modeNeeded;
    [SerializeField]
    private bool dimensionModeNeeded;
    [SerializeField]
    private dimensionModes neededDimensionMode;
    [SerializeField]
    private bool movementModeNeeded;
    [SerializeField]
    private movementModes neededMovementMode;
    [SerializeField]
    private bool colorsModeNeeded;
    [SerializeField]
    private colorsModes neededColorsMode;
    [SerializeField]
    private bool audioModeNeeded;
    [SerializeField]
    private audioModes neededAudioMode;
    private int neededModesNum;
    private int neededModeActive;

    [Space]
    [Header("Game Phase Change")]
    [SerializeField]
    private bool gamePhaseChange;
    [SerializeField]
    private gamePhases newGamePhase;

    [Header("World Change")]
    [SerializeField]
    private bool worldChange;
    [SerializeField]
    private worldChangeTypes worldChangeType;
    [SerializeField]
    private landmarks landmarkObjectToChange;
    [SerializeField]
    private landmarks newLandmarkObject;

    [Header("Mode Changes")]
    [SerializeField]
    private bool modeChange;
    [SerializeField]
    private bool changeAfterCutscene;
    [SerializeField]
    private bool dimensionModeChange;
    [SerializeField]
    private dimensionModes newDimensionMode;
    [SerializeField]
    private bool movementModeChange;
    [SerializeField]
    private movementModes newMovementMode;
    [SerializeField]
    private bool colorsModeChange;
    [SerializeField]
    private colorsModes newColorsMode;
    [SerializeField]
    private bool audioModeChange;
    [SerializeField]
    private audioModes newAudioMode;

    [Header("Case change (2d Grid)")]
    [SerializeField]
    private bool caseChange;
    [SerializeField]
    private int caseTochange;
    [SerializeField]
    private int caseNewValue;

    [Header("Only for the start of the game")]
    [SerializeField]
    private bool displayFirstHelp;

    private int gamePhase;

    private bool isItemVisible;
    private bool wasItemVisible;

    private bool playerIsIn;

    private void OnEnable()
    {
        playerView = GameObject.Find("CenterEyeAnchor");
        playerController = GameObject.FindGameObjectWithTag("Player");

        noFailAction = action;

        CheckDisplay();
    }

    private void OnDisable()
    {
        CM.SetPlayerInActionZone(false);
        IIM.ResetAction(action.ToString());
    }

    private void Update()
    {
        if (GetComponent<Renderer>().isVisible)
        {
            isItemVisible = true;
        }
        else
        {
            // temporary remove to prevent player to back in and out
            //isItemVisible = false;
            isItemVisible = true;
        }

        if (playerIsIn == false)
        {
            int currentGamePhase = GM.GetGamePhase();

            if (gamePhase != currentGamePhase)
            {
                gamePhase = currentGamePhase;

                CheckDisplay();
            }

            int currentQuestPhase = GM.GetQuestPhase((int)quest);

            if (questPhase != currentQuestPhase)
            {
                questPhase = currentQuestPhase;

                CheckDisplay();
            }
        }

        // if a specific view point is needed to display
        if (pointOfViewNeeded == true && playerIsIn == true)
        {
            // get player rotation

            playerPointOfView = playerView.transform.localRotation.eulerAngles;
            playerControllerPointOfView = playerController.transform.localRotation.eulerAngles;

            xGap = Mathf.Abs(neededPointOfView.x - playerPointOfView.x);
            yGap = Mathf.Abs(neededPointOfView.y - playerControllerPointOfView.y);

            if (xGap < xMaxGap && yGap < yMaxGap)
            {
                if (gapOK == false)
                {
                    gapOK = true;
                    CheckDisplay();
                }
            }
            else
            {
                if (gapOK == true)
                {
                    gapOK = false;
                    CheckDisplay();
                }
            }
        }
    }

    // custom debug display method
    private void Dbug(string dbugText)
    {
        if (onDbug == true)
        {
            Debug.Log(dbugText);
        }
    }

    public void CheckDisplay()
    {
        colorsMode = (colorsModes)System.Enum.Parse(typeof(colorsModes), GM.GetColorsMode());
        audioMode = (audioModes)System.Enum.Parse(typeof(audioModes), GM.GetAudioMode());

        // if itme is part of a quest
        if (quest != quests.None)
        {
            // get current quest phase
            questPhase = GM.GetQuestPhase((int)quest);

            bool questDisplay;

            // display or hide item according to quest phase
            if (questPhase >= questPhasePopIn && questPhase < questPhasePopOut)
            {
                questDisplay = true;
            }
            else
            {
                questDisplay = false;
            }

            // get quets to be over IDs
            bool questToBeOverDisplay = true;
            
            if (questsOverToBeDisplayed.Length > 0)
            {
                for (int i = 0; i < questsOverToBeDisplayed.Length; i++)
                {
                    if(GM.GetQuestPhase((int)questsOverToBeDisplayed[i]) < 999)
                    {
                        questToBeOverDisplay = false;
                    }
                }
            }

            // set questToBeOverDisplay to true if false and fail cutscene available
            if (questToBeOverDisplay == false && failCutsceneAvailable == true)
            {
                questToBeOverDisplay = true;
                failCutsceneAction = true;
            }

            // if a specific color mode is needed to display the item
            bool colorsDisplay = true;

            if (colorModesToBeDisplayed.Length > 0)
            {
                // defaut to false
                colorsDisplay = false;

                for (int i = 0; i < colorModesToBeDisplayed.Length; i++)
                {
                    // if at least one mode is the right one -> display to true
                    if (colorsMode == colorModesToBeDisplayed[i])
                    {
                        colorsDisplay = true;
                    }
                }
            }

            // if a specific audio mode is needed to display the item
            bool audioDisplay = true;

            if (audioModesToBeDisplayed.Length > 0)
            {
                // defaut to false
                audioDisplay = false;

                for (int i = 0; i < audioModesToBeDisplayed.Length; i++)
                {
                    // if at least one mode is the right one -> display to true
                    if (audioMode == audioModesToBeDisplayed[i])
                    {
                        audioDisplay = true;
                    }
                }
            }

            // if every display conditions are true -> display item
            if (questDisplay == true && colorsDisplay == true && questToBeOverDisplay == true && audioDisplay == true)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        // landmarks management
        if (itemType == itemTypes.Landmark)
        {
            // get current game phase
            gamePhase = GM.GetGamePhase();

            // display or hide landmark according to game phase
            if (gamePhase >= (int)landmarkPopIn && gamePhase < (int)landmarkPopOut)
            {
                // warning !! crash the game in a loop
                //this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        // help display management
        if (action == actions.Help)
        {
            this.GetComponent<Cutscenes>().SetActiveTextDialogue(0);
            IIM.SetHelpAvailable(true);
            AddAction();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (action == actions.NewHelp && newHelpIsSet == false)
        {
            if (helpCountDown >= secondsBeforeShowingHelp)
            {
                SetNewHelp();
                newHelpIsSet = true;
            }
        }

        if (action != actions.None && pointOfViewNeeded)
        {
            if (gapOK == true)
            {
                AddAction();
            }
            else
            {
                IIM.ResetAction(action.ToString());
            }
        }

        // if item disappear -> reset action
        if (isItemVisible == false && isItemVisible != wasItemVisible)
        {
            IIM.ResetAction(action.ToString());
        }

        wasItemVisible = isItemVisible;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsIn = true;

            CM.SetPlayerInActionZone(true);

            if (action == actions.NewHelp)
            {
                if (helpCountDown < secondsBeforeShowingHelp)
                {
                    helpCountDownCoroutine = StartCoroutine(countDownCoroutine());
                }
                else
                {
                    SetNewHelp();

                    if (countDownIsRunning == true)
                    {
                        StopCoroutine(helpCountDownCoroutine);
                        countDownIsRunning = false;
                    }
                }
            }
            else
            {
                IIM.SetHelpDisplayed(false);

                bool questToBeOverDisplay = true;

                if (questsOverToBeDisplayed.Length > 0)
                {
                    for (int i = 0; i < questsOverToBeDisplayed.Length; i++)
                    {
                        if (GM.GetQuestPhase((int)questsOverToBeDisplayed[i]) < 999)
                        {
                            questToBeOverDisplay = false;
                        }
                    }
                }

                // set questToBeOverDisplay to true if false and fail cutscene available
                if (questToBeOverDisplay == false && failCutsceneAvailable == true)
                {
                    failCutsceneAction = true;
                }
                else
                {
                    failCutsceneAction = false;
                }

                if (itemsNeeded == true)
                {
                    CheckInventory();
                }

                if (modeNeeded == true)
                {
                    CheckMode();
                }

                if (neededModeActive == neededModesNum)
                {
                    if (itemsNeeded == false || (itemsNeeded == true && neededItemsInInventory > 0))
                    {
                        if (action != actions.None)
                        {
                            AddAction();
                        }

                        if (gamePhaseChange == true)
                        {
                            AddGamePhaseChange();
                        }

                        if (questsStart.Length > 0)
                        {
                            int[] questsTostart = new int[questsStart.Length];

                            for (int i = 0; i < questsStart.Length; i++)
                            {
                                questsTostart[i] = (int)questsStart[i];
                            }

                            AddQuestStart(questsTostart);
                        }

                        if (newQuestPhase.Length > 0)
                        {
                            AddQuestPhaseChange();
                        }

                        if (worldChange == true)
                        {
                            AddWorldChange();
                        }

                        if (modeChange == true)
                        {
                            AddModeChange();
                        }

                        if (caseChange == true)
                        {
                            ChangeCaseValue();
                        }
                    }
                }
            }

            if (actionButton == actionButtons.TriggerEnter)
            {
                IIM.SetButtonDown("TriggerEnter");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsIn = false;

            CM.SetPlayerInActionZone(false);

            if (action != actions.NewHelp)
            {
                IIM.SetHelpDisplayed(true);

                if (itemsNeeded == false || (itemsNeeded == true && neededItemsInInventory > 0))
                {
                    if (action != actions.None)
                    {
                        IIM.ResetAction(action.ToString());
                    }

                    if (worldChange == true)
                    {
                        IIM.ResetWorldChange();
                    }

                    if (gamePhaseChange == true)
                    {
                        IIM.ResetGamePhase();
                    }
                }
            }
            else
            {
                StopCoroutine(helpCountDownCoroutine);
                countDownIsRunning = false;

                newHelpIsSet = false;
            }
        }

        failCutsceneAction = false;
        neededModesNum = 0;
        neededModeActive = 0;
        neededItemsInInventory = 0;
    }

    private IEnumerator countDownCoroutine()
    {
        countDownIsRunning = true;

        for (int i = 0; helpCountDown < secondsBeforeShowingHelp; i++)
        {
            helpCountDown++;
            yield return new WaitForSeconds(1);
        }
    }

    private void SetNewHelp()
    {
        GameObject helpUI = IIM.GetHelpUI();
        helpUI.GetComponent<Cutscenes>().SetNewDialogues(this.GetComponent<Cutscenes>().GetTextDialogues());
        IIM.SetHelpAvailable(true);
        IIM.SetHelpDisplayed(true);
        this.gameObject.SetActive(false);

        newHelpIsSet = true;
    }

    private void AddAction()
    {
        if (action != actions.Help)
        {
            if (actionButton != actionButtons.TriggerEnter)
            {
                IIM.DisplayAction("[" + actionButton.ToString() + "]\n" + actionText);
            }
        }

        if (failCutsceneAction == true)
        {
            action = actions.StartFailCutscene;
        }
        else
        {
            action = noFailAction;
        }

        IIM.SetAction(action.ToString(), actionButton.ToString(), this.gameObject);
    }

    private void AddWorldChange()
    {
        IIM.SetWorldChange(worldChange, worldChangeType.ToString(), (int)landmarkObjectToChange, (int)newLandmarkObject);
    }

    private void AddGamePhaseChange()
    {
        IIM.SetGamePhase(gamePhaseChange, (int)newGamePhase);
    }

    private void AddQuestStart(int[] questsToStart)
    {
        IIM.SetQuestsToStart(questsToStart);
    }

    private void AddQuestPhaseChange()
    {
        int[] quests = new int[questNewQuestPhase.Length];

        for (int i = 0; i < questNewQuestPhase.Length; i++)
        {
            quests[i] = (int)questNewQuestPhase[i];
        }

        IIM.SetQuestPhaseChange(quests, phaseChangeAfterCutscene);
    }

    private void AddModeChange()
    {
        IIM.SetModeChange(true, changeAfterCutscene);
    }

    private void ChangeCaseValue()
    {
        GM.SetMapCaseType(caseTochange, caseNewValue);
    }

    public bool GetDimensionModeChange()
    {
        return dimensionModeChange;
    }

    public string GetNewDimensionMode()
    {
        return newDimensionMode.ToString();
    }

    public bool GetMovementModeChange()
    {
        return movementModeChange;
    }

    public string GetNewMovementMode()
    {
        return newMovementMode.ToString();
    }

    public bool GetColorsModeChange()
    {
        return colorsModeChange;
    }

    public string GetNewColorsMode()
    {
        return newColorsMode.ToString();
    }

    public bool GetAudioModeChange()
    {
        return audioModeChange;
    }

    public string GetNewAudioMode()
    {
        return newAudioMode.ToString();
    }

    public int GetItemInventoryID()
    {
        return itemInventoryID;
    }

    public int GetLandmarkID()
    {
        return landmarkID;
    }
    public int GetItemToUseID()
    {
        return (int)itemToUseID;
    }

    public bool GetRemoveFromInventory()
    {
        return removeFromInvetory;
    }

    public int[] GetQuestIDToChangePhase()
    {
        int[] quests = new int[questNewQuestPhase.Length];

        for (int i = 0; i < questNewQuestPhase.Length; i++)
        {
            quests[i] = (int)questNewQuestPhase[i];
        }

        return quests;
    }

    public int[] GetNewQuestPhase()
    {
        return newQuestPhase;
    }

    public bool GetPlayerIsIn()
    {
        return playerIsIn;
    }

    private void CheckInventory()
    {
        int[] inventory = GM.GetInventory();

        for (int i = 0; i < neededItems.Length; i++)
        {
            if (inventory[(int)neededItems[i]] >= 1)
            {
                neededItemsInInventory++;
            }
        }
    }

    private void CheckMode()
    {
        if (dimensionModeNeeded == true)
        {
            neededModesNum++;

            if (neededDimensionMode.ToString() == WM.GetDimensionsMode())
            {
                neededModeActive++;
            }
        }
        if (movementModeNeeded == true)
        {
            neededModesNum++;

            if (neededMovementMode.ToString() == MM.GetMovementMode())
            {
                neededModeActive++;
            }
        }
        if (colorsModeNeeded == true)
        {
            neededModesNum++;

            if (neededColorsMode.ToString() == WM.GetColorsMode())
            {
                neededModeActive++;
            }
        }
        if (audioModeNeeded == true)
        {
            neededModesNum++;

            Debug.Log("audio needed : " + neededAudioMode.ToString());
            Debug.Log("audio active : " + AM.GetAudioMode());

            if (neededAudioMode.ToString() == AM.GetAudioMode())
            {
                neededModeActive++;
            }
        }
    }

    public string GetAction()
    {
        return action.ToString();
    }

    public bool GetDisplayFirstHelp()
    {
        return displayFirstHelp;
    }

    public Sprite GetItemIconSprite()
    {
        return itemSprite;
    }
}
