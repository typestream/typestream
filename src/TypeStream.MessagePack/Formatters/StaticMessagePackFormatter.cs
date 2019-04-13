using System;
using System.Collections.Generic;
using System.Text;
using TypeStream.Abstractions;
using MessagePack;
using System.Linq.Expressions;

namespace TypeStream.MessagePack.Formatters
{
    public class StaticMessagePackFormatter : IFormatter
    {
        private Dictionary<Type, Func<byte[], object>> deserilizersCache;

        public StaticMessagePackFormatter()
        {
            this.deserilizersCache = new Dictionary<Type, Func<byte[], object>>();
        }

        public StaticMessagePackFormatter Register<TValue>()
        {
            var type = typeof(TValue);
            this.deserilizersCache[type] = this.MessagePackDeserialize<TValue>;
            return this;
        }

        private object MessagePackDeserialize<TValue>(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<TValue>(bytes);
        }

        public TValue Deserialize<TValue>(Type type, byte[] bytes)
        {
            if (!this.deserilizersCache.TryGetValue(type, out var deserializer))
            {
                throw new InvalidOperationException($"Type {type.FullName} don't allowed.");
            }

            return (TValue)deserializer(bytes);
        }


        public byte[] Serialize<TValue>(TValue value)
        {
            return MessagePackSerializer.Serialize(value);
        }
    }
}
