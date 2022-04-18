using System.Xml;
using System.Xml.Serialization;

[XmlRoot("mapData")]
public class NewMapDataCode {
    // call for the xml structure
	[XmlElement("options")]
	public NewMapOptionsDataCode newMapOptionsDataCode = new NewMapOptionsDataCode();

}