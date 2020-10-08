using System;
using Ressource;

namespace Assets.Scripts.Resources
{
    /// <summary>
    /// class pour récupérer le type d'une ressource
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// class pour récupérer le type d'une ressource
        /// </summary>
        /// <param name="resourceEnum">liste des ressources</param>
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
