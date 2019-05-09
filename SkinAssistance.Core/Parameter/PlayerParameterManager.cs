namespace SkinAssistance.Core.Parameter
{
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
}