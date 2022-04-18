using System.Xml;
using System.Xml.Serialization;

public class ConfigOptionsData
{
    // Audio
    [XmlAttribute("musicVolume")]
    public int musicVolume = 4;
    [XmlAttribute("voicesVolume")]
    public int voicesVolume = 5;
    [XmlAttribute("sfxVolume")]
    public int sfxVolume = 4;
    // controls
    [XmlAttribute("rotationStick")]
    public bool rotationStick = false;
}
