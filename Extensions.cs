public static class Extensions
{
	public static string FullDomaine(this HttpContext context)
	{
		return $"{context.Request.Scheme}://{context.Request.Host.ToUriComponent()}";
	}

	public static async Task<string> GetPublicIpAddress()
		=> await new HttpClient().GetStringAsync("https://api.ipify.org");

}

