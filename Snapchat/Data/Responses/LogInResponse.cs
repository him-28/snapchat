// Copyright (c) Matt Saville and Alex Forbes-Reed. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Snapchat.Data.Responses
{
	/// <summary>
	/// Represents a response from the API after a login attempt.
	/// </summary>
	public sealed class LogInResponse
	{
		internal LogInResponse(Account account)
		{
			Account = account;
		}

		internal LogInResponse(string message)
		{
			Message = message;
		}

		/// <summary>
		/// Gets the account data if the log in was successful; otherwise, <c>null</c>.
		/// </summary>
		public Account Account { get; private set; }

		/// <summary>
		/// Gets a boolean value indicating whether the user has logged in successfully.
		/// </summary>
		public bool IsLogInSuccessful
		{
			get { return Account != null; }
		}

		/// <summary>
		/// Gets the error message to display.
		/// </summary>
		public string Message { get; private set; }
	}
}
