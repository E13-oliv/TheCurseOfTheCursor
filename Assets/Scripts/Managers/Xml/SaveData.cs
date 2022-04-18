using System.Xml;
using System.Xml.Serialization;

[XmlRoot("save")]
public class SaveData
{
	// call for the xml structure
	[XmlElement("options")]
	public SaveOptionsData saveOptionsData = new SaveOptionsData();
}