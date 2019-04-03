using System.Windows;

namespace SkinAssistance.Core.Native
{
    public static class VisibilityBoxes
    {
        #region Fields

        internal static object VisibleBox = Visibility.Visible;
        internal static object HiddenBox = Visibility.Hidden;
        internal static object CollapsedBox = Visibility.Collapsed;

        #endregion

        #region Members

        public static object Box(Visibility value)
        {
            if (value == Visibility.Visible)
                return VisibleBox;
            if (value == Visibility.Hidden)
                return HiddenBox;
            return CollapsedBox;
        }

        #endregion
    }
}