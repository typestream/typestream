using System;
using System.Collections.Generic;
using System.Text;
using TypeStream.Abstractions;

namespace TypeStream
{
	public class IdCache
	{
		private readonly Dictionary<Type, byte[]> cacheByType;
		private readonly Dictionary<byte[], Type> cacheByName;
		private readonly IIdResolver idResolver;

		public IdCache(IIdResolver idResolver)
		{
			this.cacheByType = new Dictionary<Type, byte[]>();
			this.cacheByName = new Dictionary<byte[], Type>(new ByteArrayComparer());
			this.idResolver = idResolver;
		}

		public byte[] GetId(Type type)
		{
			if (this.cacheByType.TryGetValue(type, out var typeId))
			{
				return typeId;
			}

			ThrowIdNotFound(type);

			return null;
		}

		private static void ThrowIdNotFound(Type type)
		{
			throw new InvalidOperationException($"Type {type.FullName} isn't registered. Have you forget to register a type?");
		}

		private void Cache(Type type, byte[] typeid)
		{
			this.cacheByType.Add(type, typeid);
			this.cacheByName.Add(typeid, type);
		}

		public Type GetTypeById(byte[] typeId)
		{
			if (this.cacheByName.TryGetValue(typeId, out var type))
			{
				return type;
			}

			ThrowTypeNotRegistered(typeId);

			return null;
		}

		private static void ThrowTypeNotRegistered(byte[] typeId)
		{
			throw new InvalidOperationException($"Type { Encoding.UTF8.GetString(typeId)} isn't registered");
		}

		public void Cache<T>()
		{
			var type = typeof(T);
			var typeId = this.idResolver.Resolve(type);
			this.Cache(type, typeId);
		}
	}
}
