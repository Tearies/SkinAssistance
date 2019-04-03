#region NS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.Instance;

#endregion

namespace SkinAssistance.Core.Parameter
{
    public abstract class ParameterBase : ServiceProvider, IParameter
    {


        #region Properties

        private Dictionary<ParameterAttribute, PropertyInfo> PropertyCache;

        /// <summary>
        ///     参数是否正确
        /// </summary>
        public bool IsValidate { get; private set; }

        /// <summary>
        /// 参数实例标识
        /// </summary>
        protected abstract string NewPlayerIdentfier { get; }

        #endregion

        #region Override

        public void SetParameter(string key, string value)
        {
            var findProperty =
                PropertyCache.Keys.FirstOrDefault(
                    o => string.Equals(o.ShortName, key, StringComparison.InvariantCultureIgnoreCase));
            if (findProperty != null)
            {
                var tvalue = findProperty.Converter == null ? value : findProperty.Converter.ConvertTo(value);
                PropertyCache[findProperty].SetValue(this, tvalue);
            }
        }

        /// <summary>
        ///     获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetParameter()
        {
            var sb = new StringBuilder();
            foreach (var keyValue in PropertyCache)
            {
                var tempP = ParameterLine.GetString(keyValue, this);
                if (tempP == string.Empty)
                    continue;
                sb.AppendFormat("{0}", tempP);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 开始验证新实例
        /// </summary>
        /// <returns></returns>
        public bool IsParameterStart(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                return false;
            }
            return arg.Trim() == NewPlayerIdentfier;
        }

        /// <summary>
        /// 格式化参数
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public string FormatParameter(string arg)
        {
            return arg.Replace(NewPlayerIdentfier, string.Empty);
        }

        /// <summary>
        /// 校验参数
        /// </summary>
        public void Validate()
        {
            foreach (var property in PropertyCache)
            {
                var tValue = property.Value.GetValue(this);
                if ((tValue == null || string.IsNullOrEmpty(tValue.ToString())) && property.Key.IsRequire)
                {
                    IsValidate = false;
                    return;
                }
            }
            IsValidate = true;
        }

        #endregion

        #region Method

        protected ParameterBase()
        {
            PropertyCache = this.ResolvePropertis<ParameterAttribute>();
            IsValidate = false;
        }

        #endregion
    }
}