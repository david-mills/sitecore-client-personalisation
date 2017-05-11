namespace Sc.Client.Personalisation.Rules
{
    using Sitecore.Rules;

    public class UserIsStudentCondition<T> : RequestMatchesValueBaseCondition<T> where T : RuleContext
    {
        public override string RequestValue
        {
            get { return "1"; }
            set { }

        }

        public override string RequestName
        {
            get { return "isstudent"; }
        }
    }
}
