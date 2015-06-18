using System.Collections.Generic;
using System.Net;

namespace Yomego.Umbraco.Utils
{
    /// <summary>
    /// Key value pair extensions
    /// </summary>
    public static class KeyValuePairUtils
    {
        /// <summary>
        /// Get Key value pair from Header collection
        /// </summary>
        /// <param name="webHeaderCollection"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string>[] GetHeaders(this WebHeaderCollection webHeaderCollection)
        {
            string[] keys = webHeaderCollection.AllKeys;

            var keyVals = new KeyValuePair<string, string>[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                keyVals[i] = new KeyValuePair<string, string>(keys[i], webHeaderCollection[keys[i]]);
            }

            return keyVals;
        }
    }
}
