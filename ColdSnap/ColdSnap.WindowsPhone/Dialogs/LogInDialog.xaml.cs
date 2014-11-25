using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace ColdSnap.Dialogs
{
	public sealed partial class LogInDialog
	{
		public static readonly DependencyProperty UsernameProperty =
			DependencyProperty.Register("Username", typeof(string), typeof(LogInDialog), new PropertyMetadata(String.Empty));

		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.Register("Password", typeof(string), typeof(LogInDialog), new PropertyMetadata(String.Empty));

		public LogInDialog()
		{
			InitializeComponent();

			// Enable the primary button only if both username and password fields are filled out.
			IsPrimaryButtonEnabled = false;
			Action validateForm = () =>
			{
				IsPrimaryButtonEnabled =
					!String.IsNullOrWhiteSpace(UsernameTextBox.Text) && !String.IsNullOrWhiteSpace(PasswordBox.Password);
			};
			UsernameTextBox.TextChanged += delegate { validateForm(); };
			PasswordBox.PasswordChanged += delegate { validateForm(); };
			SecondaryButtonClick += delegate { Password = String.Empty; }; // clear the password field if cancelled
		}

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		public string Username
		{
			get { return GetValue(UsernameProperty) as string; }
			set { SetValue(UsernameProperty, value); }
		}

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		public string Password
		{
			get { return GetValue(PasswordProperty) as string; }
			set { SetValue(PasswordProperty, value); }
		}

		private void Username_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
				PasswordBox.Focus(FocusState.Keyboard);
		}

		private void Password_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
				ForgotPasswordButton.Focus(FocusState.Keyboard);
		}
	}
}
