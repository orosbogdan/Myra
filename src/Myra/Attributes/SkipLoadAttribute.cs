using System;

namespace Myra.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class SkipLoadAttribute: Attribute
	{
	}
}
