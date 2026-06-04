namespace MyraPad.UI
{
	partial class MainForm
	{
		private void AddStylesheetTab()
		{
			if (_tabControlLeft.Items.Contains(_tabStylesheet))
			{
				return;
			}

			_tabControlLeft.Items.Add(_tabStylesheet);
		}

		private void RemoveStylesheetTab()
		{
			_tabControlLeft.Items.Remove(_tabStylesheet);
			_tabControlLeft.SelectedIndex = 0;
		}
	}
}
