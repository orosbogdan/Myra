using AssetManagementBase;
using Myra.Graphics2D.UI.Styles;
using System.Xml.Linq;
using Xunit;

namespace Myra.Tests
{
	[Collection("Myra Tests")]
	public class StylesheetTests
	{
		[Theory]
		[InlineData("Stylesheets/Default/default_ui_skin.xmms")]
		[InlineData("Stylesheets/Default/default_ui_skin_2x.xmms")]
		[InlineData("Stylesheets/Commodore64/ui_stylesheet.xmms")]
		public void XmlRoundTrip(string fileName)
		{
			var assetManager = Utility.CreateAssetManager();

			// Read the original XML string
			var originalXml = assetManager.ReadAsString(fileName);

			// Load the stylesheet from XML
			var stylesheet = assetManager.LoadStylesheet(fileName);

			// Convert back to XML
			var resultXml = stylesheet.ToXml();

			// Parse both as XDocuments and compare the normalized structure
			var originalDoc = XDocument.Parse(originalXml);
			var resultDoc = XDocument.Parse(resultXml);

			// Compare the XML structure (this handles formatting differences)
			Assert.Equal(originalDoc.ToString(), resultDoc.ToString());
		}
	}
}
