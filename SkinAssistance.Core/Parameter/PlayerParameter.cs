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
        /// ��λ����
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
        /// ����ʵ����ʶ
        /// </summary>
        protected override string NewPlayerIdentfier
        {
            get { return "||"; }
        }





        #endregion
    }
}