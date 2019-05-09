using System;

namespace SkinAssistance.ViewModel
{
    public class BrushMatchOptionOption : IMatchOption
    {
        /// <summary>
        /// 在新的文件中替换资源
        /// </summary>
        public bool ReplaceInNewFile { get; set; }

        /// <summary>
        /// 资源中的名称前缀
        /// </summary>
        public string ResourceKeyPrefix { get; set; }

        #region Implementation of IMatchOption

        public T GetOption<T>(string optionName)
        {
            switch (optionName)
            {
                case nameof(ReplaceInNewFile):
                    return (T)Convert.ChangeType(ReplaceInNewFile,typeof(T));
                case nameof(ResourceKeyPrefix):
                    return (T)Convert.ChangeType(ResourceKeyPrefix, typeof(T));
            }

            return default(T);
        }

        #endregion
    }
}