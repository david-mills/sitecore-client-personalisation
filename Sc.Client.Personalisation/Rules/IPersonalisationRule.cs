namespace Sc.Client.Personalisation.Rules
{
    public interface IPersonalisationRule
    {
        string RequestValue { get; set; }
        string RequestName { get; }
    }
}
