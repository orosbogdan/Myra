using Microsoft.Xna.Framework;
using Myra.Extended.Widgets;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using System;

namespace Myra.Samples.CustomWidgets
{
	class LogViewGame : Game
	{
		private readonly GraphicsDeviceManager _graphics;
		private LogView _logView;
		private DateTime _dt;
		private Desktop _desktop;
		private bool _isPaused = false;

		public LogViewGame()
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

			_desktop = new Desktop();

			var topPanel = new Panel();
			_desktop.Root = topPanel;

			var pauseButton = ToggleButton.CreateTextButton("Pause");
			pauseButton.Click += (s, a) =>
			{
				_isPaused = pauseButton.IsToggled;
			};
			topPanel.Widgets.Add(pauseButton);

			var logViewPanel = new Panel
			{
				Border = new SolidBrush(Color.Blue),
				BorderThickness = new Thickness(2),
				VerticalAlignment = VerticalAlignment.Bottom,
				Height = 300
			};
			topPanel.Widgets.Add(logViewPanel);

			_logView = new LogView();
			logViewPanel.Widgets.Add(_logView);


			_logView.ClearLog();

			_dt = DateTime.Now;
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (_isPaused)
			{
				return;
			}

			var passed = DateTime.Now - _dt;
			if (passed.TotalSeconds < 1.0)
			{
				return;
			}

			var messagesCount = Utility.Random.Next(1, 4);

			var damage = Utility.Random.Next(1, 10);
			_logView.LogFormat(@"/c[lightBlue]Gandalf/c[white] hits /c[green]a kobold/c[white] with his staff for /c[red]{0}/c[white] damage.", damage);

			if (messagesCount > 1)
			{
				damage = Utility.Random.Next(1, 5);
				_logView.LogFormat(@"/c[green]A kobold/c[white] claws /c[lightBlue]Gandalf/c[white] for /c[red]{0}/c[white] damage.", damage);
			}

			if (messagesCount > 2)
			{
				damage = Utility.Random.Next(1, 15);
				_logView.LogFormat(@"/c[lightBlue]Gandalf/c[white] heals himself for /c[lightgreen]{0}/c[white] hit points.", damage);
			}


			_dt = DateTime.Now;
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			GraphicsDevice.Clear(Color.Black);

			_desktop.Render();
		}
	}
}
