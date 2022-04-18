using System.Xml;
using System.Xml.Serialization;

[XmlRoot("config")]
public class ConfigData {
    // call for the xml structure
	[XmlElement("options")]
	public ConfigOptionsData configOptionsData = new ConfigOptionsData();

}