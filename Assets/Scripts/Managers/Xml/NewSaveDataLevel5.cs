using System.Xml;
using System.Xml.Serialization;

[XmlRoot("save")]
public class NewSaveDataLevel5
{
	// call for the xml structure
	[XmlElement("options")]
	public NewSaveOptionsDataLevel5 newSaveOptionsDataLevel5 = new NewSaveOptionsDataLevel5();

}