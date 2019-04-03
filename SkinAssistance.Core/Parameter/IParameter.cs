namespace SkinAssistance.Core.Parameter
{
    public interface IParameter
    {
        #region Properties

        /// <summary>
        ///     �����Ƿ���ȷ
        /// </summary>
        bool IsValidate { get; }

        #endregion

        #region Method

        /// <summary>
        ///     ���ò���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetParameter(string key, string value);

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetParameter();

        /// <summary>
        /// ��ʼ��֤��ʵ��
        /// </summary>
        /// <returns></returns>
        bool IsParameterStart(string arg);

        /// <summary>
        /// ��ʽ������
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        string FormatParameter(string arg);

        /// <summary>
        /// У�����
        /// </summary>
        void Validate();

        #endregion
    }
}