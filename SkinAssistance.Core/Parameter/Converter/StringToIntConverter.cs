#region NS

using System.Globalization;

#endregion

namespace SkinAssistance.Core.Parameter.Converter
{
    public class StringToIntConverter : IValueConverter
    {
        #region Override

        /// <summary>
        ///     ÀàÐÍ×ª»»
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertTo(object value)
        {
            if (value == null)
                value = "";
            var result = 0;
            if (!int.TryParse(value.ToString(), NumberStyles.Any, null, out result))
                result = 0;
            return result;
        }

        #endregion
    }
}