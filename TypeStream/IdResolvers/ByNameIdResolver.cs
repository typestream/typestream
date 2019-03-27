using System;
using System.Linq;
using System.Text;
using TypeStream.Core;

namespace TypeStream.IdGenerators
{
	public class ByNameIdResolver : IIdResolver
	{
		public byte[] Resolve(Type type)
		{
			return Encoding.UTF8.GetBytes(ResolveName(type));
		}

		public string ResolveName(Type type)
		{
			if (type.IsGenericType)
			{
				var startIndex = type.Name.IndexOf("`");
				var genericArguments = string.Join(",", type.GenericTypeArguments.Select(o => this.ResolveName(o)));
				return $"{type.Namespace}.{type.Name.Substring(0, startIndex)}<{genericArguments}>";
			}

			return $"{type.Namespace}.{type.Name}";
		}
	}
}
