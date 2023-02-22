using System.ComponentModel.DataAnnotations;

namespace ShortLinks.Models
{
	public class Entry
	{
		[Key]
		public DateTime EntryDate { get; set; }
		public string Public_IP_Address { get; set; }
	}
}
