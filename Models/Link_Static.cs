using System.Security.Cryptography;

namespace UrlProjectV1.Models
{
	public partial class Link
	{
		private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		// Cryptographically random so codes are not guessable or predictable.
		public static string CreateURL()
			=> RandomNumberGenerator.GetString(chars, 7);
	}
}
