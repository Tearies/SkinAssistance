namespace SkinAssistance.Core.Native
{
    internal static class BooleanBoxes
    {
        #region Fields

        internal static object TrueBox = true;
        internal static object FalseBox = false;

        #endregion

        #region Members

        internal static object Box(bool value)
        {
            if (value)
                return TrueBox;
            return FalseBox;
        }

        #endregion
    }
}