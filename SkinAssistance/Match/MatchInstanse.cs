using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.Instance;

namespace SkinAssistance.ViewModel
{
    public class MatchInstanse
    { 
        public static IMatch ResloveMatch(FileMatchOption option)
        {
            var fileMatchAttr = option.GetCustermAttribute<MatchAttribute>();
            if (fileMatchAttr == null)
                return null;
            return InstanseManager.ResolveService<IMatch>(fileMatchAttr.MatchType);
        }
    }
}