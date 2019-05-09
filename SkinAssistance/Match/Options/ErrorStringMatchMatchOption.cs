using System;
using System.Collections.Generic;

namespace SkinAssistance.ViewModel
{
    public class ErrorStringMatchMatchOption : IMatchOption
    {
        #region Implementation of IMatchOption

        public T GetOption<T>(string optionName)
        {
            return default(T);
        }

        #endregion
    }

    public class LocalResourceDictionaryMatchOptionOption : IMatchOption
    {

        public string AppName { get; set; }

        public List<string> ResourceCulture { get; set; }

        #region Implementation of IMatchOption

        public T GetOption<T>(string optionName)
        {
            switch (optionName)
            {
                case nameof(AppName):
                    return (T)Convert.ChangeType(AppName, typeof(T));
                case nameof(ResourceCulture):
                    return (T)Convert.ChangeType(ResourceCulture, typeof(T));
            }
            return default(T);
        }

        #endregion
    }
}