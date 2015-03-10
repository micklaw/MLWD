namespace Yomego.CMS.Core.Utils
{
    public static class IntUtils
    {
        /// <summary>
        /// Get the ordinal value of the integer e.g rd, st, th
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToOrdinal(this int number)
        {
            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number.ToString() + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number.ToString() + "st";
                case 2:
                    return number.ToString() + "nd";
                case 3:
                    return number.ToString() + "rd";
                default:
                    return number.ToString() + "th";
            }
        }
    }
}
