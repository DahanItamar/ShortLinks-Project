using System.ComponentModel.DataAnnotations;

namespace ShortLinks.Models
{
	public class Entry
	{
		[Key]
		public int Id { get; set; }
		public DateTime EntryDate { get; set; }
		public string VisitorIP { get; set; } = "unknown";
	}
}
