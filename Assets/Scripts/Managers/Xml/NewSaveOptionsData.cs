using System.Xml;
using System.Xml.Serialization;

public class NewSaveOptionsData
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
    public string colorsMode = "BlackAndWhite";

    [XmlAttribute("audioMode")]
    public string audioMode = "Beeps";

    // FOR DEBUG PURPOSE
    //[XmlAttribute("movementsMode")]
    //public string movementsMode = "Free3D";

    //[XmlAttribute("dimensionsMode")]
    //public string dimensionsMode = "Full3D";

    //[XmlAttribute("colorsMode")]
    //public string colorsMode = "ColorsFull";

    //[XmlAttribute("audioMode")]
    //public string audioMode = "HD";


    [XmlAttribute("playerPosition")]
    public float[] playerPosition = new float[] { -27f, 12.5f, -25f };

    [XmlAttribute("playerRotation")]
    public float[] playerRotation = new float[] { 0f, 0f, 0f };

    // GAME PHASES
    // 10 = lvl1_StartOfLevel
    // 11 = lvl1_Talked to Buba
    // 20 = lvl1_EndOfLevel
    // 30 = lvl2_StartOfLevel
    // etc.
    [XmlAttribute("gamePhase")]
    public int gamePhase = 10;

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
    public int[] inventory = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // inventory objects discovered
    // same ids as inventory
    [XmlAttribute("inventoryDiscovered")]
    public int[] inventoryDiscovered = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // quests state
    // 0 : none
    // 1 : lvl 1 - adventure key
    // 2 : lvl 2 - tetris
    // values:
    // 0 = not started
    // 1 = started
    // 2+ ...
    [XmlAttribute("questsPhase")]
    public int[] questsPhase = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 };

    [XmlAttribute("logText")]
    public string logText = "";
}