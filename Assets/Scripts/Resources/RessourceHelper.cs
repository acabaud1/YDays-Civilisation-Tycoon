using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ressource;

namespace Assets.Scripts.Resources
{
    public static class RessourceHelper
    {
        public static Type GetRessourceGameTypeFromRessourceEnum(RessourceEnum ressourceEnum)
        {
            if(ressourceEnum == RessourceEnum.Iron)
            {
                return typeof(Iron);
            }
            if(ressourceEnum == RessourceEnum.Stone)
            {
                return typeof(Stone);
            }
            if(ressourceEnum == RessourceEnum.Wood)
            {
                return typeof(Wood);
            }
            return null;
        }
    }
}
