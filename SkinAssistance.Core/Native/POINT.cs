using System.Runtime.InteropServices;

namespace SkinAssistance.Core.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        #region Fields

        public int x;
        public int y;

        #endregion

        #region Constructors

        public POINT()
        {
        }

        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Members

#if DEBUG
        public override string ToString()
        {
            return "{x=" + x + ", y=" + y + "}";
        }
#endif

        #endregion
    }
}