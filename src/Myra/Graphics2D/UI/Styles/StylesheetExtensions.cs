using AssetManagementBase;
using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using System;
using System.Collections.Generic;

namespace Myra.Graphics2D.UI.Styles
{
	/// <summary>
	/// Extension methods for stylesheet and style dictionary operations.
	/// </summary>
	internal static class StylesheetExtensions
	{
		/// <summary>
		/// Safely retrieves a style from a style dictionary, throwing an exception if not found.
		/// </summary>
		/// <typeparam name="T">The type of style to retrieve, must derive from WidgetStyle.</typeparam>
		/// <param name="styles">The style dictionary to search.</param>
		/// <param name="id">The style identifier to look up.</param>
		/// <returns>The style with the specified identifier.</returns>
		/// <exception cref="System.Exception">Thrown if the style identifier is not found in the dictionary.</exception>
		public static T SafelyGetStyle<T>(this Dictionary<string, T> styles, string id) where T : WidgetStyle
		{
			T result;
			if (!styles.TryGetValue(id, out result))
			{
				if (id == Stylesheet.DefaultStyleName)
				{
					throw new Exception("Stylesheet doesn't have the default " + typeof(T).Name);
				}
				else
				{
					throw new Exception("Stylesheet lacks the " + typeof(T).Name + " with id '" + id + "'");
				}
			}

			return result;
		}

		public static TextureRegion LoadTextureRegion(this AssetManager assetManager, string assetName)
		{
			if (assetName.Contains(":"))
			{
				// First part is texture region atlas name
				// Second part is texture region name
				var parts = assetName.Split(':');

				var textureRegionAtlas = assetManager.LoadTextureRegionAtlas(parts[0]);
				return textureRegionAtlas[parts[1]];
			}

			if (!assetName.Contains("."))
			{
				// If there's no extension, assume it's a texture region atlas with id equal to the asset name
				var textureRegionAtlas = Stylesheet.Current.Atlas;
				return textureRegionAtlas[assetName];
			}

			// Ordinary texture
#if MONOGAME || FNA || STRIDE
			var texture = assetManager.LoadTexture2D(MyraEnvironment.GraphicsDevice, assetName);
			return new TextureRegion(texture, new Rectangle(0, 0, texture.Width, texture.Height));
#else
			var texture = assetManager.LoadTexture2D(assetName);
			return new TextureRegion(texture.Texture, new Rectangle(0, 0, texture.Width, texture.Height));
#endif
		}

		public static IImage LoadImage(this AssetManager assetManager, string assetName)
		{
			var parts = assetName.Split(TintedImage.Separator);
			Color? color = null;
			if (parts.Length > 1)
			{
				color = ColorStorage.FromName(parts[1]);
			}

			var region = assetManager.LoadTextureRegion(assetName);
			if (color == null)
			{
				return region;
			}

			return new TintedImage(region, color.Value);
		}

		public static IBrush LoadBrush(this AssetManager assetManager, string assetName)
		{
			if (!assetName.Contains(".") && assetName.IndexOf(TintedImage.Separator) == -1 && !assetName.Contains(":"))
			{
				// It's either a default stylesheet texture atlas region or color name
				if (!Stylesheet.Current.Atlas.Regions.TryGetValue(assetName, out var region))
				{
					// Color
					var color = ColorStorage.FromName(assetName);
					if (color == null)
					{
						throw new Exception($"Could not parse brush name '{assetName}'");
					}

					return new SolidBrush(color.Value);
				}
			}

			return assetManager.LoadImage(assetName);
		}

		public static StaticSpriteFont MyraLoadStaticSpriteFont(this AssetManager assetManager, string assetName)
		{
			var fontData = assetManager.ReadAsString(assetName);

			return StaticSpriteFont.FromBMFont(fontData,
						name =>
						{
							var region = assetManager.LoadTextureRegion(name);
							return new TextureWithOffset(region.Texture, region.Bounds.Location);
						});
		}

		public static SpriteFontBase LoadFont(this AssetManager assetManager, string assetName)
		{
			if (!assetName.Contains("."))
			{
				// If there's no extension, assume it's a current stylesheet font
				return Stylesheet.Current.Fonts[assetName];
			}

			if (assetName.Contains(".fnt"))
			{
				return assetManager.MyraLoadStaticSpriteFont(assetName);
			}
			else if (assetName.Contains(".ttf"))
			{

				var parts = assetName.Split(':');
				if (parts.Length < 2)
				{
					throw new Exception("Missing font size");
				}

				var fontSize = int.Parse(parts[1].Trim());
				var fontSystem = assetManager.LoadFontSystem(parts[0].Trim());

				return fontSystem.GetFont(fontSize);
			}

			throw new Exception(string.Format("Can't load font '{0}'", assetName));
		}
	}
}
