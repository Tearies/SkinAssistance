namespace SkinAssistance.Core.Parameter
{
    public interface IParameter
    {
        #region Properties

        /// <summary>
        ///     参数是否正确
        /// </summary>
        bool IsValidate { get; }

        #endregion

        #region Method

        /// <summary>
        ///     设置参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetParameter(string key, string value);

        /// <summary>
        ///     获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetParameter();

        /// <summary>
        /// 开始验证新实例
        /// </summary>
        /// <returns></returns>
        bool IsParameterStart(string arg);

        /// <summary>
        /// 格式化参数
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        string FormatParameter(string arg);

        /// <summary>
        /// 校验参数
        /// </summary>
        void Validate();

        #endregion
    }
}