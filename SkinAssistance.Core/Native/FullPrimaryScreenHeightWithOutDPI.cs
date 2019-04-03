using System;
using System.Windows.Markup;

namespace SkinAssistance.Core.Native
{
    public class FullPrimaryScreenHeightWithOutDPI : MarkupExtension
    {

        #region Overrides of MarkupExtension

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var dou = Instance.InstanseManager.ResolveService<DisplayInfo>().FullScreenHeight * Instance.InstanseManager.ResolveService<DisplayInfo>().DPIY;

            return dou;
        }

        #endregion
    }
}