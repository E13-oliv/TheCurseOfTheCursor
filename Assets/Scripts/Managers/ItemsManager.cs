using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [Header("Managers")]
    protected GameManager GM;
    protected AudioManager AM;
    protected WorldManager WM;
    protected InteractiveItemsManager IIM;
    protected MovementsManager MM;
    protected ControlsManager CM;

    // MODES
    protected enum dimensionModes
    {
        None = 0,
        Flat2DCube = 1,
        Facing2D = 2,
        Full3D = 3
    }

    protected enum movementModes
    {
        None = 0,
        Grid2D = 1,
        Free2D = 2,
        Free3D = 3
    }

    protected enum colorsModes
    {
        None = 0,
        BlackAndWhite = 1,
        Colors16 = 2,
        Colors256 = 3,
        ColorsFull = 4
    }

    protected enum audioModes
    {
        None = 0,
        Beeps = 1,
        Bit8 = 2,
        HD = 3,
        HDEnd = 4
    }

    // GAME PHASES
    // 10 = lvl1_StartOfLevel
    // 11 = lvl1_Talked to Buba
    // 20 = lvl1_EndOfLevel
    // 30 = lvl2_StartOfLevel
    // etc.
    protected enum gamePhases
    {
        none = 0,
        _10_StartOfLevel1 = 10,
        _11_L1TalkedToBubaAtStart = 11,
        _20_EndOfLevel1 = 20,
        _30_StartOfLevel2 = 30,
        _31_L2TalkToBubaAtStart = 31,
        _49_EndOfLevel2 = 49,
        _50_StartOfLevel3 = 50,
        _51_L3TalkToBubaAtStart = 51,
        _69_EndOfLevel3 = 60,
        _70_StartOfLevel4 = 70,
        _71_L4TalkToBubaAtStart = 71,
        _999_EndOfTheGame = 999
    }

    // ITEMS
    protected enum itemTypes
    {
        None = 0,
        InteractiveObject = 1,
        Landmark = 2
    }

    protected enum items
    {
        None = 0,
        AdventureKey = 1,
        TetrisTPiece = 2,
        MagicalSword = 3,
        SonicRing = 4,
        Radio = 5,
        RubbuerChicken = 6,
        SuperMushroom = 7,
        PongSet = 8,
        TetrisRocket = 9,
        RaymanGloves = 10,
        MasterSword = 11,
        UltimaMateria = 12
    }

    protected enum landmarks
    {
        None = 0,
        BubaStartOfLevel1 = 1,
        AdventureDoorOpen = 2
    }

    protected enum quests
    {
        None = 0,
        Lvl1_AdventureKey = 1,
        Lvl2_Tetris = 2,
        Lvl2_MagicalSword = 3,
        Lvl2_MagicalSwordLock = 4,
        Lvl2_GreenSandCastle = 29,
        Lvl2_RedSandCastle = 30,
        Lvl2_BlueSandCastle = 31,
        Lvl2_EndOfLevel = 5,
        Lvl3_SonicRing = 6,
        Lvl3_RubberChicken = 7,
        Lvl3_SuperMushroom = 8,
        Lvl3_EndOfLevel = 9,
        Lvl4_PongSet = 10,
        Lvl4_Tetris = 11,
        Lvl4_TetrisPieces = 12,
        LVL4_MasterSword = 13,
        LVL4_MasterSowrdRaymanGloves = 14,
        LVL4_UltimaMateria = 15,
        LVL4_EndOfLevelStele = 16,
        LVL4_Tetris_01 = 17,
        LVL4_Tetris_02 = 18,
        LVL4_Tetris_03 = 19,
        LVL4_Tetris_04 = 20,
        LVL4_Tetris_05 = 21,
        LVL4_Tetris_06 = 22,
        LVL4_Tetris_07 = 23,
        LVL4_Tetris_08 = 24,
        LVL4_Tetris_09 = 25,
        LVL4_Tetris_10 = 26,
        LVL4_Tetris_11 = 27,
        LVL5_Door = 28,
        LVL5_EndOfGame = 32
    }

    // QUESTS PHASES
    // [1] Lvl 1 - Adventure key
    // 1 : start (key pop)
    // 2 : key picked up
    // 3 : talked to Buba
    // 999 : door open
    // [2] Lvl 2 - Tetris
    // 1 : start
    // 3 : tetris t piece pop
    // 999 : tetris t piece taken
    // [3] Lvl 2 - Magical Sword
    // 1 : start
    // 2 : sword unlock
    // 999 : sword taken
    // [4] Lvl 2 - Magical Sword Lock
    // 1 : start
    // 999 : sowrd unlock
    // [5] Lvl 2 - End of level
    // 1 : Buba pop (when 3 and 4 over)
    // 2 : basement pop
    // 3 : tetris t piece placed
    // 999 : sword placed


    // ACTIONS & ACTIONS BUTTONS
    protected enum actions
    {
        None = 0,
        PickUp = 1,
        Use = 2,
        StartCutscene = 3,
        StartFailCutscene = 6,
        Help = 4,
        NewHelp = 5,
        StartEndCutscene = 7
    }

    protected enum actionButtons
    {
        None = 0,
        A = 1,
        B = 2,
        X = 3,
        Y = 4,
        TriggerEnter = 5
    }

    // WORLD CHANGES
    protected enum worldChangeTypes
    {
        None = 0,
        Show = 1,
        Hide = 2,
        Swap = 3
    }

    private void Awake()
    {
        // fing the Managers
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();
        AM = gameManagerGameObject.GetComponent<AudioManager>();
        GameObject worldManagerGameObject = GameObject.Find("WorldManager");
        WM = worldManagerGameObject.GetComponent<WorldManager>();
        CM = worldManagerGameObject.GetComponent<ControlsManager>();
        IIM = worldManagerGameObject.GetComponent<InteractiveItemsManager>();
        GameObject movementManagerGameObject = GameObject.FindWithTag("Player");
        MM = movementManagerGameObject.GetComponent<MovementsManager>();
    }
}
