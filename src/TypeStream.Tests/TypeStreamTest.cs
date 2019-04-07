using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TypeStream.Abstractions;
using TypeStream.IdResolvers;
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

    public class JsonWithSequentialIdResolverTypeStreamTest : TypeStreamTest<JsonFormatter, SequentialIdResolver>
    {

    }

    public class MessagePackSequentialIdResolverTypeStreamTest : TypeStreamTest<MessagePackFormatter, SequentialIdResolver>
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
			var value = new Event();
			await this.SerializeDeserialize(value);
		}

		[Fact]
		public async Task SerializeSimpleTypeWithoutRegistration()
		{
			var value = new Event();
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await this.stream.Write(value));
		}

		[Fact]
		public async Task SerializeGenericOfSimpleType()
		{
			var value = new List<Event>
			{
				new Event(),
				new Event(),
				new Event(),
				new Event()
			};

			await this.SerializeDeserialize(value);
		}

		[Fact]
		public async Task SerializeArrayOfSimpleType()
		{
			var value = new[] 
			{
				new Event(),
				new Event(),
				new Event(),
				new Event()
			};

			await this.SerializeDeserialize(value);
		}

        [Fact]
        public async Task SerializeConcretTypeDeserializeBaseType()
        {
            this.stream.Register<StartSession>();
            var value = new StartSession();

            await this.stream.Write(value);
            this.memory.Seek(0, SeekOrigin.Begin);
            var result = await this.stream.Read<Event>();

            Assert.Equal(result, value);
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
