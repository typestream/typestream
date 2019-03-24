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
			if (type.IsGenericType)
			{
				var startIndex = type.Name.IndexOf("`");
				var genericArguments = string.Join(",", type.GenericTypeArguments.Select(o => this.Resolve(o)));
				var fullTypeName = $"{type.Namespace}.{type.Name.Substring(0, startIndex)}<{genericArguments}>";
				return Encoding.UTF8.GetBytes(fullTypeName);
			}

			var typeName = $"{type.Namespace}.{type.Name}";

			return Encoding.UTF8.GetBytes(typeName);
		}
	}
}
