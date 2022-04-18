using System.Xml;
using System.Xml.Serialization;

[XmlRoot("save")]
public class NewSaveDataLevel3
{
	// call for the xml structure
	[XmlElement("options")]
	public NewSaveOptionsDataLevel3 newSaveOptionsDataLevel3 = new NewSaveOptionsDataLevel3();

}