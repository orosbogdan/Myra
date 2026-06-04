using System.IO;
using System.Reflection;
using System;
using Myra.Graphics2D.UI;
using Myra.Graphics2D;
using Microsoft.Xna.Framework;
using Xunit;
using Myra.Graphics2D.Brushes;
using FontStashSharp.RichText;
using AssetManagementBase;

namespace Myra.Tests
{
	internal static class Utility
	{
		public static readonly Assembly Assembly = typeof(Utility).Assembly;

		public static Widget LoadProjectRootClone(string name)
		{
			var assetManager = CreateAssetManager();
			var project = assetManager.LoadProject(name);

			return project.Root.Clone();
		}

		/// <summary>
		/// Creates an AssetManager that loads files from the Assets folder in the test output directory.
		/// </summary>
		/// <returns>A file-based AssetManager pointing to the Assets folder.</returns>
		public static AssetManager CreateAssetManager()
		{
			var assetPath = Path.Combine(Path.GetDirectoryName(Assembly.Location), "Assets");
			return AssetManager.CreateFileAssetManager(assetPath);
		}

		public static void AssertSolidBrush(Color color, IBrush brush)
		{
			Assert.IsType<SolidBrush>(brush);
			var solidBrush = (SolidBrush)brush;

			Assert.Equal(color, solidBrush.Color);
		}

		public static void AssertSolidBrush(string colorName, IBrush brush)
		{
			var color = ColorStorage.FromName(colorName);
			AssertSolidBrush(color.Value, brush);
		}

		public static void AssertColor(string colorName, Color color)
		{
			var color2 = ColorStorage.FromName(colorName);
			Assert.Equal(color2.Value, color);
		}
	}
}
