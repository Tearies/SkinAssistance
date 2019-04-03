#region NS

using System;

#endregion

namespace SkinAssistance.Core.Instance
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