using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementsManager : MonoBehaviour
{
    // MANAGERS
    private GameManager GM;
    private XmlManager xmlManager;

    [Header("Player Controller")]
    [SerializeField]
    private OVRPlayerController playerController;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject playerTrackingSpace;

    Vector3 playerPosition;
    Vector3 playerRotation;

    [Header("In Game UI")]
    [SerializeField]
    private UIInGame UIIG;

    // Movement modes
    enum movementModes
    {
        None = 0,
        Grid2D = 1,
        Free2D = 2,
        Free3D = 3
    }

    private movementModes movementMode;
    private movementModes newMovementMode;

    private bool stickRotation;

    //private bool currentStickRotation;

    // 2D GRID
    private float angleGap;
    // 0 = north, 1 = east, 2 = south, 3 = west
    private int cardinalPoint;
    private string cardinalPointLetter;
    private int gridMoveState = -1;
    private bool facingCardinalPoint;
    private float cardinalpointGap = 25.0f;

    private bool eastSpecial;
    private bool westSpecial;

    // MAP
    private int caseSize = 10;
    private int mapColums = 42;
    private int mapLines = 35;
    private int mapNumOfCases;
    private bool[] casesAccessible = new bool[] { false, false, false, false };
    private int[] casesAccessibleAltitude = new int[] { 0, 0, 0, 0 };
    private int[] mapCasesType;
    private int caseNumber;

    private void Start()
    {
        // fing the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        // get config data
        stickRotation = GM.GetRotationStick();

        // get map data
        GM.LoadFile("map");
        mapCasesType = GM.GetMapCasesType();

        mapNumOfCases = mapColums * mapLines;

        // set default movement mode
        newMovementMode = movementModes.Free3D;
        newMovementMode = (movementModes)System.Enum.Parse(typeof(movementModes), GM.GetMovementsMode());
        ChangeMovementMode(true);
    }

    private void Update()
    {
        // on map position (first case number = 0)
        int xPos = Mathf.RoundToInt(transform.position.x / 10) * 10;
        int zPos = Mathf.Abs(Mathf.RoundToInt(transform.position.z / 10) * 10);

        caseNumber = zPos / caseSize * mapColums + xPos / caseSize;

        playerRotation = transform.localRotation.eulerAngles;

        // GRID 2D
        if (movementMode == movementModes.Grid2D)
        {
            AreCasesAccessible(caseNumber);

            // Change enable rotation id different from config
            if (stickRotation != GM.GetRotationStick())
            {
                stickRotation = GM.GetRotationStick();
                SetEnableRotationState(stickRotation);
            }

            cardinalPoint = Mathf.RoundToInt(playerRotation.y / 90);

            angleGap = playerRotation.y - 90 * cardinalPoint;

            // north exception managment (both 0 and 4)
            if (cardinalPoint == 4)
            {
                cardinalPoint = 0;
            }

            if (Mathf.Abs(angleGap) < cardinalpointGap)
            {
                facingCardinalPoint = true;
                UIIG.ShowArrows(casesAccessible, cardinalPoint);
            }
            else
            {
                facingCardinalPoint = false;
                UIIG.HideArrows(cardinalPoint);
            }
        }
    }

    private void FixedUpdate()
    {
        playerPosition = transform.position;

        // movement mode change
        if (newMovementMode != movementMode)
        {
            ChangeMovementMode(false);
        }

        // 2G Grid Movements
        if (movementMode == movementModes.Grid2D)
        {
            if (gridMoveState >= 0)
            {
                GridMove(gridMoveState);
            }

            // to prevent movement bug after dialogues
            SetMoveScaleMultiplier(0f);
        }
    }

    private void ChangeMovementMode(bool fromStart)
    {
        switch (newMovementMode)
        {
            case movementModes.Grid2D:
                MovementModeToGrid2D(fromStart);
                UIIG.ShowHideAllModeUI(false);
                UIIG.ShowHideGrid2DUI(true);
                player.GetComponent<CapsuleCollider>().enabled = false;
                playerController.EnableRotation = stickRotation;
                playerController.SnapRotation = true;
                player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + .5f, player.transform.localPosition.z);
                player.transform.rotation = Quaternion.Euler(0, playerRotation.y, 0);
                break;
            case movementModes.Free2D:
                MovementModeToFree2D();
                UIIG.ShowHideAllModeUI(false);
                player.GetComponent<CapsuleCollider>().enabled = true;
                playerController.EnableRotation = true;
                playerController.SnapRotation = false;
                break;
            case movementModes.Free3D:
                MovementModeToFree3D();
                UIIG.ShowHideAllModeUI(false);
                player.GetComponent<CapsuleCollider>().enabled = true;
                playerController.EnableRotation = true;
                playerController.SnapRotation = false;
                break;
        }

        GM.SetMovementsMode(newMovementMode.ToString());

        // set new mode
        movementMode = newMovementMode;
    }

    private void SetEnableRotationState(bool state)
    {
        playerController.EnableRotation = state;
    }

    // movement mode change methods
    private void MovementModeToGrid2D(bool fromStart)
    {
        // set playermove speed
        SetMoveScaleMultiplier(0.0f);

        // set player jump force
        SetJumpForce(0.0f);

        if (fromStart == false)
        {
            // cause a bug that set the player in water
            //SnapPlayerToGrid();
        }
    }

    private void SnapPlayerToGrid()
    {
        Vector3 newPlayerPosition = transform.position;

        // place the player on the grid
        newPlayerPosition.x = Mathf.Round(playerPosition.x / 10) * 10;
        newPlayerPosition.z = Mathf.Round(playerPosition.z / 10) * 10;

        transform.position = newPlayerPosition;
    }

    private void SnapPlayerToGridAfterMove(Vector3 pos)
    {
        Vector3 newPlayerPosition = pos;

        // place the player on the grid
        newPlayerPosition.x = Mathf.Round(pos.x / 10) * 10;
        newPlayerPosition.z = Mathf.Round(pos.z / 10) * 10;

        transform.position = newPlayerPosition;
    }

    private void MovementModeToFree2D()
    {
        // set playermove speed
        SetMoveScaleMultiplier(2.0f);
        // set player jump force
        SetJumpForce(0.0f);
    }

    private void MovementModeToFree3D()
    {
        // set playermove speed
        SetMoveScaleMultiplier(2.0f);
        // set player jump force
        SetJumpForce(0.6f);
    }

    // set player base speed
    private void SetMoveScaleMultiplier(float speed)
    {
        playerController.SetMoveScaleMultiplier(speed);
    }

    // set player jump force
    private void SetJumpForce(float jumpForce)
    {
        playerController.JumpForce = jumpForce;
    }

    // GRID 2D METHODS
    private void GridMove(int playerDirection)
    {
        if (movementMode == movementModes.Grid2D && facingCardinalPoint == true)
        {
            Vector3 newPlayerPosition = playerPosition;

            playerDirection = playerDirection + cardinalPoint;

            if (playerDirection >= 4)
            {
                playerDirection -= 4;
            }

            switch (playerDirection)
            {
                case 0:
                    if (casesAccessible[0] == true)
                    {
                        newPlayerPosition.z += 10;
                    }
                    break;
                case 1:
                    if (casesAccessible[1] == true)
                    {
                        if (eastSpecial == true)
                        {
                            newPlayerPosition.x += 20;
                        }
                        else
                        {
                            newPlayerPosition.x += 10;
                        }
                    }
                    break;
                case 2:
                    if (casesAccessible[2] == true)
                    {
                        newPlayerPosition.z -= 10;
                    }
                    break;
                case 3:
                    if (casesAccessible[3] == true)
                    {
                        if (westSpecial == true)
                        {
                            newPlayerPosition.x -= 20;
                        }
                        else
                        {
                            newPlayerPosition.x -= 10;
                        }
                    }
                    break;
            }

            transform.position = newPlayerPosition;

            SnapPlayerToGridAfterMove(transform.position);

            gridMoveState = -1;
        }
    }

    private void AreCasesAccessible(int caseNum)
    {
        // accessibility reset
        for (int i = 0; i < casesAccessible.Length; i++)
        {
            casesAccessible[i] = false;
        }

        // each cardinal point check
        // north
        if (caseNum - mapColums >= 0 && mapCasesType[caseNum - mapColums] > 0)
        {
            casesAccessible[0] = true;
            casesAltitudeAccessibility(0, caseNum - mapColums);
        }
        // east
        if (caseNum % mapColums != mapColums - 1 && mapCasesType[caseNum + 1] > 0)
        {
            casesAccessible[1] = true;

            // check for mega jump
            if (mapCasesType[caseNum + 1] == 1000)
            {
                eastSpecial = true;
            }
            else
            {
                eastSpecial = false;
                casesAltitudeAccessibility(1, caseNum + 1);
            }
        }
        // south
        if (caseNum + mapColums < mapNumOfCases && mapCasesType[caseNum + mapColums] > 0)
        {
            casesAccessible[2] = true;
            casesAltitudeAccessibility(2, caseNum + mapColums);
        }
        // west
        if (caseNum - mapColums >= 0 && caseNum % mapColums != 0 && mapCasesType[caseNum - 1] > 0)
        {
            casesAccessible[3] = true;

            // check for mega jump
            if (mapCasesType[caseNum - 1] == 1000)
            {
                westSpecial = true;
            }
            else
            {
                westSpecial = false;
                casesAltitudeAccessibility(3, caseNum - 1);
            }
        }
    }

    // check accessibility regarding the altitude of the cases
    private void casesAltitudeAccessibility(int casesAccessibleID, int casesAccessibleNum)
    {
        int playerCaseType = mapCasesType[caseNumber];
        int playerCaseAltitude = Mathf.FloorToInt(playerCaseType / 100);

        int caseToCheckType = mapCasesType[casesAccessibleNum];
        int caseToCheckAltitude = Mathf.FloorToInt(caseToCheckType / 100);

        if (playerCaseAltitude < caseToCheckAltitude)
        {
            casesAccessible[casesAccessibleID] = false;
        }
    }

    // move forward state when Grid2D
    public void SetGridMoveState(int playerDirection)
    {
        if (facingCardinalPoint == true)
        {
            gridMoveState = playerDirection;
        }
    }

    // PUBLIC METHOD
    // set the new movement mode (applied in Fixed Update)
    public void SetMovementMode(string mode)
    {
        newMovementMode = (movementModes) System.Enum.Parse(typeof(movementModes), mode);
    }

    public string GetMovementMode()
    {
        return movementMode.ToString();
    }

    public float GetCaseSIze()
    {
        return caseSize;
    }

    public int GetCaseNumber()
    {
        return caseNumber;
    }

    public Vector3 GetPlayerRotation()
    {
        return playerRotation;
    }

    public int GetPlayerCaseType()
    {
        return mapCasesType[caseNumber];
    }

    public int GetCaseType(int cardinalPoint, int caseNum, int distance, int gap)
    {
        int caseType = 0;

        switch (cardinalPoint)
        {
            // north
            case 0:
                caseType = mapCasesType[(caseNum - mapColums * distance) + gap];
                break;
            // East
            case 1:
                caseType = mapCasesType[caseNum + distance + gap * mapColums];
                break;
            // South
            case 2:
                caseType = mapCasesType[(caseNum + mapColums * distance) - gap];
                break;
            // West
            case 3:
                caseType = mapCasesType[caseNum - distance - gap * mapColums];
                break;
        }

        return caseType;
    }

    //public void EnablePlayerStickRotation()
    //{
    //    playerController.EnableRotation = currentStickRotation;
    //}

    //public void DisablePlayerStickRotation()
    //{
    //    playerController.EnableRotation = false;
    //}
}
