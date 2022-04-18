using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WorldManager : MonoBehaviour
{

    // save / new game / new game with code
    private bool newGameWithCode;
    private int saveType;

    [Header("Managers")]
    private GameManager GM;
    private AudioManager AM;
    [SerializeField]
    private MovementsManager MM;
    [SerializeField]
    private UIWorldManager UIWM;
    [SerializeField]
    private UIInGame UIIG;

    private int _testInt;

    [Header("Camera")]
    [SerializeField]
    private Camera inGameCamera;

    [Header("Game Modes")]
    [SerializeField]
    private GameObject GameMode;

    [Header("Water")]
    [SerializeField]
    private GameObject water;
    private float waterY2D = 0.19f;
    private float waterY3D = 0f;

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource voicesAudioSource;
    [SerializeField]
    private AudioSource pickUpAudioSource;

    [Header("Post Credits Voice")]
    [SerializeField]
    private AudioClip postCreditsVoice;

    enum dimensionModes
    {
        None = 0,
        Flat2DCube = 1,
        Facing2D = 2,
        Full3D = 3
    }

    private dimensionModes dimensionsMode;

    [Header("Flat 2D Cube")]
    [SerializeField]
    public GameObject twoDimensionalCubicOverlayCanvas;
    private bool twoDimensionalCubicOverlayCanvasActive;

    [Header("PostProcess Volume")]
    [SerializeField]
    private PostProcessVolume processVolume;

    [Header("Color Profiles")]
    [SerializeField]
    private PostProcessProfile[] colorProfiles;

    enum colorsModes
    {
        None = 0,
        BlackAndWhite = 1,
        Colors16 = 2,
        Colors256 = 3,
        ColorsFull = 4
    }

    private colorsModes colorsMode;

    [Header("Pause Mode Objects")]
    [SerializeField]
    private GameObject pauseMode;
    private Vector3 pauseModeDefaultPos;
    private Quaternion pauseModeDefaultRot;
    [SerializeField]
    private GameObject[] pausePanels;
    [SerializeField]
    private GameObject renderBoyPanel;
    [SerializeField]
    private GameObject mapPanel;
    [SerializeField]
    private GameObject warningPanel;
    [SerializeField]
    private GameObject saveButton;
    private Color inGameSkyColor = new Color(.2f, .5f, 1, 1);
    private Color inPauseSkyColor = new Color(.2f, .2f, .2f, 1);

    [Header("PlayerController")]
    [SerializeField]
    private GameObject playerControllerGO;
    [SerializeField]
    private GameObject playerTrackingSpaceGO;
    [SerializeField]
    private GameObject playerView;

    // player controller position on world
    private Vector3 playerControllerPosition;
    private Vector3 playerTrackingPosition;
    private Vector3 playerTrackingRotation;
    private Vector3 playerViewRotation;

    private void Awake()
    {
        // find the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();
        AM = gameManagerGameObject.GetComponentInChildren<AudioManager>();

        GM.setPauseState(false);
        AM.SetInGame(true);

        // get game type (new, new with code, load)
        newGameWithCode = GM.GetNewGameWithCode();

        // if new game with code
        if (newGameWithCode == true)
        {
            GM.NewSaveLevel2();
            GM.LoadFile("save");

            GM.NewMapCode();
            GM.LoadFile("map");
        }
        else
        {
            // if new game
            if (GM.GetContinueGame() == false)
            {
                GM.NewSave();
                GM.NewMap();

                // FOR DEBUG PURPOSE
                //GM.NewMapCode();
                //GM.NewSaveLevel2();
                //GM.NewSaveLevel3();
                //GM.NewSaveLevel4();
                //GM.NewSaveLevel5();
            }

            if (GM.GetPostCredits() == true)
            {
                GM.NewSaveLevel6();
            }

            GM.LoadFile("save");
            GM.LoadFile("map");
        }

        // get saved player position and rotation
        Vector3 savedPlayerPosition = GM.GetPlayerPosition();
        Vector3 savedPlayerRotation = GM.GetPlayerRotation();

        // set player position and rotation for post-credits
        if (GM.GetPostCredits() == true)
        {
            string finalChoice = GM.GetFinalChoice();

            switch (finalChoice)
            {
                case "A":
                    savedPlayerPosition = new Vector3(-90.6f, 12.6f, -175.1f);
                    savedPlayerRotation = new Vector3(0, 314, 0);
                    break;
                case "B":
                    savedPlayerPosition = new Vector3(-9f, 12.6f, -106.6f);
                    savedPlayerRotation = new Vector3(0, 260, 0);
                    break;
                //case "X"
                default:
                    savedPlayerPosition = new Vector3(-49.4f, 13.4f, -47.2f);
                    savedPlayerRotation = new Vector3(0, 38.7f, 0);
                    break;
            }

            StartCoroutine(PostCreditsVoiceCoroutine());
        }

        // move the player to the saved place
        playerControllerGO.transform.localPosition = savedPlayerPosition;
        playerTrackingSpaceGO.transform.eulerAngles = savedPlayerRotation;
    }

    private void Start()
    {
        // set saved dimensions and colors modes
        dimensionModes savedDimensionMode = (dimensionModes)System.Enum.Parse(typeof(dimensionModes), GM.GetDimensionsMode());
        colorsModes saveColorsMode = (colorsModes)System.Enum.Parse(typeof(colorsModes), GM.GetColorsMode());

        dimensionsMode = savedDimensionMode;
        colorsMode = saveColorsMode;

        ChangeDimensionsMode();
        ChangeColorsMode(false);

        // store pauseMode defaut position and rotation
        pauseModeDefaultPos = pauseMode.transform.position;
        pauseModeDefaultRot = pauseMode.transform.rotation;
    }

    private void Update()
    {
        // if game paused
        bool pauseState = GM.getPauseState();

        if (pauseState == false)
        {
            // store player controller in game position and rotation
            playerControllerPosition = playerControllerGO.transform.localPosition;
            playerTrackingPosition = playerTrackingSpaceGO.transform.position;
            playerTrackingRotation = playerTrackingSpaceGO.transform.eulerAngles;
            playerViewRotation = playerView.transform.eulerAngles;

            ShowTwoDiensionalCubicOverlayCanvas(twoDimensionalCubicOverlayCanvasActive);
        }
    }
    
    public void SetGamePause(bool state, bool toMap)
    {
        // music croosfade
        AM.PauseModeMusicCrossFade(state);

        if (state == true)
        {
            // set pause to true in Game Manager & pause game
            SetGMPauseState(true);
            Time.timeScale = 0;

            // change "sky" color
            playerView.GetComponent<Camera>().backgroundColor = inPauseSkyColor;

            // set pause color mode
            ChangeColorsMode(true);

            float newY = playerViewRotation.y - playerTrackingRotation.y;
            pauseMode.transform.eulerAngles = new Vector3(0, newY, 0);

            // show pause UI objects
            pauseMode.SetActive(true);

            // savebutton activation
            if (GM.GetGamePhase() >= 51)
            {
                if (saveButton.activeSelf == false)
                {
                    saveButton.SetActive(true);
                }
            }
            else
            {
                if (saveButton.activeSelf == true)
                {
                    saveButton.SetActive(false);
                }
            }

            // deactivate all panels before activate the right one
            for (int i = 0; i < pausePanels.Length; i++)
            {
                pausePanels[i].SetActive(false);
            }

            if (toMap == false)
            {
                renderBoyPanel.SetActive(true);
                UIWM.SetCurrrentAndNextPanel(renderBoyPanel);
            }
            else
            {
                mapPanel.SetActive(true);
                UIWM.SetCurrrentAndNextPanel(mapPanel);
            }

            UIWM.showInGamePlayerUI(false);
            UIWM.ShowControllers(true);

            Vector3 inPausePlayerLocation = new Vector3(0, 4000, 0);
            Vector3 inPausePlayerRotation = new Vector3(0, 1, 0);

            // reset position and rotation before rotation
            pauseMode.transform.position = pauseModeDefaultPos;

            playerTrackingSpaceGO.transform.position = inPausePlayerLocation;
            playerTrackingSpaceGO.transform.eulerAngles = inPausePlayerRotation;

            ShowTwoDiensionalCubicOverlayCanvas(false);
        }
        else
        {
            // change "sky" color
            playerView.GetComponent<Camera>().backgroundColor = inGameSkyColor;

            // set pause to false in Game Manager & resume game
            SetGMPauseState(false);
            Time.timeScale = 1;

            // set right color mode
            ChangeColorsMode(false);

            // hide warning panel if active
            if (warningPanel.activeSelf == true)
            {
                warningPanel.SetActive(false);
            }

            // hide pause UI objects
            pauseMode.SetActive(false);

            UIWM.showInGamePlayerUI(true);
            UIWM.ShowControllers(false);

            playerTrackingSpaceGO.transform.position = playerTrackingPosition;
            playerTrackingSpaceGO.transform.eulerAngles = playerTrackingRotation;
            //playerControllerGO.transform.eulerAngles = playerControllerRotation;

            if (twoDimensionalCubicOverlayCanvasActive == true)
            {
                ShowTwoDiensionalCubicOverlayCanvas(true);
            }
        }
    }

    private IEnumerator PostCreditsVoiceCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        voicesAudioSource.clip = postCreditsVoice;
        voicesAudioSource.Play();
    }

    public void SetGamePauseButton(bool state)
    {
        SetGamePause(state, false);
    }

    // CAMERA CULLINGMASK METHODS
    private void ShowCullingMaSk(string layerName)
    {
        inGameCamera.cullingMask |= 1 << LayerMask.NameToLayer(layerName);
    }

    private void HideCullingMack(string layerName)
    {
        inGameCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(layerName));
    }

    // DIMENSIONS MODES
    private void ChangeDimensionsMode()
    {
        switch (dimensionsMode)
        {
            case dimensionModes.Full3D:
                HideCullingMack(dimensionModes.Facing2D.ToString());
                ShowCullingMaSk(dimensionModes.Full3D.ToString());
                SetTwoDimensionalCubicOverlayCanvasActive(false);
                SetWaterY(dimensionModes.Full3D);
                playerControllerGO.GetComponent<CapsuleCollider>().enabled = true;
                break;
            case dimensionModes.Facing2D:
                ShowCullingMaSk(dimensionModes.Facing2D.ToString());
                HideCullingMack(dimensionModes.Full3D.ToString());
                SetTwoDimensionalCubicOverlayCanvasActive(false);
                SetWaterY(dimensionModes.Facing2D);
                playerControllerGO.GetComponent<CapsuleCollider>().enabled = true;
                break;
            case dimensionModes.Flat2DCube:
                SetTwoDimensionalCubicOverlayCanvasActive(true);
                SetWaterY(dimensionModes.Flat2DCube);
                playerControllerGO.GetComponent<CapsuleCollider>().enabled = false;
                break;
        }

        // set dimensions mode in GM for save
        GM.SetDimensionssMode(dimensionsMode.ToString());
    }

    public void SetDimensionsMode(string mode)
    {
        dimensionsMode = (dimensionModes)System.Enum.Parse(typeof(dimensionModes), mode);
        ChangeDimensionsMode();
    }

    public string GetDimensionsMode()
    {
        return dimensionsMode.ToString();
    }

    // COLORS MODE
    private void ChangeColorsMode(bool isPaused)
    {
        if (isPaused == true)
        {
            processVolume.profile = colorProfiles[3];
        }
        else
        {
            if (dimensionsMode != dimensionModes.Flat2DCube)
            {
                switch (colorsMode)
                {
                    case colorsModes.BlackAndWhite:
                        processVolume.profile = colorProfiles[0];
                        //ChangeColorCullingMask(colorsModes.BlackAndWhite);
                        break;
                    case colorsModes.Colors16:
                        processVolume.profile = colorProfiles[1];
                        //ChangeColorCullingMask(colorsModes.Colors16);
                        break;
                    case colorsModes.Colors256:
                        processVolume.profile = colorProfiles[2];
                        //ChangeColorCullingMask(colorsModes.Colors256);
                        break;
                    case colorsModes.ColorsFull:
                        processVolume.profile = colorProfiles[3];
                        //ChangeColorCullingMask(colorsModes.ColorsFull);
                        break;
                }
            }
            else
            {
                processVolume.profile = colorProfiles[3];
            }
        }

        // set colors mode in GM for save
        GM.SetColorssMode(colorsMode.ToString());
    }

    public void SetColorsMode(string mode)
    {
        colorsMode = (colorsModes)System.Enum.Parse(typeof(colorsModes), mode);
    }

    public string GetColorsMode()
    {
        return colorsMode.ToString();
    }

    // GENERAL METHODS
    private void SetWaterY(dimensionModes mode)
    {
        if (mode == dimensionModes.Full3D)
        {
            water.transform.position = new Vector3(water.transform.position.x, waterY3D, water.transform.position.z);
        }
        else
        {
            water.transform.position = new Vector3(water.transform.position.x, waterY2D, water.transform.position.z);
        }
    }

    private void SetTwoDimensionalCubicOverlayCanvasActive(bool state)
    {
        twoDimensionalCubicOverlayCanvasActive = state;
    }

    private void ShowTwoDiensionalCubicOverlayCanvas(bool state)
    {
        twoDimensionalCubicOverlayCanvas.SetActive(state);
    }

    public void SetGMPauseState(bool state)
    {
        GM.setPauseState(state);
    }

    public Vector3 GetPlayerPosition()
    {
        return playerControllerPosition;
    }
    public Vector3 GetPlayerRotation()
    {
        return playerTrackingRotation;
    }

    public void addToLogText(string logText)
    {
        UIIG.SetDebugText("WM add log");
        // store in GM for saves
        GM.AddToLogText(logText);
        //GM.DebugText();
    }

    public string GetLogText()
    {
        return GM.GetLogText();
    }

    public string GetAudioMode()
    {
        return GM.GetAudioMode();
    }

    public AudioSource GetVoicesAudioSource()
    {
        return voicesAudioSource;
    }

    public AudioSource GetPickUpAUdioSource()
    {
        return pickUpAudioSource;
    }
}
