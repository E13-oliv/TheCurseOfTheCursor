using System.Xml;
using System.Xml.Serialization;

public class NewSaveOptionsDataLevel2
{
    // save file type
    // 0 = no save
    // 1 = code save
    // 2 = player save
    [XmlAttribute("saveType")]
    public int saveType = 0;

    //render mode
    [XmlAttribute("movementsMode")]
    public string movementsMode = "Grid2D";

    [XmlAttribute("dimensionsMode")]
    public string dimensionsMode = "Flat2DCube";

    [XmlAttribute("colorsMode")]
    public string colorsMode = "Colors16";

    [XmlAttribute("audioMode")]
    public string audioMode = "Beeps";

    [XmlAttribute("playerPosition")]
    public float[] playerPosition = new float[] { 41.1f, 12.5f, -57f };

    [XmlAttribute("playerRotation")]
    public float[] playerRotation = new float[] { 0f, 90f, 0f };

    // GAME PHASES
    // 10 = lvl1_StartOfLevel
    // 11 = lvl1_Talked to Buba
    // 20 = lvl1_EndOfLevel
    // 30 = lvl2_StartOfLevel
    // etc.
    [XmlAttribute("gamePhase")]
    public int gamePhase = 31;

    // inventory objects
    // 0 : none
    // 1 : adventure key
    // 2 : tetris t
    // 3 : magical sword
    // 4 : Sonic Ring
    // 5 : Radio
    // 6 : Rubber Chicken
    // 7 : Super Mushroom
    [XmlAttribute("inventory")]
    public int[] inventory = new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // inventory objects discovered
    // same ids as inventory
    [XmlAttribute("inventoryDiscovered")]
    public int[] inventoryDiscovered = new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // quests state
    // 0 : none
    // 1 : lvl 1 - adventure key
    // 2 : lvl 2 - tetris
    // values:
    // 0 = not started
    // 1 = started
    // 2+ ...
    [XmlAttribute("questsPhase")]
    public int[] questsPhase = new int[] { 0, 999, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 };

    [XmlAttribute("logText")]
    public string logText = "Hello little Cursor.\nI am Buba the wise owl and inspector at the NPC Recruiting Office.\nI will help you during your journey  and to succeed in your promotion process.\nYou’ll have to travel through the Video Game History and pass some tests.\nSo first, let me explain how the promotion process works.\nYou are now in the world of the five mythic islands.\nOn every island, you’ll have to find different iconic items and bring them back to me.\nAre you ready for this big journey?\nDon’t be afraid young cursor, I will be here to guide and help you to fulfil your quest.\nBuba thinks: There was really too much text in these old games...\nGo now! Find the key and bring it back to me.\nGreat you found the way to contact me.\nWell Done Young Cursor! I hope it was not too hard to find.\nThis yellow key, from Adventure  on Atari 2600, is one of the first key in video game history.\nIt allowed the player to open the yellow castle.\nUse this key to open the door and meet me on the next island.\nWell done, you reach the BEGINNER level.\nI look wonderful with all these colors. Don’t you think?\nYou can now interact with some items in the world.\nIt will help you to progress through your journey.\nAs the game evolves, you can now choose how you want to see the world.\nYou can change these settings on your RenderBoy.\nBe careful! Mixing settings could sometimes result in a very strange world \nNevertheless, you’ll have to do so to solve some puzzles.\nI bet you’d like to save your progression now...\nSorry, there were no ‘save’ yet at this time in game history.\nBut I offer you this code: 1337.\nYou can use it to restart the game at this level.\nIt’s time to continue your journey,\nSolve 2 puzzles and meet me in the south of this island.";
}