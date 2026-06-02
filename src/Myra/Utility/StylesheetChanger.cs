using Myra.Graphics2D.UI.Styles;
using System;

namespace Myra.Utility
{
	// RAII pattern utility: temporarily changes stylesheet and restores on Dispose
	// Used to apply a specific stylesheet during loading/saving without permanently changing global stylesheet
	internal struct StylesheetChanger : IDisposable
	{
		private readonly Stylesheet _oldStylesheet;

		public StylesheetChanger(Stylesheet newStylesheet)
		{
			_oldStylesheet = Stylesheet._current;
			Stylesheet.Current = newStylesheet;
		}

		public void Dispose()
		{
			Stylesheet.Current = _oldStylesheet;
		}
	}
}
