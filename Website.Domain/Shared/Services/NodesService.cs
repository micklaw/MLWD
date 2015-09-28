using Website.Domain.Contents.DocTypes;

namespace Website.Domain.Shared.Services
{
    public class NodesService : WebsiteService
    {
        private Contact _contact { get; set; }

        public Contact Contact
        {
            get
            {
                if (_contact == null)
                {
                    _contact = App.Services.Content.First<Contact>();
                }

                return _contact;
            }
        }
    }
}