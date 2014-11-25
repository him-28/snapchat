// Copyright (c) Matt Saville and Alex Forbes-Reed. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace ColdSnap.Helpers
{
	/// <summary>
	/// Provides helper methods for the status bar.
	/// </summary>
	public static class StatusBarHelper
	{
		public static async Task ShowStatusBarAsync(string status)
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = status;
			await StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
#else
			// TODO: Implement for Windows apps
			throw new NotImplementedException();
#endif
		}

		public static async Task HideStatusBarAsync()
		{
#if WINDOWS_PHONE_APP
			StatusBar.GetForCurrentView().ProgressIndicator.Text = String.Empty;
			await StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
#else
			// TODO: Implement for Windows apps
			throw new NotImplementedException();
#endif
		}
	}
}
