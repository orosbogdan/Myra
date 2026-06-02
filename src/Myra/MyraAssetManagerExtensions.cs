using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;

#if MONOGAME || FNA
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
	}
}
