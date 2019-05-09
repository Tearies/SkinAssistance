#region NS

using System;
using System.Linq;
using System.Reflection;

#endregion

namespace SkinAssistance.Core.Refrecter
{
    #region Reference

    #endregion

    internal class InstanseCore
    {
        #region Properties

        private const BindingFlags ConstructorsFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private const BindingFlags StaticFieldFlags = PublicStaticFlags | BindingFlags.NonPublic;
        internal const BindingFlags PublicStaticFlags = BindingFlags.Public | BindingFlags.Static;
        private const FieldAttributes PublicReadOnlyFlags = FieldAttributes.Public | FieldAttributes.InitOnly;
        private const FieldAttributes PrivateReadOnlyFieldFlags = PrivateFlags | FieldAttributes.InitOnly;
        private const FieldAttributes PrivateFlags = FieldAttributes.Private;

        #endregion

        #region Method

        internal static TypeInstanceInfo GetTypeInstanceInfo(Type instanceType)
        {
            var instanseName = "";
            var isSingInstanse = false;

            var constructorInfos = instanceType.GetConstructors(ConstructorsFlags);
            isSingInstanse = constructorInfos.All(o => !o.IsPublic);
            if (isSingInstanse)
            {
                isSingInstanse = false;

                var result = instanceType.GetFields(StaticFieldFlags).Where(o => o.FieldType == instanceType);

                if (result.Any())
                {
                    var tryCheck = result.FirstOrDefault(o => o.Attributes.HasFlag(PublicReadOnlyFlags));
                    if (tryCheck != null)
                    {
                        isSingInstanse = true;
                        instanseName = tryCheck.Name;
                    }
                    else
                    {
                        tryCheck =
                            result.FirstOrDefault(
                                o =>
                                    o.Attributes.HasFlag(PrivateFlags) ||
                                    o.Attributes.HasFlag(PrivateReadOnlyFieldFlags));
                        if (tryCheck != null)
                        {
                            var propertyInfos =
                                instanceType.GetProperties(PublicStaticFlags)
                                    .FirstOrDefault(o => o.PropertyType == instanceType && !o.CanWrite && o.CanRead);
                            if (propertyInfos != null)
                            {
                                isSingInstanse = true;
                                instanseName = propertyInfos.Name;
                            }
                        }
                    }
                }
            }
            var info = new TypeInstanceInfo(isSingInstanse, instanseName);
            return info;
        }

        #endregion

        // internal const BindingFlags PublicStaicReadOnlyAttributes = PublicStaticFlags;
    }
}