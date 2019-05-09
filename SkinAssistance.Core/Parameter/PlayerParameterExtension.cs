using System;
using System.Windows.Markup;

namespace SkinAssistance.Core.Parameter
{
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