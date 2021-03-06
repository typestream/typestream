﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TypeStream.Abstractions
{
	public interface IFormatter
	{
		byte[] Serialize<TValue>(TValue value);

		TValue Deserialize<TValue>(Type type, byte[] bytes);
	}
}
