using System.Web.Optimization;



namespace Chat
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(
				new ScriptBundle("~/bundles/common").Include(
					"~/Scripts/json2.js",
					"~/Scripts/jquery-{version}.js",
					"~/Scripts/bootstrap.js",
					"~/Scripts/knockout-{version}.js"));

			bundles.Add(
				new StyleBundle("~/Content/css").Include(
					"~/Content/bootstrap.css",
					"~/Content/site.css"));
		}
	}
}
