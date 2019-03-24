using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeStream
{
	public class ByteArrayComparer : IEqualityComparer<byte[]>
	{
		public bool Equals(byte[] left, byte[] right)
		{
			if (left == null || right == null)
			{
				return left == right;
			}

			return left.SequenceEqual(right);
		}

		public int GetHashCode(byte[] key)
		{
			return key.Sum(b => b);
		}
	}
}
