using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeStream.Tests.Data
{
    [MessagePackObject]
	public class Event
	{
        [Key(0)]
		public Guid Id { get; set; } = Guid.NewGuid();

        [Key(1)]
		public string TextId { get; set; } = Guid.NewGuid().ToString();

        [Key(2)]
		public int SomeInt32 { get; set; } = 42;

		public override bool Equals(object obj)
		{
			var @class = obj as Event;
			return @class != null &&
				   this.Id.Equals(@class.Id) &&
				   this.TextId == @class.TextId &&
				   this.SomeInt32 == @class.SomeInt32;
		}

        public override int GetHashCode()
        {
            var hashCode = -1168663602;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(this.Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.TextId);
            hashCode = hashCode * -1521134295 + this.SomeInt32.GetHashCode();
            return hashCode;
        }
    }

    [MessagePackObject]
    public class RestoreMessageContainer<TValue> : Event
    {
        [Key(3)]
        public TValue Message { get; set; }
    }

    [MessagePackObject]
    public class StartSession : Event
    {
        [Key(3)]
        public Guid SessionId { get; set; } = Guid.NewGuid();
    }

    [MessagePackObject]
    public class StopSession : Event
    {
        [Key(3)]
        public Guid SessionId { get; set; } = Guid.NewGuid();
    }
}
