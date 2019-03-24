using System;
using System.Collections.Generic;
using System.Text;

namespace TypeStream.Core
{
	public interface IIdResolver
	{
		byte[] Resolve(Type type);
	}
}
