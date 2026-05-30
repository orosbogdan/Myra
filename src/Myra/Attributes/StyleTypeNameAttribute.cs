using System;

namespace Myra.Attributes
{
	/// <summary>
	/// Attribute used to specify a custom style type name for a class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class StyleTypeNameAttribute: Attribute
	{
		private readonly string _name;

		/// <summary>
		/// Gets the custom style type name.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StyleTypeNameAttribute"/> class with the specified name.
		/// </summary>
		/// <param name="name">The custom style type name.</param>
		public StyleTypeNameAttribute(string name)
		{
			_name = name;
		}
	}
}
