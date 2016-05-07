using System;
using System.Linq.Expressions;
using System.Web.Mvc;



namespace Chat.Helpers
{
	public static class ModelStateExtensions
	{
		public static void AddModelError(this ModelStateDictionary modelState, Expression<Func<object, object>> expression, string error)
		{
			var propertyName = ExpressionHelper.GetExpressionText(expression);

			modelState.AddModelError(propertyName, error);
		}
	}
}
