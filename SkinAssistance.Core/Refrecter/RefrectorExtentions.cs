using System;

namespace SkinAssistance.Core.Refrecter
{
    public static class RefrectorExtentions
    {
        #region Method

        public static object GetStaticReadOnlyfields(this Type hostType, string fieldName)
        {
            var fieldInfo = hostType.GetField(fieldName, InstanseCore.PublicStaticFlags);
            if (fieldInfo != null)
                return fieldInfo.GetValue(null);
            return null;
        }

        #endregion
    }
}