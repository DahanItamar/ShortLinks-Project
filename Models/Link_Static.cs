using System.Security.Cryptography;
using System.Text;

namespace UrlProjectV1.Models
{
	public partial class Link
	{
		private static readonly Random random = new Random();
		private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		public static string CreateURL()
		{
			return new string(Enumerable.Repeat(chars, 7)
				.Select(s => s[random.Next(s.Length)])
				.ToArray());
		}
	}
}
