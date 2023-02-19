public static class Extensions
{
	public static string FullDomaine(this HttpContext context)
	{
		return $"{context.Request.Scheme}://{context.Request.Host.ToUriComponent()}";
	}
}

