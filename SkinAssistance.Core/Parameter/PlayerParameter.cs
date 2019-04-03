using System;
using System.Windows.Markup;
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

    public static class PlayerParameterManager
    {
        static PlayerParameterManager()
        {
            Empty = new PlayerParameter() { CanvasName = string.Empty, CanvasID = string.Empty };
        }
        public static PlayerParameter Empty { get; }

        public static bool IsNullOrDefault(PlayerParameter currentPlayer)
        {
            if (currentPlayer == null)
                return true;
            return currentPlayer.CanvasID == Empty.CanvasID;
        }
    }

    public class PlayerParameterExtension : MarkupExtension
    {
        public PlayerParameterExtension(string cavasId, string cavasName)
        {
            CavasId = cavasId;
            CavasName = cavasName;
        }

        public string CavasId { get; private set; }

        public string CavasName { get; private set; }

        #region Overrides of MarkupExtension

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new PlayerParameter()
            {
                CanvasID = CavasId,
                CanvasName = CavasName
            };
        }

        #endregion
    }
}