using System;

namespace RethinkDBEnhancements.Attributes
{
	public class RepositoryAttribute : Attribute
	{
		public string Database { get; set; }
		public string Table { get; set; }
	}
}