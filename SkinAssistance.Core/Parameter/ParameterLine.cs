#region NS

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SkinAssistance.Core.Instance;

#endregion

namespace SkinAssistance.Core.Parameter
{
    public class ParameterLine
    {
        private const char SpliteChar = '^';

        #region Method

        public static List<T> Parse<T>(string[] providers) where T : class, IParameter
        {
            var tempResult = new List<T>();
            T t = ActivatorWrapper.SolveInstance<T>();
            foreach (var arg in providers)
            {
                if (string.IsNullOrEmpty(arg))
                    continue;
                if (t.IsParameterStart(arg))
                {

                    tempResult.Add(t);
                    t = ActivatorWrapper.SolveInstance<T>();
                    continue;
                }
                var arg1 = t.FormatParameter(arg);
                var tempArg = arg1.Split(SpliteChar);
                var key = tempArg[0].Replace("/", "").Replace("-", "").Replace("--", "");
                var value = "";
                if (tempArg.Length > 1)
                    value = tempArg[1];
                t.SetParameter(key, value);
            }
            tempResult.Add(t);
            tempResult.ForEach(o => o.Validate());
            return tempResult.Where(o => o.IsValidate).ToList();
        }

        #endregion

        public static string GetString(KeyValuePair<ParameterAttribute, PropertyInfo> keyValue, object valueInstanse)
        {
            var temp = keyValue.Value.GetValue(valueInstanse);
            if (temp == null || temp == string.Empty)
                return string.Empty;
            return "/" + keyValue.Key.ShortName + "^" + temp + " ";
        }
    }
}