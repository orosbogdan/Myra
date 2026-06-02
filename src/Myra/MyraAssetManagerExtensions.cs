using System;
using FontStashSharp;
using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI.Styles;
using Myra.Graphics2D.UI;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using FontStashSharp.RichText;

#if MONOGAME || FNA
using Microsoft.Xna.Framework;
#elif STRIDE
using Stride.Core.Mathematics;
using Texture2D = Stride.Graphics.Texture;
#else
using System.Drawing;
using Texture2D = System.Object;
#endif

namespace AssetManagementBase
{
	/// <summary>
	/// Provides extension methods for the AssetManager class to load Myra-specific assets like texture atlases, fonts, and stylesheets.
	/// </summary>
	public static partial class MyraAssetManagerExtensions
	{
		private static AssetLoader<TextureRegionAtlas> _atlasLoader = (manager, assetName, settings, tag) =>
		{
			var data = manager.ReadAsString(assetName);

#if !PLATFORM_AGNOSTIC
			return TextureRegionAtlas.FromXml(data, name => manager.LoadTexture2D(MyraEnvironment.GraphicsDevice, name, true));
#else
			return TextureRegionAtlas.FromXml(data, name => manager.LoadTexture2D(name).Texture);
#endif
		};

		private static AssetLoader<StaticSpriteFont> _staticFontLoader = (manager, assetName, settings, tag) =>
		{
			var fontData = manager.ReadAsString(assetName);

			return StaticSpriteFont.FromBMFont(fontData,
						name =>
						{
							var region = (TextureRegion)LoadTextureRegion(manager, name);
							return new TextureWithOffset(region.Texture, region.Bounds.Location);
						});
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
		/// Loads a texture region by either image name (e.g., 'image.png') or atlas name/id (e.g., 'atlas.xmat:id').
		/// </summary>
		/// <param name="assetManager">The asset manager instance.</param>
		/// <param name="assetName">The name of the image or atlas region to load.</param>
		/// <returns>The loaded texture region.</returns>
		public static IImage LoadTextureRegion(this AssetManager assetManager, string assetName)
		{
			var parts = assetName.Split(TintedImage.Separator);
			Color? color = null;
			if (parts.Length > 1)
			{
				color = ColorStorage.FromName(parts[1]);
			}

			if (assetName.Contains(":"))
			{
				// First part is texture region atlas name
				// Second part is texture region name
				parts = assetName.Split(':');

				var textureRegionAtlas = assetManager.LoadTextureRegionAtlas(parts[0]);
				var region = textureRegionAtlas[parts[1]];
				if (color == null)
				{
					return region;
				}

				return new TintedImage(region, color.Value);
			}

			if (!assetName.Contains("."))
			{
				// If there's no extension, assume it's a texture region atlas with id equal to the asset name
				var textureRegionAtlas = Stylesheet.Current.Atlas;
				var region = textureRegionAtlas[assetName];

				if (color == null)
				{
					return region;
				}

				return new TintedImage(region, color.Value);
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

			return assetManager.LoadTextureRegion(assetName);
		}

		internal static StaticSpriteFont MyraLoadStaticSpriteFont(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_staticFontLoader, assetName);

		/// <summary>
		/// Loads a sprite font by either TTF name/size (e.g., 'font.ttf:32') or by FNT name (e.g., 'font.fnt').
		/// </summary>
		/// <param name="assetManager">The asset manager instance.</param>
		/// <param name="assetName">The name of the font asset to load.</param>
		/// <returns>The loaded sprite font.</returns>
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

		/// <summary>
		/// Loads a Myra project from an XML asset file.
		/// </summary>
		/// <param name="assetManager">The asset manager instance.</param>
		/// <param name="assetName">The name of the project asset to load.</param>
		/// <returns>The loaded project.</returns>
		public static Project LoadProject(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_projectLoader, assetName);
	}
}
