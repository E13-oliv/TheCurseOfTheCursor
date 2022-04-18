using System.Xml;
using System.Xml.Serialization;

[XmlRoot("save")]
public class NewSaveData
{
	// call for the xml structure
	[XmlElement("options")]
	public NewSaveOptionsData newSaveOptionsData = new NewSaveOptionsData();

}