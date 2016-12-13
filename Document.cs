using System;
using Newtonsoft.Json;

namespace RethinkDBEnhancements
{
    public abstract class Document
    {
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Guid? Id { get; set; }
	}
}
