using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ColdSnap
{
    public sealed partial class App
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			if (Debugger.IsAttached)
				DebugSettings.EnableFrameRateCounter = true;

			// Create navigation context and set it as the window's content.
			var rootFrame = Window.Current.Content as Frame;
			if (rootFrame == null)
			{
				rootFrame = new Frame { CacheSize = 2 };

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// TODO: Load state from previously suspended application
				}

				Window.Current.Content = rootFrame;
			}

			// Navigate to the starting page when the navigation stack isn't restored.
			if (rootFrame.Content == null)
			{
				if (!rootFrame.Navigate(typeof(Pages.StartPage)))
					throw new Exception("Failed to create initial page");
			}

			// Activate the window.
			Window.Current.Activate();
		}

		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();

			// TODO: Save application state and stop any background activity.
			deferral.Complete();
		}
    }
}