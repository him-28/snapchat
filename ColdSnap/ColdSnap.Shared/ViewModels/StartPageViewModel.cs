// Copyright (c) Matt Saville and Alex Forbes-Reed. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using ColdSnap.Helpers;

namespace ColdSnap.ViewModels
{
	public sealed class StartPageViewModel
	{
		public async Task LogInAsync(string username, string password)
		{
			// Display progress indicator.
			await StatusBarHelper.ShowStatusBarAsync(App.Strings.GetString("StatusLoggingInMessage"));

			// Try to log in with the given credentials.
			var response = await App.Snapchat.AuthenticateAsync(username, password);
			if (response.IsLogInSuccessful)
			{
				// TODO: Navigate to MainPage with the account passed as a reference.
				await new MessageDialog("Yay, you logged in! Username: " + response.Account.Username).ShowAsync();
			}
			else
			{
				// Display error message.
				var message = String.IsNullOrEmpty(response.Message)
					? App.Strings.GetString("LogInGenericErrorMessage")
					: response.Message;

				// TODO: Record error info to be sent to our error tracker

				await new MessageDialog(message).ShowAsync();
			}

			// Hide progress indicator.
			await StatusBarHelper.HideStatusBarAsync();
		}
	}
}
