using System;
using System.Collections.Generic;
using System.Text;
using TypeStream.Abstractions;

namespace TypeStream.IdResolvers
{
    public class SequentialIdResolver : IIdResolver
    {
        private Dictionary<Type, byte[]> cache = new Dictionary<Type, byte[]>();
        private int count = 0;

        public byte[] Resolve(Type type)
        {
            if (this.cache.TryGetValue(type, out var bytes))
            {
                return bytes;
            }

            var currentId = this.count++;
            bytes = BitConverter.GetBytes(currentId);
            this.cache.Add(type, bytes);

            return bytes;
        }
    }
}
