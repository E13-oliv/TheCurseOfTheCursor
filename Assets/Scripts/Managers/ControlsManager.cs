using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{

    [Header("Managers")]
    [SerializeField]
    private UIInGame UIIG;
    [SerializeField]
    private UILogPanel UILP;
    private InteractiveItemsManager IIM;
    private GameManager GM;
    private WorldManager WM;
    private CutscenesManager CSM;

    [Header("Movements Manager")]
    [SerializeField]
    private MovementsManager MM;

    [Header("Player Controller")]
    [SerializeField]
    private OVRPlayerController PC;

    private bool gridPlayerCanMove = true;

    private bool isRunning = false;
    private float baseSpeed = 3f;
    private float runSpeed = 6f;
    //private float runSpeed = 12f;

    // ACTIONS AND CUTSCENES
    private bool playerInActionZone = false;
    private bool cutsceneActive = false;
    private bool endCutsceneActive = false;
    private bool postCreditsActive = false;

    // PAUSE
    private bool canvasSliding = false;

    private void Start()
    {
        // find the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        WM = this.GetComponent<WorldManager>();
        IIM = this.GetComponent<InteractiveItemsManager>();
        CSM = this.GetComponent<CutscenesManager>();
    }

    private void Update()
    {
        postCreditsActive = GM.GetPostCredits();

        if (postCreditsActive == true)
        {
            PC.SetMoveScaleMultiplier(0);

            if (GM.GetCanSkipCredits() == true)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.A))
                {
                    CSM.SetCreditsButton(true);
                }
            }
        }
        else
        {
            // get game pause state
            bool pauseState = GM.getPauseState();

            // BUTTONS INPUT

            // Start button : pause or resume game
            if (OVRInput.GetDown(OVRInput.Button.Start) || Input.GetKeyDown(KeyCode.Space))
            {
                if (cutsceneActive == false)
                {
                    if (pauseState == false)
                    {
                        WM.SetGamePause(true, false);
                    }
                    else
                    {
                        if (canvasSliding == false)
                        {
                            WM.SetGamePause(false, false);
                        }
                    }
                }
            }

            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                if (endCutsceneActive == true)
                {
                    CSM.SetEndCutsceneChoiceButton("A");
                    GM.SetFinalChoice("A");
                }
                else
                {
                    if (cutsceneActive == false)
                    {
                        if (playerInActionZone == false)
                        {
                            PC.Jump();
                        }
                    }
                    else
                    {
                        CSM.SetCutsceneButton(true);
                    }

                    if (pauseState == false)
                    {
                        IIM.SetButtonDown("A");
                    }
                }
            }

            if (pauseState == false)
            {
                string currentMovementMode = GM.GetMovementsMode();

                if (currentMovementMode != "Grid2D")
                {
                    float leftTrigger = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
                    if (leftTrigger > .1f)
                    {
                        PC.SetMoveScaleMultiplier(runSpeed);
                        isRunning = true;
                    }
                    else
                    {
                        if (isRunning == true)
                        {
                            PC.SetMoveScaleMultiplier(baseSpeed);
                            isRunning = false;
                        }
                    }
                }

                if (OVRInput.GetDown(OVRInput.RawButton.B))
                {
                    if (endCutsceneActive == true)
                    {
                        CSM.SetEndCutsceneChoiceButton("B");
                        GM.SetFinalChoice("B");
                    }
                    else
                    {
                        IIM.SetButtonDown("B");
                    }
                }

                if (OVRInput.GetDown(OVRInput.RawButton.X))
                {
                    if (endCutsceneActive == true)
                    {
                        CSM.SetEndCutsceneChoiceButton("X");
                        GM.SetFinalChoice("X");
                    }
                    else
                    {
                        IIM.SetButtonDown("X");
                    }
                }
            }

            if (OVRInput.GetDown(OVRInput.RawButton.Y))
            {
                if (cutsceneActive == false)
                {
                    if (pauseState == false)
                    {
                        WM.SetGamePause(true, true);
                    }
                    else
                    {
                        if (canvasSliding == false)
                        {
                            WM.SetGamePause(false, false);
                        }
                    }
                }

                if (pauseState == false)
                {
                    IIM.SetButtonDown("Y");
                }
            }

            if (OVRInput.GetUp(OVRInput.Button.One) || OVRInput.GetUp(OVRInput.Button.Two) || OVRInput.GetUp(OVRInput.Button.Three) || OVRInput.GetUp(OVRInput.Button.Four))
            {
                IIM.SetButtonDown("None");
            }


            // STICKS INPUT
            Vector2 leftThumbstick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
            Vector2 rightThumbstick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

            if (cutsceneActive == false)
            {
                if (pauseState == true)
                {
                    if (rightThumbstick.y > .2f)
                    {
                        UILP.SetStickMovement(1);
                    }
                    else if (rightThumbstick.y < -0.2f)
                    {
                        UILP.SetStickMovement(-1);
                    }
                    else
                    {
                        UILP.SetStickMovement(0);
                    }
                }
                else
                {
                    if (leftThumbstick.y > .2f)
                    {
                        if (gridPlayerCanMove == true)
                        {
                            MM.SetGridMoveState(0);
                            gridPlayerCanMove = false;
                        }
                    }
                    if (leftThumbstick.x > .2f)
                    {
                        if (gridPlayerCanMove == true)
                        {
                            MM.SetGridMoveState(1);
                            gridPlayerCanMove = false;
                        }
                    }
                    if (leftThumbstick.y < -0.2f)
                    {
                        if (gridPlayerCanMove == true)
                        {
                            MM.SetGridMoveState(2);
                            gridPlayerCanMove = false;
                        }
                    }
                    if (leftThumbstick.x < -0.2f)
                    {
                        if (gridPlayerCanMove == true)
                        {
                            MM.SetGridMoveState(3);
                            gridPlayerCanMove = false;
                        }
                    }
                    if (leftThumbstick.y == 0 && leftThumbstick.x == 0)
                    {
                        gridPlayerCanMove = true;
                    }
                }
            }
        }
    }

    public void SetPlayerSpeed(float speed)
    {
        PC.SetMoveScaleMultiplier(speed);
    }

    public void SetPlayerRotation(bool state)
    {
        PC.EnableRotation = state;
    }

    public void SetPlayerInActionZone(bool state)
    {
        playerInActionZone = state;
    }

    public void SetCutsceneActive(bool state)
    {
        cutsceneActive = state;
    }

    public void SetEndCutsceneActive(bool state)
    {
        endCutsceneActive = state;
    }

    public void SetPostCreditsActive(bool state)
    {
        postCreditsActive = state;
    }

    public void SetCanvasSliding(bool state)
    {
        canvasSliding = state;
    }
}
