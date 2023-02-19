using ShortLinks.Models;
using System.ComponentModel.DataAnnotations;

namespace UrlProjectV1.Models
{
	public partial class Link
	{
		[Key]
		public string Code { get; set; }
		[Required]
		public string OriginalURL { get; set; }
		public DateTime Created { get; private set; } = DateTime.UtcNow;
		public string? UserID { get; set; }
		public virtual ICollection <Entry> Entries { get; set; } = new List<Entry> ();
	}
}
