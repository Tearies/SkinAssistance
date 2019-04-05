using System;

namespace SkinAssistance.ViewModel
{
    public interface IOperation
    {
        /// <summary>
        /// 操作器名称
        /// </summary>
        string OptionName { get; }

        /// <summary>
        /// 操作器视图类型
        /// </summary>
        Type OperationViewType { get; }
    }
}