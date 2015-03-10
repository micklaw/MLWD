using Examine;
using System.Xml;
using System.Xml.Linq;
using Umbraco.Core;
using umbraco.cms.businesslogic.member;

namespace Yomego.CMS.Umbraco.Events
{
    public class MemberEvents : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            Member.AfterSave += MemberAfterSave;
        }

        void MemberAfterSave(Member member, umbraco.cms.businesslogic.SaveEventArgs e)
        {
            IndexMember(member);
        }

        private void IndexMember(Member member)
        {
            var node = member.ToXml(new XmlDocument(), false);

            var xDoc = new XDocument();

            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);

            var indexer = ExamineManager.Instance.IndexProviderCollection[Yomego.CMS.Constants.Examine.MainExamineIndexProvider];

            indexer.DeleteFromIndex(member.Id.ToString());

            indexer.ReIndexNode(xDoc.Root, "content");
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //throw new NotImplementedException();
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
