using System;
using System.Collections.Generic;
using System.Text;
using TypeStream.IdResolvers;
using TypeStream.Tests.Data;
using Xunit;

namespace TypeStream.Tests.IdResolvers
{
	public class ByNameIdResolverTest
	{
		private const string SIMPLE_CLASS_NAME = "TypeStream.Tests.Data.Event";
		private const string SIMPLE_CLASS_ARRAY_NAME = SIMPLE_CLASS_NAME + "[]";
		private const string IENUMERABLE_NAME = "System.Collections.Generic.IEnumerable<" + SIMPLE_CLASS_NAME + ">";
		private const string GENERIC_TYPE_NAME = "System.Collections.Generic.List<" + SIMPLE_CLASS_NAME + ">";

		private readonly ByNameIdResolver resolver;

		public ByNameIdResolverTest()
		{
			this.resolver = new ByNameIdResolver();
		}

		[Theory]
		[InlineData(SIMPLE_CLASS_NAME, typeof(Event))]
		[InlineData(SIMPLE_CLASS_ARRAY_NAME, typeof(Event[]))]
		[InlineData(GENERIC_TYPE_NAME, typeof(List<Event>))]
		[InlineData(IENUMERABLE_NAME, typeof(IEnumerable<Event>))]
		public void ResolveNameFromType(string name, Type type)
		{
			var bytes = Encoding.UTF8.GetBytes(name);
			var result = this.resolver.Resolve(type);

			Assert.Equal(bytes, result);
		}
	}
}
