using System.Xml;
using System.Xml.Serialization;

[XmlRoot("save")]
public class NewSaveDataLevel4
{
	// call for the xml structure
	[XmlElement("options")]
	public NewSaveOptionsDataLevel4 newSaveOptionsDataLevel4 = new NewSaveOptionsDataLevel4();

}