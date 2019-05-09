using System;
using SkinAssistance.Core.Parameter.Converter;

namespace SkinAssistance.Core.Parameter
{
    public class PlayerParameter : ParameterBase
    {
        [Parameter("I", "ID", true)]
        public string CanvasID { get; set; }

        [Parameter("N", "N")]
        public string CanvasName { get; set; }

        /// <summary>
        /// 单位毫秒
        /// </summary>
        [Parameter("D", "D", convertType: typeof(StringToIntConverter))]
        public int DelayToStart { get; set; }

        #region Overrides of ParameterBase

        public Guid CanvasGuid
        {
            get
            {
                if (Guid.TryParse(CanvasID, out Guid guid))
                {
                    return guid;
                }

                return Guid.Empty;
            }
        }
        /// <summary>
        /// 参数实例标识
        /// </summary>
        protected override string NewPlayerIdentfier
        {
            get { return "||"; }
        }





        #endregion
    }
}