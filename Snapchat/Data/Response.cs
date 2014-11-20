// Copyright (c) Matt Saville and Alex Forbes-Reed. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Snapchat.Data
{
	/// <summary>
	/// Represents a basic response from the Snapchat API.
	/// </summary>
	public class Response
	{
		/// <summary>
		/// Gets whether the request has been logged.
		/// </summary>
		[JsonProperty("logged")]
		public bool IsLogged { get; private set; }

		/// <summary>
		/// Gets the message passed along with the response.
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; private set; }
	}
}