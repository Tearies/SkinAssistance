﻿using System;
using System.Windows.Markup;

namespace SkinAssistance.Core.Native
{
    public class FullPrimaryScreenWidthWithOutDPI : MarkupExtension
    {
		 
        #region Overrides of MarkupExtension

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var dou = Instance.InstanseManager.ResolveService<DisplayInfo>().FullScreenWidth * Instance.InstanseManager.ResolveService<DisplayInfo>().DPIX;

            return dou;
        }

        #endregion
    }
}