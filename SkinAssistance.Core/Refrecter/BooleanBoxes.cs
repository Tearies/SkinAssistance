namespace SkinAssistance.Core.Refrecter
{
    public static class BooleanBoxes
    {
        private static object TrueBox = (object)true;
        private static object FalseBox = (object)false;

        public static object Box(bool value)
        {
            if (value)
                return BooleanBoxes.TrueBox;
            return BooleanBoxes.FalseBox;
        }
    }
}