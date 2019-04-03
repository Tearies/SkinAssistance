namespace SkinAssistance.Core.Native
{
    public enum PRF : long
    {
        PRF_CHECKVISIBLE = 0x00000001L,
        PRF_NONCLIENT = 0x00000002L,
        PRF_CLIENT = 0x00000004L,
        PRF_ERASEBKGND = 0x00000008L,
        PRF_CHILDREN = 0x00000010L,
        PRF_OWNED = 0x00000020L
    }
}