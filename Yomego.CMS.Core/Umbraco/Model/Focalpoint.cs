namespace Yomego.CMS.Core.Umbraco.Model
{
    public class Focalpoint
    {
        public float left { get; set; }

        public float top { get; set; }

        public string QueryString
        {
            get { return top + "," + left; }
        }
    }
}