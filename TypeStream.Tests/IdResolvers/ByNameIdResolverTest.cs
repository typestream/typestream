using System;
using System.Collections.Generic;
using System.Text;
using TypeStream.IdGenerators;
using TypeStream.Tests.Data;
using Xunit;

namespace TypeStream.Tests.IdResolvers
{
	public class ByNameIdResolverTest
	{
		private const string IENUMERABLE_NAME = "System.Collections.Generic.IEnumerable<" + SIMPLE_CLASS_NAME + ">";
		private const string SIMPLE_CLASS_NAME = "TypeStream.Tests.Data.SimpleClass";
		private const string SIMPLE_CLASS_ARRAY_NAME = SIMPLE_CLASS_NAME + "[]";
		private const string GENERIC_TYPE_NAME = "System.Collections.Generic.List<" + SIMPLE_CLASS_NAME + ">";
		private readonly ByNameIdResolver resolver;

		public ByNameIdResolverTest()
		{
			this.resolver = new ByNameIdResolver();
		}

		[Theory]
		[InlineData(SIMPLE_CLASS_NAME, typeof(SimpleClass))]
		[InlineData(SIMPLE_CLASS_ARRAY_NAME, typeof(SimpleClass[]))]
		[InlineData(GENERIC_TYPE_NAME, typeof(List<SimpleClass>))]
		[InlineData(IENUMERABLE_NAME, typeof(IEnumerable<SimpleClass>))]
		public void ResolveNameFromType(string name, Type type)
		{
			var bytes = Encoding.UTF8.GetBytes(name);
			var result = this.resolver.Resolve(type);

			Assert.Equal(bytes, result);
		}
	}
}
