using System.Xml;
using System.Xml.Serialization;

[XmlRoot("mapData")]
public class NewMapData {
    // call for the xml structure
	[XmlElement("options")]
	public NewMapOptionsData newMapOptionsData = new NewMapOptionsData();

}