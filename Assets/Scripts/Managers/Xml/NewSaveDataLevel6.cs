using System.Xml;
using System.Xml.Serialization;

[XmlRoot("save")]
public class NewSaveDataLevel6
{
	// call for the xml structure
	[XmlElement("options")]
	public NewSaveOptionsDataLevel6 newSaveOptionsDataLevel6 = new NewSaveOptionsDataLevel6();

}