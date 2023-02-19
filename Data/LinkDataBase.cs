#pragma warning disable CS8625
#pragma warning disable CS8603
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UrlProjectV1.Models;

namespace UrlProjectV1.Data
{
	public class LinkDataBase : IdentityDbContext
	{
		public virtual DbSet<Link> Links { get; set; }

		public LinkDataBase(DbContextOptions options)
			: base(options)
		{
		}

		public async Task<string> CreateShortLink(string originalURL, string userID)
		{
			var DB_url = await Links.FirstOrDefaultAsync(U => U.OriginalURL == originalURL && U.UserID == userID);
			if (DB_url != null)
				return DB_url.Code;
			StringBuilder shortURL = new StringBuilder();
			do
			{
				shortURL.Length = 0;
				shortURL.Append(Link.CreateURL());
			} while (await Links.AnyAsync(u => u.Code == shortURL.ToString()));

			var value = new Link
			{
				OriginalURL = originalURL,
				Code = shortURL.ToString(),
				UserID = userID // set UserID to null if isAnonymous is true
			};

			await Links.AddAsync(value);
			await SaveChangesAsync();
			return shortURL.ToString();
		}

		public async Task<string> GiveOriginalLink(string shortURL, string userID = null)
		{
			var value = await Links.FirstOrDefaultAsync(U => U.Code == shortURL);
			if (value != null)
			{
				value.Entries++;
				await SaveChangesAsync();
				return value.OriginalURL;
			}
			return null;
		}

		public IEnumerable<Link> AllLinksByID(string userID)
		{
			return Links.Where(u => u.UserID.Equals(userID)).ToList();
		}
	}
}
