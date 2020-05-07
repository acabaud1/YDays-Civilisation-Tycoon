using System;
using Ressource;

namespace Assets.Scripts.Resources
{
    public static class ResourceHelper
    {
        public static Type GetResourceGameTypeFromRessourceEnum(ResourceEnum resourceEnum)
        {
            if(resourceEnum == ResourceEnum.Iron)
            {
                return typeof(Iron);
            }
            if(resourceEnum == ResourceEnum.Stone)
            {
                return typeof(Stone);
            }
            if(resourceEnum == ResourceEnum.Wood)
            {
                return typeof(Wood);
            }
            return null;
        }
    }
}
