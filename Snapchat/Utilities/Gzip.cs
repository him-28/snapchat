// Copyright (c) Matt Saville and Alex Forbes-Reed. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Snapchat.Utilities
{
	/// <summary>
	/// Provides static methods for compressing and decompressing data using gzip.
	/// </summary>
	public static class Gzip
	{
		/// <summary>
		/// Compresses binary data using gzip.
		/// </summary>
		/// <param name="data">
		/// The binary data to compress.
		/// </param>
		/// <returns>
		/// A byte array containing the compressed data.
		/// </returns>
		public static async Task<byte[]> CompressAsync(byte[] data)
		{
			Contract.Requires<ArgumentNullException>(data != null);

			using (var outputStream = new MemoryStream())
			using (var gzip = new GZipStream(outputStream, CompressionMode.Compress))
			{
				await gzip.WriteAsync(data, 0, data.Length);
				return outputStream.ToArray();
			}
		}

		/// <summary>
		/// Compresses a string using gzip.
		/// </summary>
		/// <param name="data">
		/// A string to compress.
		/// </param>
		/// <param name="encoding">
		/// The character encoding of the string.
		/// </param>
		/// <returns>
		/// A byte array containing the compressed data.
		/// </returns>
		public static async Task<byte[]> CompressAsync(string data, Encoding encoding)
		{
			Contract.Requires<ArgumentNullException>(data != null);
			Contract.Requires<ArgumentNullException>(encoding != null);

			return await CompressAsync(encoding.GetBytes(data));
		}

		/// <summary>
		/// Decompresses binary data using gzip.
		/// </summary>
		/// <param name="data">
		/// The compressed data.
		/// </param>
		/// <returns>
		/// A byte array containing the decompressed data.
		/// </returns>
		public static async Task<byte[]> DecompressAsync(byte[] data)
		{
			Contract.Requires<ArgumentNullException>(data != null);

			using (var stream = new MemoryStream(data))
			using (var gzip = new GZipStream(stream, CompressionMode.Decompress))
			using (var outputStream = new MemoryStream())
			{
				await gzip.CopyToAsync(outputStream);
				return outputStream.ToArray();
			}
		}

		/// <summary>
		/// Decompresses binary data into a string.
		/// </summary>
		/// <param name="data">
		/// The compressed data.
		/// </param>
		/// <param name="encoding">
		/// The character encoding of the string.
		/// </param>
		/// <returns>
		/// A string containing the decompressed data.
		/// </returns>
		public static async Task<string> DecompressAsync(byte[] data, Encoding encoding)
		{
			Contract.Requires<ArgumentNullException>(data != null);
			Contract.Requires<ArgumentNullException>(encoding != null);

			return encoding.GetString(await DecompressAsync(data), 0, data.Length);
		}
	}
}