namespace MLWD.Umbraco.Mvc.Model.Media
{
    public class MediaItem
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int Bytes { get; set; }

        public string Name { get; set; }

        public virtual bool HasUrl
        {
            get { return !string.IsNullOrWhiteSpace(Url); }
        }
    }
}
