// Copyright (c) Matt Saville and Alex Forbes-Reed. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Snapchat
{
	/// <summary>
	/// Provides various methods for authentication and account registration.
	/// </summary>
	public class SnapchatManager
	{
		/// <summary>
		/// The default user agent to use when sending requests.
		/// </summary>
		public const string DefaultUserAgent =
			"User-Agent: Snapchat/8.0.0 (Nexus 4; Android 21; gzip)";



		/// <summary>
		/// Provides a <see cref="EndpointManager"/> for each API version.
		/// </summary>
		internal readonly IReadOnlyDictionary<string, EndpointManager> Endpoints;

		/// <summary>
		/// Defines all supported API versions.
		/// </summary>
		private static readonly IReadOnlyList<string> EndpointVersions = new[]
		{
			"bq",
			"ph",
			"loq",
		};

		/// <summary>
		/// Defines the base URI of the API.
		/// </summary>
		private static readonly Uri BaseApiUri = new Uri("https://api.snapchat.com/");



		/// <summary>
		/// Initializes a new instance of the Snapchat API with the given auth tokens.
		/// </summary>
		/// <param name="staticToken">
		/// The static token to use when generating a request token.
		/// </param>
		/// <param name="secretToken">
		/// The secret token to use when generating a request token.
		/// </param>
		public SnapchatManager(string staticToken, string secretToken)
		{
			_userAgent = DefaultUserAgent;
			
			// Create an endpoint manager for each endpoint.
			var managers = new Dictionary<string, EndpointManager>();
			foreach (string version in EndpointVersions)
			{
				var endpointUri = new Uri(BaseApiUri, version);
				managers.Add(version, new EndpointManager(endpointUri, staticToken, secretToken, _userAgent));
			}
			Endpoints = managers;
		}



		/// <summary>
		/// Gets or sets the user agent to use when sending requests.
		/// </summary>
		public string UserAgent
		{
			get { return _userAgent; }
			set
			{
				foreach (EndpointManager ep in Endpoints.Values)
					ep.UserAgent = value;

				_userAgent = value;
			}
		}
		private string _userAgent;
	}
}
