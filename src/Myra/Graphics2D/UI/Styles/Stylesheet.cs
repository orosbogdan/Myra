using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Linq;
using Myra.MML;
using System.Collections;
using FontStashSharp;
using Myra.Graphics2D.TextureAtlases;
using System.ComponentModel;
using Myra.Attributes;



#if MONOGAME || FNA
using Microsoft.Xna.Framework;
#elif STRIDE
using Stride.Core.Mathematics;
#else
using Color = FontStashSharp.FSColor;
using SolidBrush = Myra.Graphics2D.Brushes.SolidBrush;
#endif

namespace Myra.Graphics2D.UI.Styles
{
	/// <summary>
	/// Manages a collection of UI styles used throughout the application.
	/// Provides access to default styles and named style variants for all UI widgets.
	/// </summary>
	public class Stylesheet
	{
		internal static readonly Dictionary<string, string> LegacyClassNames = new Dictionary<string, string>();
		internal static readonly Dictionary<string, string> LegacyPropertyNames = new Dictionary<string, string>();

		/// <summary>
		/// The default style identifier used when no specific style name is provided.
		/// </summary>
		public const string DefaultStyleName = "";

		internal static Stylesheet _current;

		/// <summary>
		/// Gets or sets the current active stylesheet used globally.
		/// If not explicitly set, returns the default stylesheet from DefaultAssets.
		/// </summary>
		public static Stylesheet Current
		{
			get
			{
				if (_current == null)
				{
					_current = DefaultAssets.DefaultStylesheet;
				}

				return _current;
			}

			set
			{
				_current = value;
			}
		}

		private readonly Dictionary<string, LabelStyle> _labelStyles = new Dictionary<string, LabelStyle>();
		private readonly Dictionary<string, LabelStyle> _tooltipStyles = new Dictionary<string, LabelStyle>();
		private readonly Dictionary<string, TextBoxStyle> _textBoxStyles = new Dictionary<string, TextBoxStyle>();
		private readonly Dictionary<string, ButtonStyle> _buttonStyles = new Dictionary<string, ButtonStyle>();
		private readonly Dictionary<string, ImageTextButtonStyle> _checkBoxStyles = new Dictionary<string, ImageTextButtonStyle>();
		private readonly Dictionary<string, ImageTextButtonStyle> _radioButtonStyles = new Dictionary<string, ImageTextButtonStyle>();
		private readonly Dictionary<string, SpinButtonStyle> _spinButtonStyles = new Dictionary<string, SpinButtonStyle>();
		private readonly Dictionary<string, SliderStyle> _horizontalSliderStyles = new Dictionary<string, SliderStyle>();
		private readonly Dictionary<string, SliderStyle> _verticalSliderStyles = new Dictionary<string, SliderStyle>();
		private readonly Dictionary<string, ProgressBarStyle> _horizontalProgressBarStyles =
			new Dictionary<string, ProgressBarStyle>();
		private readonly Dictionary<string, ProgressBarStyle> _verticalProgressBarStyles =
			new Dictionary<string, ProgressBarStyle>();
		private readonly Dictionary<string, SeparatorStyle> _horizontalSeparatorStyles =
			new Dictionary<string, SeparatorStyle>();
		private readonly Dictionary<string, SeparatorStyle> _verticalSeparatorStyles =
			new Dictionary<string, SeparatorStyle>();
		private readonly Dictionary<string, ComboBoxStyle> _comboBoxStyles = new Dictionary<string, ComboBoxStyle>();
		private readonly Dictionary<string, ListBoxStyle> _listBoxStyles = new Dictionary<string, ListBoxStyle>();
		private readonly Dictionary<string, TabControlStyle> _tabControlStyles = new Dictionary<string, TabControlStyle>();
		private readonly Dictionary<string, TreeStyle> _treeStyles = new Dictionary<string, TreeStyle>();
		private readonly Dictionary<string, SplitPaneStyle> _horizontalSplitPaneStyles =
			new Dictionary<string, SplitPaneStyle>();
		private readonly Dictionary<string, SplitPaneStyle> _verticalSplitPaneStyles =
			new Dictionary<string, SplitPaneStyle>();
		private readonly Dictionary<string, ScrollViewerStyle> _scrollViewerStyles = new Dictionary<string, ScrollViewerStyle>();
		private readonly Dictionary<string, MenuStyle> _horizontalMenuStyles = new Dictionary<string, MenuStyle>();
		private readonly Dictionary<string, MenuStyle> _verticalMenuStyles = new Dictionary<string, MenuStyle>();
		private readonly Dictionary<string, WindowStyle> _windowStyles = new Dictionary<string, WindowStyle>();
		private readonly Dictionary<string, FileDialogStyle> _fileDialogStyles = new Dictionary<string, FileDialogStyle>();
		private readonly Dictionary<string, ColorPickerDialogStyle> _colorPickerDialogStyles = new Dictionary<string, ColorPickerDialogStyle>();

		private TextureRegion _whiteRegion;

		/// <summary>
		/// Gets the texture atlas containing all texture regions used in the stylesheet.
		/// Skip load, since it is loaded manually
		/// </summary>
		[XmlName("TextureRegionAtlas")]
		[SkipLoad]
		public TextureRegionAtlas Atlas { get; internal set; }

		/// <summary>
		/// Gets a white texture region from the atlas, used for solid color rendering.
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public TextureRegion WhiteRegion
		{
			get
			{
				if (Atlas == null)
				{
					// Since we switch atlases sometimes
					// In async context, Atlas could be null at the moment of access, so we need to check it every time
					return DefaultAssets.WhiteRegion;
				}

				if (_whiteRegion == null)
				{
					if (!Atlas.Regions.TryGetValue("white", out _whiteRegion))
					{
						_whiteRegion = DefaultAssets.WhiteRegion;
					}
				}

				return _whiteRegion;
			}
		}

		/// <summary>
		/// Gets the dictionary of fonts available in this stylesheet, keyed by font name.
		/// Skip load, since it is loaded manually
		/// </summary>
		[SkipLoad]
		public Dictionary<string, SpriteFontBase> Fonts { get; internal set; }

		/// <summary>
		/// Gets or sets the style applied to the desktop background.
		/// </summary>
		public DesktopStyle DesktopStyle { get; set; }

		/// <summary>
		/// Gets or sets the default style for label widgets.
		/// </summary>
		[XmlIgnore]
		public LabelStyle LabelStyle
		{
			get => GetDefaultStyle(_labelStyles);
			set => SetDefaultStyle(_labelStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for tooltip labels.
		/// </summary>
		[XmlIgnore]
		public LabelStyle TooltipStyle
		{
			get => GetDefaultStyle(_tooltipStyles);
			set => SetDefaultStyle(_tooltipStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for text box widgets.
		/// </summary>
		[XmlIgnore]
		public TextBoxStyle TextBoxStyle
		{
			get => GetDefaultStyle(_textBoxStyles);
			set => SetDefaultStyle(_textBoxStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for button widgets.
		/// </summary>
		[XmlIgnore]
		public ButtonStyle ButtonStyle
		{
			get => GetDefaultStyle(_buttonStyles);
			set => SetDefaultStyle(_buttonStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for checkbox widgets.
		/// </summary>
		[XmlIgnore]
		public ImageTextButtonStyle CheckBoxStyle
		{
			get => GetDefaultStyle(_checkBoxStyles);
			set => SetDefaultStyle(_checkBoxStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for radio button widgets.
		/// </summary>
		[XmlIgnore]
		public ImageTextButtonStyle RadioButtonStyle
		{
			get => GetDefaultStyle(_radioButtonStyles);
			set => SetDefaultStyle(_radioButtonStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for spin button widgets.
		/// </summary>
		[XmlIgnore]
		public SpinButtonStyle SpinButtonStyle
		{
			get => GetDefaultStyle(_spinButtonStyles);
			set => SetDefaultStyle(_spinButtonStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for horizontal slider widgets.
		/// </summary>
		[XmlIgnore]
		public SliderStyle HorizontalSliderStyle
		{
			get => GetDefaultStyle(_horizontalSliderStyles);
			set => SetDefaultStyle(_horizontalSliderStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for vertical slider widgets.
		/// </summary>
		[XmlIgnore]
		public SliderStyle VerticalSliderStyle
		{
			get => GetDefaultStyle(_verticalSliderStyles);
			set => SetDefaultStyle(_verticalSliderStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for horizontal progress bar widgets.
		/// </summary>
		[XmlIgnore]
		public ProgressBarStyle HorizontalProgressBarStyle
		{
			get => GetDefaultStyle(_horizontalProgressBarStyles);
			set => SetDefaultStyle(_horizontalProgressBarStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for vertical progress bar widgets.
		/// </summary>
		[XmlIgnore]
		public ProgressBarStyle VerticalProgressBarStyle
		{
			get => GetDefaultStyle(_verticalProgressBarStyles);
			set => SetDefaultStyle(_verticalProgressBarStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for horizontal separator widgets.
		/// </summary>
		[XmlIgnore]
		public SeparatorStyle HorizontalSeparatorStyle
		{
			get => GetDefaultStyle(_horizontalSeparatorStyles);
			set => SetDefaultStyle(_horizontalSeparatorStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for vertical separator widgets.
		/// </summary>
		[XmlIgnore]
		public SeparatorStyle VerticalSeparatorStyle
		{
			get => GetDefaultStyle(_verticalSeparatorStyles);
			set => SetDefaultStyle(_verticalSeparatorStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for combo box widgets.
		/// </summary>
		[XmlIgnore]
		public ComboBoxStyle ComboBoxStyle
		{
			get => GetDefaultStyle(_comboBoxStyles);
			set => SetDefaultStyle(_comboBoxStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for list box widgets.
		/// </summary>
		[XmlIgnore]
		public ListBoxStyle ListBoxStyle
		{
			get => GetDefaultStyle(_listBoxStyles);
			set => SetDefaultStyle(_listBoxStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for tab control widgets.
		/// </summary>
		[XmlIgnore]
		public TabControlStyle TabControlStyle
		{
			get => GetDefaultStyle(_tabControlStyles);
			set => SetDefaultStyle(_tabControlStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for tree widgets.
		/// </summary>
		[XmlIgnore]
		public TreeStyle TreeStyle
		{
			get => GetDefaultStyle(_treeStyles);
			set => SetDefaultStyle(_treeStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for horizontal split pane widgets.
		/// </summary>
		[XmlIgnore]
		public SplitPaneStyle HorizontalSplitPaneStyle
		{
			get => GetDefaultStyle(_horizontalSplitPaneStyles);
			set => SetDefaultStyle(_horizontalSplitPaneStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for vertical split pane widgets.
		/// </summary>
		[XmlIgnore]
		public SplitPaneStyle VerticalSplitPaneStyle
		{
			get => GetDefaultStyle(_verticalSplitPaneStyles);
			set => SetDefaultStyle(_verticalSplitPaneStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for scroll viewer widgets.
		/// </summary>
		[XmlIgnore]
		public ScrollViewerStyle ScrollViewerStyle
		{
			get => GetDefaultStyle(_scrollViewerStyles);
			set => SetDefaultStyle(_scrollViewerStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for horizontal menu widgets.
		/// </summary>
		[XmlIgnore]
		public MenuStyle HorizontalMenuStyle
		{
			get => GetDefaultStyle(_horizontalMenuStyles);
			set => SetDefaultStyle(_horizontalMenuStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for vertical menu widgets.
		/// </summary>
		[XmlIgnore]
		public MenuStyle VerticalMenuStyle
		{
			get => GetDefaultStyle(_verticalMenuStyles);
			set => SetDefaultStyle(_verticalMenuStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for window widgets.
		/// </summary>
		[XmlIgnore]
		public WindowStyle WindowStyle
		{
			get => GetDefaultStyle(_windowStyles);
			set => SetDefaultStyle(_windowStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for file dialog widgets.
		/// </summary>
		[XmlIgnore]
		public FileDialogStyle FileDialogStyle
		{
			get => GetDefaultStyle(_fileDialogStyles);
			set => SetDefaultStyle(_fileDialogStyles, value);
		}

		/// <summary>
		/// Gets or sets the default style for color picker dialog widgets.
		/// </summary>
		[XmlIgnore]
		public ColorPickerDialogStyle ColorPickerDialogStyle
		{
			get => GetDefaultStyle(_colorPickerDialogStyles);
			set => SetDefaultStyle(_colorPickerDialogStyles, value);
		}

		/// <summary>
		/// Gets the dictionary of named label styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, LabelStyle> LabelStyles => _labelStyles;

		/// <summary>
		/// Gets the dictionary of named tooltip label styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, LabelStyle> TooltipStyles => _tooltipStyles;

		/// <summary>
		/// Gets the dictionary of named text box styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, TextBoxStyle> TextBoxStyles => _textBoxStyles;

		/// <summary>
		/// Gets the dictionary of named button styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, ButtonStyle> ButtonStyles => _buttonStyles;

		/// <summary>
		/// Gets the dictionary of named checkbox styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, ImageTextButtonStyle> CheckBoxStyles => _checkBoxStyles;

		/// <summary>
		/// Gets the dictionary of named radio button styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, ImageTextButtonStyle> RadioButtonStyles => _radioButtonStyles;

		/// <summary>
		/// Gets the dictionary of named spin button styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, SpinButtonStyle> SpinButtonStyles => _spinButtonStyles;

		/// <summary>
		/// Gets the dictionary of named horizontal slider styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, SliderStyle> HorizontalSliderStyles => _horizontalSliderStyles;

		/// <summary>
		/// Gets the dictionary of named vertical slider styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, SliderStyle> VerticalSliderStyles => _verticalSliderStyles;

		/// <summary>
		/// Gets the dictionary of named horizontal progress bar styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, ProgressBarStyle> HorizontalProgressBarStyles => _horizontalProgressBarStyles;

		/// <summary>
		/// Gets the dictionary of named vertical progress bar styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, ProgressBarStyle> VerticalProgressBarStyles => _verticalProgressBarStyles;

		/// <summary>
		/// Gets the dictionary of named horizontal separator styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, SeparatorStyle> HorizontalSeparatorStyles => _horizontalSeparatorStyles;

		/// <summary>
		/// Gets the dictionary of named vertical separator styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, SeparatorStyle> VerticalSeparatorStyles => _verticalSeparatorStyles;

		/// <summary>
		/// Gets the dictionary of named combo box styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, ComboBoxStyle> ComboBoxStyles => _comboBoxStyles;

		/// <summary>
		/// Gets the dictionary of named list box styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, ListBoxStyle> ListBoxStyles => _listBoxStyles;

		/// <summary>
		/// Gets the dictionary of named tab control styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, TabControlStyle> TabControlStyles => _tabControlStyles;

		/// <summary>
		/// Gets the dictionary of named tree styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, TreeStyle> TreeStyles => _treeStyles;

		/// <summary>
		/// Gets the dictionary of named horizontal split pane styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, SplitPaneStyle> HorizontalSplitPaneStyles => _horizontalSplitPaneStyles;

		/// <summary>
		/// Gets the dictionary of named vertical split pane styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, SplitPaneStyle> VerticalSplitPaneStyles => _verticalSplitPaneStyles;

		/// <summary>
		/// Gets the dictionary of named scroll viewer styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, ScrollViewerStyle> ScrollViewerStyles => _scrollViewerStyles;

		/// <summary>
		/// Gets the dictionary of named horizontal menu styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, MenuStyle> HorizontalMenuStyles => _horizontalMenuStyles;

		/// <summary>
		/// Gets the dictionary of named vertical menu styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, MenuStyle> VerticalMenuStyles => _verticalMenuStyles;

		/// <summary>
		/// Gets the dictionary of named window styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, WindowStyle> WindowStyles => _windowStyles;

		/// <summary>
		/// Gets the dictionary of named file dialog styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, FileDialogStyle> FileDialogStyles => _fileDialogStyles;

		/// <summary>
		/// Gets the dictionary of named color picker dialog styles, keyed by style identifier.
		/// </summary>
		public Dictionary<string, ColorPickerDialogStyle> ColorPickerDialogStyles => _colorPickerDialogStyles;

		static Stylesheet()
		{
			LegacyClassNames["TextBlockStyle"] = "LabelStyle";
			LegacyClassNames["TextFieldStyle"] = "TextBoxStyle";
			LegacyClassNames["ScrollPaneStyle"] = "ScrollViewerStyle";

			LegacyPropertyNames["TextBlockStyle"] = "LabelStyle";
			LegacyPropertyNames["TextFieldStyle"] = "TextBoxStyle";
			LegacyPropertyNames["ScrollPaneStyle"] = "ScrollViewerStyle";
			LegacyPropertyNames["TextBlockStyles"] = "LabelStyles";
			LegacyPropertyNames["TextFieldStyles"] = "TextBoxStyles";
			LegacyPropertyNames["ScrollPaneStyles"] = "ScrollViewerStyles";
		}

		private static T GetDefaultStyle<T>(Dictionary<string, T> styles) where T : WidgetStyle
		{
			T result = null;
			if (!styles.TryGetValue(DefaultStyleName, out result))
			{
				throw new Exception("Stylesheet doesnt define default style for " + typeof(T).Name + ".");
			}

			return result;
		}

		private static void SetDefaultStyle<T>(Dictionary<string, T> styles, T value) where T : WidgetStyle
		{
			styles[DefaultStyleName] = value;
		}

		public string ToXml()
		{
			var saveContext = new SaveContext();
			saveContext.PrependNamespace = false;

			var root = saveContext.Save(this);

			var xDoc = new XDocument(root);

			return xDoc.ToString();
		}

		/// <summary>
		/// Gets an array of style names available for the specified widget type.
		/// </summary>
		/// <param name="name">The widget type name (e.g., "Button", "Label", "TextBox").</param>
		/// <returns>An array of style identifiers for the widget type, or null if the widget type has no styles defined.</returns>
		public string[] GetStylesByWidgetName(string name)
		{
			// Special case
			if (name.Contains("Button"))
			{
				name = "Button";
			}

			var propertyName = name + "Styles";
			var property = GetType().GetProperty(propertyName);
			if (property == null)
			{
				return null;
			}

			var dict = (IDictionary)property.GetValue(this);

			var result = new List<string>();
			foreach (var k in dict.Keys)
			{
				result.Add((string)k);
			}

			return result.ToArray();
		}

		private void CloneStylesTo<T>(Stylesheet destStylesheet, Func<Stylesheet, Dictionary<string, T>> stylesGetter) where T : WidgetStyle
		{
			var src = stylesGetter(this);
			var dest = stylesGetter(destStylesheet);

			dest.Clear();
			foreach (var pair in src)
			{
				dest[pair.Key] = (T)pair.Value.Clone();
			}
		}

		/// <summary>
		/// Creates a deep copy of this stylesheet including all styles and fonts.
		/// </summary>
		/// <returns>A new Stylesheet instance with cloned styles and fonts.</returns>
		public Stylesheet Clone()
		{
			var result = new Stylesheet
			{
				Atlas = Atlas,
				Fonts = new Dictionary<string, SpriteFontBase>()
			};

			// Clone all dictionary properties
			CloneStylesTo(result, s => s.HorizontalSliderStyles);
			CloneStylesTo(result, s => s.VerticalSliderStyles);
			CloneStylesTo(result, s => s.HorizontalProgressBarStyles);
			CloneStylesTo(result, s => s.VerticalProgressBarStyles);
			CloneStylesTo(result, s => s.HorizontalSeparatorStyles);
			CloneStylesTo(result, s => s.VerticalSeparatorStyles);
			CloneStylesTo(result, s => s.HorizontalSplitPaneStyles);
			CloneStylesTo(result, s => s.VerticalSplitPaneStyles);
			CloneStylesTo(result, s => s.HorizontalMenuStyles);
			CloneStylesTo(result, s => s.VerticalMenuStyles);

			CloneStylesTo(result, s => s.LabelStyles);
			CloneStylesTo(result, s => s.TextBoxStyles);
			CloneStylesTo(result, s => s.ButtonStyles);
			CloneStylesTo(result, s => s.CheckBoxStyles);
			CloneStylesTo(result, s => s.RadioButtonStyles);
			CloneStylesTo(result, s => s.SpinButtonStyles);
			CloneStylesTo(result, s => s.ComboBoxStyles);
			CloneStylesTo(result, s => s.ListBoxStyles);
			CloneStylesTo(result, s => s.TabControlStyles);
			CloneStylesTo(result, s => s.TreeStyles);
			CloneStylesTo(result, s => s.ScrollViewerStyles);
			CloneStylesTo(result, s => s.WindowStyles);

			foreach (var pair in Fonts)
			{
				result.Fonts[pair.Key] = pair.Value;
			}

			return result;
		}
	}
}