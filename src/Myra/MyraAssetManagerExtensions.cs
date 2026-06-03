using FontStashSharp;
using FontStashSharp.RichText;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System;

#if MONOGAME || FNA
using Microsoft.Xna.Framework;
#elif STRIDE
using Stride.Core.Mathematics;
#else
using System.Drawing;
using Color = FontStashSharp.FSColor;
#endif

namespace AssetManagementBase
{
	/// <summary>
	/// Provides extension methods for the AssetManager class to load Myra-specific assets like texture atlases, fonts, and stylesheets.
	/// </summary>
	public static partial class MyraAssetManagerExtensions
	{
		private static AssetLoader<StaticSpriteFont> _staticFontLoader = (manager, assetName, settings, tag) =>
		{
			var fontData = manager.ReadAsString(assetName);

			var result = StaticSpriteFont.FromBMFont(fontData,
						name =>
						{
							var region = LoadTextureRegion(manager, name);
							return new TextureWithOffset(region.Texture, region.Bounds.Location);
						});

			result.Name = assetName;

			return result;
		};

		private static AssetLoader<TextureRegionAtlas> _atlasLoader = (manager, assetName, settings, tag) =>
		{
			var data = manager.ReadAsString(assetName);

#if !PLATFORM_AGNOSTIC
			var result = TextureRegionAtlas.FromXml(data, name => manager.LoadTexture2D(MyraEnvironment.GraphicsDevice, name, true));
#else
			var result = TextureRegionAtlas.FromXml(data, name => manager.LoadTexture2D(name).Texture);
#endif

			result.Name = assetName;

			return result;
		};

		private static AssetLoader<Project> _projectLoader = (manager, assetName, settings, tag) =>
		{
			var data = manager.ReadAsString(assetName);

			return Project.LoadFromXml(data, manager);
		};

		/// <summary>
		/// Loads a texture region atlas from an XML asset file.
		/// </summary>
		/// <param name="assetManager">The asset manager instance.</param>
		/// <param name="assetName">The name of the atlas asset to load.</param>
		/// <returns>The loaded texture region atlas.</returns>
		public static TextureRegionAtlas LoadTextureRegionAtlas(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_atlasLoader, assetName);

		/// <summary>
		/// Loads a Myra project from an XML asset file.
		/// </summary>
		/// <param name="assetManager">The asset manager instance.</param>
		/// <param name="assetName">The name of the project asset to load.</param>
		/// <returns>The loaded project.</returns>
		public static Project LoadProject(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_projectLoader, assetName);

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
			var result = new TextureRegion(texture, new Rectangle(0, 0, texture.Width, texture.Height));
#else
			var texture = assetManager.LoadTexture2D(assetName);
			var result = new TextureRegion(texture.Texture, new Rectangle(0, 0, texture.Width, texture.Height));
#endif

			result.Name = assetName;

			return result;
		}

		public static IImage LoadImage(this AssetManager assetManager, string assetName)
		{
			var parts = assetName.Split(TintedImage.Separator);
			Color? color = null;
			if (parts.Length > 1)
			{
				color = ColorStorage.FromName(parts[1]);
				assetName = parts[0];
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

		public static StaticSpriteFont MyraLoadStaticSpriteFont(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_staticFontLoader, assetName);

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

				var fontName = parts[0].Trim();
				var fontSystem = assetManager.LoadFontSystem(fontName);

				var result = fontSystem.GetFont(fontSize);

				result.Name = $"{fontName}:{fontSize}";

				return result;
			}

			throw new Exception(string.Format("Can't load font '{0}'", assetName));
		}
	}
}
