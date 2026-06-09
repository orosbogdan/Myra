using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI.Properties;
using DigitalRiseModel.Primitives;

namespace Myra.Samples.CustomWidgets
{
	public class Scene3DGame : Game
	{
		private readonly GraphicsDeviceManager _graphics;

		private PropertyGrid _propertyGrid;
		private Desktop _desktop;

		public Scene3DGame()
		{
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1200,
				PreferredBackBufferHeight = 800
			};
			Window.AllowUserResizing = true;

			IsMouseVisible = true;
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			MyraEnvironment.Game = this;

			var scene3d = new Scene3D();
			var scrollViewer = new ScrollViewer
			{
				ShowHorizontalScrollBar = false
			};

			_propertyGrid = new PropertyGrid
			{
				Object = scene3d
			};

			scrollViewer.Content = _propertyGrid;

			var topPanel = new HorizontalSplitPane();


			topPanel.Widgets.Add(scene3d);
			topPanel.Widgets.Add(scrollViewer);

			topPanel.SetSplitterPosition(0, 0.75f);

			_desktop = new Desktop
			{
				Root = topPanel,

				// Inform Myra that external text input is available
				// So it stops translating Keys to chars
				HasExternalTextInput = true
			};

			// Provide that text input
			Window.TextInput += (s, a) =>
			{
				_desktop.OnChar(a.Character);
			};

			var mesh = MeshPrimitives.CreateCapsuleMeshPart(GraphicsDevice, tessellation: 256, uScale: 8, vScale: 8);
			scene3d.Mesh = mesh;
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			GraphicsDevice.Clear(Color.Black);

			_desktop.Render();
		}
	}
}