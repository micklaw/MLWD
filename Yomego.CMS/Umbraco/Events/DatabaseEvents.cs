using System;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Yomego.CMS.Core.Utils;

namespace Yomego.CMS.Umbraco.Events
{
    public class DatabaseEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var db = applicationContext.DatabaseContext.Database;

            ArchetypeLengthFix(db);
            MultilineTextBoxLenghtFix(db);
        }

        private void MultilineTextBoxLenghtFix(UmbracoDatabase db)
        {
            const string tableName = "cmsPropertyData";
            const string columnName = "dataNvarchar";
            const string expectedColumnDataType = "nvarchar(2000)";

            if (db.TableExist(tableName))
            {
                var columnDataType = db.GetColumnDataType(tableName, columnName);
                var hasValidField = string.Equals(columnDataType, expectedColumnDataType, StringComparison.OrdinalIgnoreCase);

                if (hasValidField == false)
                {
                    var updateSql = string.Format("ALTER TABLE {0} ALTER COLUMN {1} {2}", tableName, columnName,
                        expectedColumnDataType);

                    db.Execute(updateSql);
                }
            }
        }

        private void ArchetypeLengthFix(UmbracoDatabase db)
        {
            const string tableName = "cmsDataTypePreValues";
            const string columnName = "value";
            const string expectedColumnDataType = "ntext";

            if (db.TableExist(tableName))
            {
                var columnDataType = db.GetColumnDataType(tableName, columnName);
                var hasValidField = string.Equals(columnDataType, expectedColumnDataType, StringComparison.OrdinalIgnoreCase);

                if (hasValidField == false)
                {
                    var updateSql = string.Format("ALTER TABLE {0} ALTER COLUMN {1} {2}", tableName, columnName,
                        expectedColumnDataType);

                    db.Execute(updateSql);
                }
            }
        }
    }
}
