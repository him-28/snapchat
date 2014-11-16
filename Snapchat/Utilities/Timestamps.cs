// Copyright (c) Matt Saville and Alex Reed. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Snapchat.Utilities
{
	/// <summary>
	/// Provides utility methods used to convert between various timestamp formats.
	/// </summary>
	public static class Timestamps
	{
		/// <summary>
		/// Represents the Unix epoch.
		/// </summary>
		public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);



		/// <summary>
		/// Converts this <see cref="DateTime"/> object into a JScript-based timestamp (milliseconds
		/// since the epoch).
		/// </summary>
		/// <returns>
		/// A 64-bit integer representing the number of milliseconds elapsed since the epoch.
		/// </returns>
		public static long ToJScriptTime(this DateTime value)
		{
			return Convert.ToInt64((value - Epoch).TotalMilliseconds);
		}

		/// <summary>
		/// Converts this <see cref="DateTime"/> object into a Unix-based timestamp (seconds since
		/// the epoch).
		/// </summary>
		/// <returns>
		/// A 32-bit integer representing the number of seconds elapsed since the epoch.
		/// </returns>
		public static int ToUnixTime(this DateTime value)
		{
			return Convert.ToInt32((value - Epoch).TotalSeconds);
		}

		/// <summary>
		/// Converts a JScript-based timestamp (milliseconds since the epoch) into a
		/// <see cref="DateTime"/> object.
		/// </summary>
		/// <returns>
		/// A <see cref="DateTime"/> object representing the timestamp.
		/// </returns>
		public static DateTime FromJScriptTime(long jscriptTime)
		{
			return Epoch.AddMilliseconds(jscriptTime);
		}

		/// <summary>
		/// Converts a Unix-based timestamp (seconds since the epoch) into a <see cref="DateTime"/>
		/// object.
		/// </summary>
		/// <returns>
		/// A <see cref="DateTime"/> object representing the timestamp.
		/// </returns>
		public static DateTime FromUnixTime(int unixTime)
		{
			return Epoch.AddSeconds(unixTime);
		}
	}
}