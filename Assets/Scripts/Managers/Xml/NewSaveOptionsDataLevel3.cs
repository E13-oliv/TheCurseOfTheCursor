using System.Xml;
using System.Xml.Serialization;

public class NewSaveOptionsDataLevel3
{
    // save file type
    // 0 = no save
    // 1 = code save
    // 2 = player save
    [XmlAttribute("saveType")]
    public int saveType = 0;

    //render mode
    [XmlAttribute("movementsMode")]
    public string movementsMode = "Free2D";

    [XmlAttribute("dimensionsMode")]
    public string dimensionsMode = "Facing2D";

    [XmlAttribute("colorsMode")]
    public string colorsMode = "Colors256";

    [XmlAttribute("audioMode")]
    public string audioMode = "Bit8";

    [XmlAttribute("playerPosition")]
    public float[] playerPosition = new float[] { 67.81f, 12.5f, -146.99f };

    [XmlAttribute("playerRotation")]
    public float[] playerRotation = new float[] { 0f, 270f, 0f };

    // game phases
    // 10 = new game
    // 15 = key found
    // 17 = after talking to buba
    // 20 = end of level 1
    // 30 = start of level 2
    // 49 = end of level 2
    // 50 = start of level 3
    // 69 = end of level 3
    // 70 = start of level 4
    [XmlAttribute("gamePhase")]
    public int gamePhase = 51;

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
    public int[] inventoryDiscovered = new int[] { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // quests state
    // 0 : none
    // 1 : lvl 1 - adventure key
    // 2 : lvl 2 - tetris
    // values:
    // 0 = not started
    // 1 = started
    // 2+ ...
    [XmlAttribute("questsPhase")]
    public int[] questsPhase = new int[] { 0, 999, 999, 999, 999, 4, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 999, 999, 999, 0, 0, 0, 0, 0, 0, 0, 0 };

    [XmlAttribute("logText")]
    public string logText = "Hello little Cursor.\nI am Buba the wise owl and inspector at the NPC Recruiting Office.\nI will help you during your journey  and to succeed in your promotion process.\nYou’ll have to travel through the Video Game History and pass some tests.\nSo first, let me explain how the promotion process works.\nYou are now in the world of the five mythic islands.\nOn every island, you’ll have to find different iconic items and bring them back to me.\nAre you ready for this big journey?\nDon’t be afraid young cursor, I will be here to guide and help you to fulfil your quest.\nBuba thinks: There was really too much text in these old games...\nGo now! Find the key and bring it back to me.\nGreat you found the way to contact me.\nWell Done Young Cursor! I hope it was not too hard to find.\nThis yellow key, from Adventure  on Atari 2600, is one of the first key in video game history.\nIt allowed the player to open the yellow castle.\nUse this key to open the door and meet me on the next island.\nWell done, you reach the BEGINNER level.\nI look wonderful with all these colors. Don’t you think?\nYou can now interact with some items in the world.\nIt will help you to progress through your journey.\nAs the game evolves, you can now choose how you want to see the world.\nYou can change these settings on your RenderBoy.\nBe careful! Mixing settings could sometimes result in a very strange world \nNevertheless, you’ll have to do so to solve some puzzles.\nI bet you’d like to save your progression now...\nSorry, there were no ‘save’ yet at this time in game history.\nBut I offer you this code: 1337.\nYou can use it to restart the game at this level.\nIt’s time to continue your journey,\nSolve 2 puzzles and meet me in the south of this island.\nWow, all the pieces have been scattered.\nSuch a beautifull sand castle, what a pity...\nThere is a button beneath it. *click\nI heard a noise far away.\nHmm. All sand castles have disappeared.\nGreat! You found them both!\nThis piece is the T piece from Tetris .\nHave you ever noticed that all Tetris pieces are made up of 4 squares?\nAs for this sword, it’s the Magic Sword.\nIt’s the most powerful of the first Zelda game.\nBy combining these 2 items, you will be able to access to the next island.\nHe! He! He! Welcome to the 8-bit world!\nI can finally talk to you.\nWe’re done with all these f**** dialog boxes.\nAnother great news, you can now move freely in the world.\nSome areas previously inaccessible should no longer be.\nAs you can see, the world has gained a new dimension as well.\nBut you’ll have to wait a bit to reach these heights.\nFrom this point, you can save your progression at any time accessing your renderboy [Z].\nYou are now at INTERMEDIATE level, one step away from the last island.\nFind the 3 items of this new quest.\nGo Cursor! Catch them all! Oops sorry, wrong game…";
}