﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TypeStream.Tests.TestDomain
{
	public class SimpleClass
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string TextId { get; set; } = Guid.NewGuid().ToString();
		public int SomeInt32 { get; set; } = 42;

		public override bool Equals(object obj)
		{
			var @class = obj as SimpleClass;
			return @class != null &&
				   this.Id.Equals(@class.Id) &&
				   this.TextId == @class.TextId &&
				   this.SomeInt32 == @class.SomeInt32;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(this.Id, this.TextId, this.SomeInt32);
		}
	}
}