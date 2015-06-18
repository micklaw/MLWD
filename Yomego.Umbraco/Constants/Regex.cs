namespace Yomego.Umbraco.Constants
{
    public class Regex
    {
        public const string EmailRegex = @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?";

        public const string Decimal = @"^\d+[\.,]?\d*$";
    }
}
