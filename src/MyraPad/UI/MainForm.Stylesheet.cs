using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System.Collections;

namespace MyraPad.UI
{
	partial class MainForm
	{
		private Stylesheet _stylesheetAtStyleTree;

		private bool HasCustomStylesheet => Project != null && !string.IsNullOrEmpty(Project.StylesheetPath);

		private void AddStylesheetTab()
		{
			if (!_tabControlLeft.Items.Contains(_tabStylesheet))
			{
				_tabControlLeft.Items.Add(_tabStylesheet);
			}

			RefreshStyles();
		}

		private void RemoveStylesheetTab()
		{
			_tabControlLeft.Items.Remove(_tabStylesheet);
			_tabControlLeft.SelectedIndex = 0;
		}

		private void RefreshStyles()
		{
			if (Project.Stylesheet == _stylesheetAtStyleTree)
			{
				return;
			}

			_treeViewStylesheet.RemoveAllSubNodes();

			if (Project == null || Project.Stylesheet == null)
			{
				_stylesheetAtStyleTree = Project.Stylesheet;
				return;
			}

			var allProperties = typeof(Stylesheet).GetProperties();
			foreach (var property in allProperties)
			{
				if (property.GetMethod == null ||
					!property.GetMethod.IsPublic ||
					property.GetMethod.IsStatic ||
					!typeof(IDictionary).IsAssignableFrom(property.PropertyType) ||
					!property.Name.EndsWith("Styles"))
				{
					continue;
				}

				var dict = (IDictionary)property.GetValue(Project.Stylesheet);
				if (dict.Count == 0)
				{
					continue;
				}

				var subNode = _treeViewStylesheet.AddSubNode(new Label
				{
					Text = property.Name.Substring(0, property.Name.Length - 6)
				});


				subNode.IsExpanded = true;

				foreach (var key in dict.Keys)
				{
					var name = key.ToString();
					var style = dict[key];
					if (name == Stylesheet.DefaultStyleName)
					{
						subNode.Tag = style;
						continue;
					}

					var styleNode = subNode.AddSubNode(new Label
					{
						Text = name
					});

					styleNode.Tag = style;
				}
			}

			_stylesheetAtStyleTree = Project.Stylesheet;
		}

		private void _treeViewStylesheet_SelectionChanged(object sender, MyraEventArgs e)
		{
			_propertyGrid.Object = _treeViewStylesheet.SelectedNode?.Tag;
		}
	}
}
