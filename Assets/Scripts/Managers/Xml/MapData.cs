using System.Xml;
using System.Xml.Serialization;

[XmlRoot("mapData")]
public class MapData {
    // call for the xml structure
	[XmlElement("options")]
	public MapOptionsData mapOptionsData = new MapOptionsData();

}