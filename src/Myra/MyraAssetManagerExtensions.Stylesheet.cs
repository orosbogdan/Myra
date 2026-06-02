using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI.Styles;
using Myra.MML;
using Myra.Utility;
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

			using (var changer = new StylesheetChanger(result))
			{
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
				var fonts = new Dictionary<string, SpriteFontBase>();
				var fontsNode = xDoc.Root.Element("Fonts");

				var usedSpaceAttr = fontsNode.Attribute("UsedSpace");
				Texture2D existingTexture = null;
				var existingTextureUsedSpace = Rectangle.Empty;
				if (usedSpaceAttr != null)
				{
					var usedSpace = usedSpaceAttr.Value.ParseRectangle();

					existingTexture = result.Atlas.Texture;
					existingTextureUsedSpace = usedSpace;
				}

				foreach (var el in fontsNode.Elements())
				{
					SpriteFontBase font = null;

					var fontFile = el.Attribute("File").Value;
					if (fontFile.EndsWith(".ttf") || fontFile.EndsWith(".otf"))
					{
						var parts = new List<string>()
					{
						fontFile
					};

						var typeAttribute = el.Attribute("Effect");
						if (typeAttribute != null)
						{
							parts.Add(typeAttribute.Value);

							var amountAttribute = el.Attribute("Amount");
							parts.Add(amountAttribute.Value);
						}

						if (el.Attribute("Size") == null)
						{
							throw new Exception($"Can't load stylesheet ttf font '{fontFile}', since Size isn't specified.");
						}

						parts.Add(el.Attribute("Size").Value);
						var fontSystem = manager.LoadFontSystem(fontFile, existingTexture: existingTexture, existingTextureUsedSpace: existingTextureUsedSpace);
						font = fontSystem.GetFont(float.Parse(el.Attribute("Size").Value));
					}
					else if (fontFile.EndsWith(".fnt"))
					{
						font = manager.MyraLoadStaticSpriteFont(fontFile);
					}
					else
					{
						throw new Exception(string.Format("Font '{0}' isn't supported", fontFile));
					}

					fonts[el.Attribute(BaseContext.IdName).Value] = font;
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
				};

				loadContext.Load<object>(result, xDoc.Root, null);

				return result;
			}
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
