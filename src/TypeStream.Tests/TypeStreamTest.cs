using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TypeStream.Abstractions;
using TypeStream.IdGenerators;
using TypeStream.Json.Formatters;
using TypeStream.MessagePack.Formatters;
using TypeStream.Tests.Data;
using Xunit;

namespace TypeStream.Tests
{
    public class JsonTypeStreamTest : TypeStreamTest<JsonFormatter, ByNameIdResolver>
    {

    }

    public class MessagePackTypeStreamTest : TypeStreamTest<MessagePackFormatter, ByNameIdResolver>
    {

    }


    public abstract class TypeStreamTest<TFormatter, TIdResolver>
        where TFormatter : IFormatter, new()
        where TIdResolver : IIdResolver, new()
    {
		private readonly MemoryStream memory;
		private readonly TypeStream stream;
        private IIdResolver idResolver;
        private IFormatter formatter;

        public TypeStreamTest()
		{
			this.memory = new MemoryStream();
            this.formatter = new TFormatter();
            this.idResolver = new TIdResolver();
            this.stream = new TypeStream(this.memory, this.memory, this.formatter, this.idResolver);
		}

		[Fact]
		public void FormatterIsRequired()
		{
			Assert.Throws<ArgumentNullException>(() => new TypeStream(null, null, null, this.idResolver));
		}

		[Fact]
		public void IdResolverIsRequired()
		{
			Assert.Throws<ArgumentNullException>(() => new TypeStream(null, null, this.formatter, null));
		}

		[Fact]
		public async Task SerializeSimpleType()
		{
			var value = new SimpleClass();
			await this.SerializeDeserialize(value);
		}

		[Fact]
		public async Task SerializeSimpleTypeWithoutRegistration()
		{
			var value = new SimpleClass();
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await this.stream.Write(value));
		}

		[Fact]
		public async Task SerializeGenericOfSimpleType()
		{
			var value = new List<SimpleClass>
			{
				new SimpleClass(),
				new SimpleClass(),
				new SimpleClass(),
				new SimpleClass()
			};

			await this.SerializeDeserialize(value);
		}

		[Fact]
		public async Task SerializeArrayOfSimpleType()
		{
			var value = new[] 
			{
				new SimpleClass(),
				new SimpleClass(),
				new SimpleClass(),
				new SimpleClass()
			};

			await this.SerializeDeserialize(value);
		}

		private async Task SerializeDeserialize<TValue>(TValue value)
		{
			this.stream.Register<TValue>();

			await this.stream.Write(value);
			this.memory.Seek(0, SeekOrigin.Begin);
			var result = await this.stream.Read<TValue>();

			Assert.Equal(result, value);
		}
	}
}
