namespace MLWD.Umbraco.Mvc.Model
{
    public class OgModel
    {
        public OgModel() : this("article")
        {

        }

        public OgModel(string pageType)
        {
            PageType = pageType;
        }

        public string Title { get; set; }

        public string PageType { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string PageUrl { get; set; }
    }
}
