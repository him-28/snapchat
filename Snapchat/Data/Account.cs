// Copyright (c) Matt Saville and Alex Forbes-Reed. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Snapchat.Data
{
	/// <summary>
	/// Represents an account.
	/// </summary>
	public sealed class Account
		: ObservableObject
	{
		/// <summary>
		/// Deserializes the given JSON string into an <see cref="Account"/>.
		/// </summary>
		/// <param name="json">
		/// The JSON data representing the account.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="Account"/> class.
		/// </returns>
		public static Account Deserialize(string json)
		{
			dynamic data = JObject.Parse(json);
			var updatesResponse = data["updates_response"];
			//var friendsResponse = data["friends_response"];
			//var storiesResponse = data["stories_response"];
			//var convoResponse = data["conversations_response"];
			//messaging_gateway_info, background_fetch_secret_key

			return new Account
			{
				Username = (string) updatesResponse["username"]
			};
		}



		/// <summary>
		/// Gets the username associated with this account.
		/// </summary>
		public string Username
		{
			get { return _username; }
			private set { SetValue(ref _username, value); }
		}
		private string _username;
	}
}
