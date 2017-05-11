namespace Sc.Client.Personalisation.Rules
{
    using Sitecore.Rules;

    public class UserIsAFromGeoLocation<T> : RequestMatchesValueBaseCondition<T> where T : RuleContext
    {
        public override string RequestValue
        { get; set; }

        public override string RequestName
        {
            get { return "geolocation"; }
        }
    }
}
