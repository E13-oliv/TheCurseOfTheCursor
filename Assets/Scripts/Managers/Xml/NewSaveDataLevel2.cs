using System.Xml;
using System.Xml.Serialization;

[XmlRoot("save")]
public class NewSaveDataLevel2
{
	// call for the xml structure
	[XmlElement("options")]
	public NewSaveOptionsDataLevel2 newSaveOptionsDataLevel2 = new NewSaveOptionsDataLevel2();

}