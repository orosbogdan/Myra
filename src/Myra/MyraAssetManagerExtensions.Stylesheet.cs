using Myra.Graphics2D.UI.Styles;
using Myra.MML;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace AssetManagementBase
{
	partial class MyraAssetManagerExtensions
	{
		private static AssetLoader<Stylesheet> _stylesheetLoader = (manager, assetName, settings, tag) =>
		{
			var result = new Stylesheet();

			var xmlData = manager.ReadAsString(assetName);
			var xDoc = XDocument.Parse(xmlData);

			// Load atlas
			var attr = xDoc.Root.Attribute("TextureRegionAtlas");
			if (attr == null)
			{
				throw new Exception("Mandatory attribute 'TextureRegionAtlas' doesnt exist");
			}
			result.Atlas = manager.LoadTextureRegionAtlas(attr.Value);

			// Load fonts
			var fonts = new Dictionary<string, StylesheetFont>();
			var fontsNode = xDoc.Root.Element("Fonts");

			foreach (var el in fontsNode.Elements())
			{
				if (el.Attribute(BaseContext.IdName) == null)
				{
					throw new Exception($"Font is missing mandatory 'Id' attribute");
				}
				var key = el.Attribute(BaseContext.IdName).Value;

				if (el.Attribute("File") == null)
				{
					throw new Exception($"Font '{key}' is missing mandatory 'File' attribute");
				}
				var file = el.Attribute("File").Value;

				var font = new StylesheetFont
				{
					Id = key,
					File = file
				};

				if (el.Attribute("Size") != null)
				{
					font.Size = int.Parse(el.Attribute("Size").Value);
				}

				font.Validate();
				font.Font = manager.LoadFont(font.BuildFontFileKey(), null);

				fonts[key] = font;
			}

			result.Fonts = fonts;

			// Load rest
			var loadContext = new LoadContext
			{
				Assemblies = new Dictionary<Assembly, string[]>()
				{
					{ typeof( WidgetStyle ).Assembly, new string[] { typeof( WidgetStyle ).Namespace } }
				},
				AssetManager = manager,
				NodesToIgnore = new HashSet<string>(new[] { "Designer", "Colors", "Fonts" }),
				LegacyClassNames = Stylesheet.LegacyClassNames,
				LegacyPropertyNames = Stylesheet.LegacyPropertyNames,
				DemandContentProperty = false,
				Stylesheet = result
			};

			loadContext.Load(result, xDoc.Root);

			return result;
		};

		/// <summary>
		/// Loads a UI stylesheet from an XML asset file.
		/// </summary>
		/// <param name="assetManager">The asset manager instance.</param>
		/// <param name="assetName">The name of the stylesheet asset to load.</param>
		/// <returns>The loaded stylesheet.</returns>
		public static Stylesheet LoadStylesheet(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_stylesheetLoader, assetName);
	}
}
