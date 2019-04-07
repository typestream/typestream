using System;
using System.Collections.Generic;
using System.Text;

namespace TypeStream.Abstractions
{
	public interface IIdResolver
	{
		byte[] Resolve(Type type);
	}
}
