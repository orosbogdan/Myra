using AssetManagementBase;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using Myra.MML;
using Xunit;

namespace Myra.Tests
{
	[Collection("Myra Tests")]
	public class MMLTests
	{
		[Fact]
		public void LoadMMLWithExternalAssets()
		{
			var assetManager = AssetManager.CreateResourceAssetManager(Utility.Assembly, "Resources.");

			var project = assetManager.LoadProject("GridWithExternalResources.xmmp");

			var imageButton1 = (Button)project.Root.FindChildById("spawnUnit1");
			Assert.NotNull(imageButton1);

			var image = (Image)imageButton1.Content;
			Assert.NotNull(image);
			Assert.NotNull(image.Renderable);
			Assert.Equal(image.Renderable.Size, new Point(64, 64));

			var label = (Label)project.Root.FindChildById("label");
			Assert.NotNull(label);
			Assert.NotNull(label.Font);
		}

		[Fact]
		public void CheckGridAttachedProperties()
		{
			var properties = AttachedPropertiesRegistry.GetPropertiesOfType(typeof(Grid));
			Assert.Equal(4, properties.Length);
		}
	}
}



