namespace SkinAssistance.Core.Parameter
{
    public interface IKeyValueCache
    {
        #region Method

        /// <summary>
        ///     根据key查找Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T ResolveValue<T>(string key);

        /// <summary>
        ///     添加KeyValue
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="tk"></param>
        /// <param name="tv"></param>
        void AddKeyValuePaier<TK, TV>(TK tk, TV tv) where TK : class where TV : class;

        #endregion
    }
}