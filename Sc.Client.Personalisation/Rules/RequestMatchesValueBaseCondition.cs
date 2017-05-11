namespace Sc.Client.Personalisation.Rules
{
    using System;
    using System.Web;
    using Constants;
    using Sitecore.Rules;
    using Sitecore.Rules.Conditions;

    public abstract class RequestMatchesValueBaseCondition<T> : WhenCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            if (ruleContext == null || string.IsNullOrWhiteSpace(RequestValue) || string.IsNullOrWhiteSpace(RequestName))
            {
                return false;
            }
            var context = HttpContext.Current;
            if (context == null)
            {
                return false;
            }
            var key = PersonalisationConstants.RequestPrefix + RequestName;

            string requestCode = GetValueFromRequest(context, key);

            return string.Equals(RequestValue, requestCode, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string GetValueFromRequest(HttpContext context, string key)
        {
            var requestCode = context.Request.QueryString[key];

            if (string.IsNullOrWhiteSpace(requestCode))
            {
                var cookie = context.Request.Cookies[key];
                if (cookie != null)
                {
                    requestCode = cookie.Value;
                }
            }

            return requestCode;
        }

        public abstract string RequestValue { get; set; }
        public abstract string RequestName { get; }
    }
}
