// Copyright (c) Matt Saville and Alex Forbes-Reed. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Web.Http;
using Snapchat.Utilities;

using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace Snapchat
{
	/// <summary>
	/// Represents a key-value collection of API parameters.
	/// 
	/// This class cannot be inherited.
	/// </summary>
	internal sealed class RequestParameters
		: Dictionary<string, string>
	{
		/// <summary>
		/// Encodes the API parameters into a string.
		/// </summary>
		/// <returns>The encoded parameters.</returns>
		public string Encode()
		{
			return String.Join("&", this.Select(o => String.Format("{0}={1}", o.Key, Uri.EscapeDataString(o.Value))));
		}
	};



	/// <summary>
	/// Provides various methods for sending GET and POST requests to a Snapchat endpoint.
	/// </summary>
	internal class EndpointManager
	{
		/// <summary>
		/// Specifies the hashing pattern to use when generating a request token.
		/// </summary>
		private static readonly string HashingPattern =
			"0001110111101110001111010101111011010001001110011000110001000110";



		/// <summary>
		/// Initializes a new <see cref="EndpointManager"/> for an endpoint.
		/// </summary>
		/// <param name="baseUri">
		/// The base URI of the endpoint.
		/// </param>
		/// <param name="staticToken">
		/// The static token to use when generating a request token.
		/// </param>
		/// <param name="secretToken">
		/// The secret token to use when generating a request token.
		/// </param>
		/// <param name="userAgent">
		/// The user agent to use when sending requests.
		/// </param>
		public EndpointManager(Uri baseUri, string staticToken, string secretToken, string userAgent)
		{
			Contract.Requires<ArgumentNullException>(baseUri != null);
			Contract.Requires<ArgumentNullException>(staticToken != null);
			Contract.Requires<ArgumentNullException>(secretToken != null);
			Contract.Requires<ArgumentNullException>(userAgent != null);

			BaseUri = baseUri;
			StaticToken = staticToken;
			SecretToken = secretToken;
			UserAgent = userAgent;
		}



		/// <summary>
		/// Gets the base URI of the API.
		/// </summary>
		public Uri BaseUri { get; private set; }

		/// <summary>
		/// Gets the static token used when generating a request token.
		/// </summary>
		public string StaticToken { get; private set; }

		/// <summary>
		/// Gets the secret token used when generating a request token.
		/// </summary>
		public string SecretToken { get; private set; }

		/// <summary>
		/// Gets or sets the user agent to use when sending requests.
		/// </summary>
		public string UserAgent { get; set; }



		#region POST

		/// <summary>
		/// Sends a POST request to an API function using a <paramref name="token"/>.
		/// </summary>
		/// <param name="function">
		/// The name of the function.
		/// </param>
		/// <param name="args">
		/// A set of parameters to pass along with the request.
		/// </param>
		/// <param name="token">
		/// The auth token.
		/// </param>
		/// <param name="headerValues">
		/// A dictionary containing additional headers to include in the POST request.
		/// </param>
		/// <returns>
		/// A <see cref="HttpResponseMessage"/> received from the server.
		/// </returns>
		public async Task<HttpResponseMessage> PostAsync(
			string function, RequestParameters args, string token, Dictionary<string, string> headerValues)
		{
			Contract.Requires<ArgumentNullException>(token != null);
			Contract.Requires<ArgumentNullException>(function != null);

			// Append the timestamp and request token to the arguments.
			args = args ?? new RequestParameters();
			args["timestamp"] = DateTime.UtcNow.ToJScriptTime().ToString();
			args["req_token"] = GenerateRequestToken(token, args["timestamp"]);

			// Create the HTTP client.
			var client = new HttpClient();
			client.DefaultRequestHeaders.TryAppendWithoutValidation("User-Agent", UserAgent);
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Accept", "*/*");
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Accept-Encoding", "gzip,deflate");
			if (headerValues != null)
			{
				foreach (var header in headerValues)
					client.DefaultRequestHeaders.Add(header.Key, header.Value);
			}

			// Send a POST request to the function, and return the response.
			var endpoint = new Uri(BaseUri, function);
			var postBody = new HttpStringContent(args.Encode(), UnicodeEncoding.Utf8, "application/x-www-form-urlencoded");
			Debug.WriteLine("[EndpointManager] Sending POST request to {0}; Data: {1}", endpoint, postBody);
			return await client.PostAsync(endpoint, postBody);
		}

		#endregion

		#region GET

		/// <summary>
		/// Sends a GET request to an API function.
		/// </summary>
		/// <param name="function">
		/// The name of the function.
		/// </param>
		/// <param name="headerValues">
		/// A dictionary containing additional headers to include in the POST request.
		/// </param>
		/// <returns>
		/// A <see cref="HttpResponseMessage"/> received from the server.
		/// </returns>
		public async Task<HttpResponseMessage> GetAsync(string function, Dictionary<string, string> headerValues)
		{
			Contract.Requires<ArgumentNullException>(function != null);

			// Create the HTTP client.
			var client = new HttpClient();
			client.DefaultRequestHeaders.TryAppendWithoutValidation("User-Agent", UserAgent);
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Accept", "*/*");
			client.DefaultRequestHeaders.TryAppendWithoutValidation("Accept-Encoding", "gzip,deflate");
			if (headerValues != null)
			{
				foreach (var header in headerValues)
					client.DefaultRequestHeaders.Add(header.Key, header.Value);
			}

			// Send a GET request to the endpoint, and return the response.
			var endpoint = new Uri(BaseUri, function);
			Debug.WriteLine("[EndpointManager] Sending GET request to {0}", endpoint);
			return await client.GetAsync(endpoint);
		}

		#endregion

		/// <summary>
		/// Generates a request token from the given token and timestamp.
		/// </summary>
		/// <param name="token">
		/// The auth token.
		/// </param>
		/// <param name="timestamp">
		/// The timestamp of the request.
		/// </param>
		/// <returns>
		/// A request token, all nice.
		/// </returns>
		private string GenerateRequestToken(string token, string timestamp)
		{
			Contract.Requires<ArgumentNullException>(token != null);
			Contract.Requires<ArgumentNullException>(timestamp != null);

			// SHA-256 hashing function
			Func<string, string> hash = data =>
			{
				var input = CryptographicBuffer.ConvertStringToBinary(data, BinaryStringEncoding.Utf8);
				var hashedData = HashAlgorithmProvider.OpenAlgorithm("SHA256").HashData(input);
				return CryptographicBuffer.EncodeToHexString(hashedData);
			};

			// Generate hashes.
			var s1 = hash(SecretToken + token);
			var s2 = hash(timestamp + SecretToken);

			// Create a token based on the generated hashes and the hashing pattern.
			var output = new StringBuilder();
			for (var i = 0; i < HashingPattern.Length; i++)
			{
				output.Append(HashingPattern[i] == '0'
					? s1[i]
					: s2[i]);
			}
			return output.ToString();
		}
	}
}