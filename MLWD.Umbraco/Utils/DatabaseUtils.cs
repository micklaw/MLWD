using Umbraco.Core.Persistence;

namespace MLWD.Umbraco.Utils
{
    public static class DatabaseUtils
    {
        public static string GetColumnDataType(this Database db, string tableName, string columName)
        {
            var query = new Sql().Select("DATA_TYPE")
                                 .From("INFORMATION_SCHEMA.COLUMNS")
                                 .Where(string.Format("(TABLE_NAME = '{0}') AND (COLUMN_NAME = '{1}')", tableName, columName));

            return db.ExecuteScalar<string>(query);
        }
    }
}
