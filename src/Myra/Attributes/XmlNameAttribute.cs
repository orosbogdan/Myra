using System;

namespace Myra.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class XmlNameAttribute : Attribute
	{
		public string XmlName { get; }

		public XmlNameAttribute(string xmlName)
		{
			if (string.IsNullOrEmpty(xmlName))
			{
				throw new ArgumentException(nameof(xmlName));
			}

			XmlName = xmlName;
		}
	}
}
