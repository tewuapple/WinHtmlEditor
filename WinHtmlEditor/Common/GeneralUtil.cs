using System;
using System.Globalization;

namespace WinHtmlEditor
{
    public static class GeneralUtil
    {
        #region Utility Functions

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return null == obj;
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 将 String 类型转换为 int 型,转换失败返回 int.MinValue
        /// </summary>
        /// <param name="strValue">待转换 String</param>
        /// <returns>返回 int</returns>
        public static int ParseInt(string strValue)
        {
            int returnValue = int.MinValue;

            if (!string.IsNullOrEmpty(strValue))
            {
                if (!int.TryParse(strValue, out returnValue))
                {
                    returnValue = int.MinValue;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 将 Object 类型转换为 int 型,转换失败时返回 int.MinValue
        /// </summary>
        /// <param name="objValue">待转换 Object</param>      
        /// <returns>返回 int</returns>
        public static int ParseInt(object objValue)
        {
            int returnValue = int.MinValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                if (!int.TryParse(objValue.ToString(), out returnValue))
                {
                    returnValue = int.MinValue;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Method to perform a parse of a string into a byte number
        /// </summary>
        public static byte TryParseByte(this string stringValue, byte defaultValue)
        {
            // define the return type
            byte result;

            // try the conversion to a double number
            if (!byte.TryParse(stringValue, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result))
            {
                // default value will be returned
                result = defaultValue;
            }

            // return the byte value
            return result;

        } //TryParseNumber


        /// <summary>
        /// Method to perform a parse of a string into a ushort number
        /// </summary>
        public static ushort TryParseUshort(this string stringValue, ushort defaultValue)
        {
            // define the return type
            ushort result;

            // try the conversion to a double number
            if (!ushort.TryParse(stringValue, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result))
            {
                // default value will be returned
                result = defaultValue;
            }

            // return the ushort value
            return result;

        } //TryParseUshort


        /// <summary>
        /// Method to perform a parse of the string into an enum
        /// </summary>
        public static object TryParseEnum(this Type enumType, string stringValue, object defaultValue)
        {
            // try the enum parse and return the default 
            object result;
            try
            {
                // try the enum parse operation
                result = Enum.Parse(enumType, stringValue, true);
            }
            catch (Exception)
            {
                // default value will be returned
                result = defaultValue;
            }

            // return the enum value
            return result;

        } //TryParseEnum

        /// <summary>
        /// Method to determine if the tag name is of the correct type
        /// A string comparision is made whilst ignoring case
        /// </summary>
        public static bool IsStringEqual(this string tagText, string tagType)
        {
            return (String.Compare(tagText, tagType, StringComparison.OrdinalIgnoreCase) == 0);

        } //IsStringEqual

        #endregion
    }
}
