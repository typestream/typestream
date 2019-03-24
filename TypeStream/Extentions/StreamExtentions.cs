using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TypeStream.Extentions
{

	public static class StreamExtentions
	{
		public static async Task<int> ReadInt32Async(this Stream stream)
		{
			var buffer = ArrayPool<byte>.Shared.Rent(4);
			await stream.ReadAsync(buffer, 0, 4);
			var value = BitConverter.ToInt32(buffer, 0);

			ArrayPool<byte>.Shared.Return(buffer);
			return value;
		}

		public static async Task WriteAsync(this Stream stream, int value)
		{
			var bytes = BitConverter.GetBytes(value);
			await stream.WriteAsync(bytes, 0, bytes.Length);
		}

		public static async Task<byte[]> ReadByteArrayAsync(this Stream stream)
		{
			var size = await stream.ReadInt32Async();
			var buffer = new byte[size];

			await stream.ReadAsync(buffer, 0, buffer.Length);

			return buffer;
		}

		public static async Task WriteByteArrayAsync(this Stream stream, byte[] bytes)
		{
			await stream.WriteAsync(bytes.Length);
			await stream.WriteAsync(bytes, 0, bytes.Length);
		}

		public static async Task<string> ReadStringAsync(this Stream stream)
		{
			var size = await stream.ReadInt32Async();
			var buffer = new byte[size];

			await stream.ReadAsync(buffer, 0, buffer.Length);

			return Encoding.UTF8.GetString(buffer);
		}

		public static async Task WriteStringAsync(this Stream stream, string text)
		{
			var bytes = Encoding.UTF8.GetBytes(text);
			await stream.WriteByteArrayAsync(bytes);
		}
	}

}
