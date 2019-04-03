﻿using System;
using System.Windows.Markup;

namespace SkinAssistance.Core.Native
{
    public class FullPrimaryScreenWidthStr : MarkupExtension
    {

        #region Overrides of MarkupExtension

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var dou = Instance.InstanseManager.ResolveService<DisplayInfo>().FullScreenWidth;
            return dou.ToString();
        }

        #endregion
    }
}