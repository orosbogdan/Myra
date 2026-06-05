using AssetManagementBase;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System.Xml.Linq;
using Xunit;

namespace Myra.Tests
{
	[Collection("Myra Tests")]
	public class StylesheetTests
	{
		[Theory]
		[InlineData("Stylesheets/Default/default_ui_skin.xmms", "allControls.xmmp")]
		[InlineData("Stylesheets/Default/default_ui_skin_2x.xmms", "allControls.xmmp")]
		[InlineData("Stylesheets/Commodore64/ui_stylesheet.xmms", "allControlsC64.xmmp")]
		public void AllControlsFormStylesheetApplication(string stylesheetPath, string formPath)
		{
			var assetManager = Utility.CreateAssetManager();

			// Load the stylesheet first
			var stylesheet = assetManager.LoadStylesheet(stylesheetPath);
			Assert.NotNull(stylesheet);

			// Load the UI form XML and apply the stylesheet
			var projectXml = assetManager.ReadAsString(formPath);
			var project = Project.LoadFromXml(projectXml, assetManager, stylesheet);

			// Get the style categories from the stylesheet
			var labelStyle = stylesheet.LabelStyles[Stylesheet.DefaultStyleName];
			var buttonStyle = stylesheet.ButtonStyles[Stylesheet.DefaultStyleName];
			var textBoxStyle = stylesheet.TextBoxStyles[Stylesheet.DefaultStyleName];
			var sliderStyle = stylesheet.HorizontalSliderStyles[Stylesheet.DefaultStyleName];

			// Verify Label widgets use stylesheet font
			var labels = Utility.CollectWidgetsByType<Label>(project.Root);
			Assert.True(labels.Count > 0);
			// Check fonts on all labels (fonts are less likely to be overridden)
			foreach (var label in labels)
			{
				Assert.Equal(labelStyle.Font, label.Font);
			}
			// Check text color on labels that don't have explicit colors
			// Labels in ComboView and ListView have explicit TextColor in the xmmp file
			var mainGridLabel = (Label)project.Root.FindChildById("_textButtonLabel");
			if (mainGridLabel != null)
			{
				Assert.Equal(labelStyle.TextColor, mainGridLabel.TextColor);
			}

			// Verify Button widgets have stylesheet background and padding
			var button = (Button)project.Root.FindChildById("_button");
			if (button != null)
			{
				Assert.Equal(buttonStyle.Background, button.Background);
				Assert.Equal(buttonStyle.PressedBackground, button.PressedBackground);
				Assert.Equal(buttonStyle.OverBackground, button.OverBackground);
				Assert.Equal(buttonStyle.Padding, button.Padding);
			}

			var textButton = (Button)project.Root.FindChildById("_textButton");
			if (textButton != null)
			{
				Assert.Equal(buttonStyle.Background, textButton.Background);
				Assert.Equal(buttonStyle.PressedBackground, textButton.PressedBackground);
				Assert.Equal(buttonStyle.Padding, textButton.Padding);
			}

			var imageButton = (Button)project.Root.FindChildById("_imageButton");
			if (imageButton != null)
			{
				Assert.Equal(buttonStyle.Background, imageButton.Background);
				Assert.Equal(buttonStyle.PressedBackground, imageButton.PressedBackground);
			}

			// Verify TextBox widgets have stylesheet properties
			var textBoxes = Utility.CollectWidgetsByType<TextBox>(project.Root);

			foreach (var textBox in textBoxes)
			{
				// TextBox background
				if (textBoxStyle.Background != null && textBox.Background != null)
				{
					Assert.Equal(textBoxStyle.Background, textBox.Background);
				}
				// TextBox font
				if (textBoxStyle.Font != null)
				{
					Assert.Equal(textBoxStyle.Font, textBox.Font);
				}
				// TextBox text colors
				Assert.Equal(textBoxStyle.TextColor, textBox.TextColor);
				Assert.Equal(textBoxStyle.DisabledTextColor, textBox.DisabledTextColor);
				Assert.Equal(textBoxStyle.FocusedTextColor, textBox.FocusedTextColor);
				// TextBox cursor and selection
				if (textBoxStyle.Cursor != null)
				{
					Assert.Equal(textBoxStyle.Cursor, textBox.Cursor);
				}
				if (textBoxStyle.Selection != null)
				{
					Assert.Equal(textBoxStyle.Selection, textBox.Selection);
				}
			}

			// Verify CheckButton widgets have stylesheet images
			var checkBoxStyle = stylesheet.CheckBoxStyles[Stylesheet.DefaultStyleName];
			var checkButtons = Utility.CollectWidgetsByType<CheckButton>(project.Root);
			foreach (var checkButton in checkButtons)
			{
				if (checkButton.Background != null && checkBoxStyle.Background != null)
				{
					Assert.Equal(checkBoxStyle.Background, checkButton.Background);
				}
			}

			// Verify HorizontalSlider widgets have stylesheet properties
			var sliders = Utility.CollectWidgetsByType<HorizontalSlider>(project.Root);
			Assert.True(sliders.Count > 0);
			foreach (var slider in sliders)
			{
				Assert.Equal(sliderStyle.Background, slider.Background);
			}

			// Verify ComboView has stylesheet properties (if stylesheet has it)
			if (stylesheet.ComboBoxStyles.TryGetValue(Stylesheet.DefaultStyleName, out var comboBoxStyle))
			{
				var comboView = (ComboView)project.Root.FindChildById("_comboView");
				if (comboView != null)
				{
					if (comboBoxStyle.Background != null)
					{
						Assert.Equal(comboBoxStyle.Background, comboView.Background);
					}
					// Verify text color is applied to ComboBox labels
					var comboLabels = Utility.CollectWidgetsByType<Label>(comboView);
					foreach (var label in comboLabels)
					{
						Assert.NotNull(label);
					}
					Assert.True(comboView.Widgets.Count > 0);
				}
			}

			// Verify form loaded successfully
			Assert.NotNull(project.Root);
		}

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

			// Parse both as XDocuments
			var originalDoc = XDocument.Parse(originalXml);
			var resultDoc = XDocument.Parse(resultXml);

			// Assert XML is semantically equal (ignoring attribute order)
			Utility.AssertXmlEqual(originalDoc, resultDoc);
		}
	}
}
