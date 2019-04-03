namespace SkinAssistance.Core.Parameter
{
    public interface IValueConverter
    {
        #region Method

        /// <summary>
        ///     ÀàÐÍ×ª»»
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object ConvertTo(object value);

        #endregion
    }
}