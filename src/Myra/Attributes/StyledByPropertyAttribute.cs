using System;

namespace Myra.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class StyledByPropertyAttribute: Attribute
	{
		private readonly string _propertyName;

		public string PropertyName
		{
			get
			{
				return _propertyName;
			}
		}

		public StyledByPropertyAttribute(string propertyName)
		{
			_propertyName = propertyName;
		}
	}
}
