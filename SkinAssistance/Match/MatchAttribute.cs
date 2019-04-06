using System;

namespace SkinAssistance.ViewModel
{
    public class MatchAttribute : Attribute
    {
        public MatchAttribute(Type matchType)
        {
            MatchType = matchType;
        }

        public Type MatchType { get; private set; }
    }
}