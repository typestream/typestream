using System;
using System.Collections.Generic;
using System.Text;
using TypeStream.Abstractions;
using MessagePack;
using System.Linq.Expressions;

namespace TypeStream.MessagePack.Formatters
{
    public class MessagePackFormatter : IFormatter
    {
        private Dictionary<Type, Func<byte[], object>> deserilizersCache;

        public MessagePackFormatter()
        {
            this.deserilizersCache = new Dictionary<Type, Func<byte[], object>>();
        }

        public TValue Deserialize<TValue>(Type type, byte[] bytes)
        {
            if (!this.deserilizersCache.TryGetValue(type, out var deserializer))
            {
                deserializer = this.CacheDeserializer(type);
            }

            return (TValue)deserializer(bytes);
        }

        private Func<byte[], object> CacheDeserializer(Type type)
        {
            var bytesParameter = Expression.Parameter(typeof(byte[]));
            var deserializeMethod = typeof(MessagePackSerializer).GetMethod(nameof(MessagePackSerializer.Deserialize), new[] { typeof(byte[]) })
                .MakeGenericMethod(type);

            var call = Expression.Call(deserializeMethod, bytesParameter);
            var lambda = Expression.Lambda<Func<byte[], object>>(call, bytesParameter).Compile();

            var deserializer = lambda;
            this.deserilizersCache[type] = lambda;
            return deserializer;
        }

        public byte[] Serialize<TValue>(TValue value)
        {
            return MessagePackSerializer.Serialize(value);
        }
    }
}
