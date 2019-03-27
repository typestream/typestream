using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TypeStream.IdGenerators;
using TypeStream.Json.Formatters;
using TypeStream.Tests.Data;
using Xunit;

namespace TypeStream.Tests
{
	public class JsonTypeStreamTest
	{
		private readonly MemoryStream memory;
		private readonly TypeStream stream;

		public JsonTypeStreamTest()
		{
			this.memory = new MemoryStream();
			this.stream = new TypeStream(this.memory, this.memory, new JsonFormatter(), new ByNameIdResolver());
		}

		[Fact]
		public async Task SerializeSimpleType()
		{
			var value = new SimpleClass();
			this.stream.Register<SimpleClass>();

			await this.stream.Write(value);
			this.memory.Seek(0, SeekOrigin.Begin);
			var result = await this.stream.Read<SimpleClass>();

			Assert.Equal(result, value);
		}
	}
}
