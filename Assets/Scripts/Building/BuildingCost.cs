﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.Building
{
    /// <summary>
    /// Cout du batiment.
    /// </summary>
    public class BuildingCost
    {
        /// <summary>
        /// Obtient ou définit la resource.
        /// </summary>
        public Type Resource { get; set; }

        /// <summary>
        /// obtient ou définit la quantité de ressource.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Récupère le cout du batiment.
        /// </summary>
        /// <param name="building">Batiment.</param>
        /// <returns>Cout.</returns>
        public static List<BuildingCost> GetCost(BuildingEnum building)
        {
            List<BuildingCost> result = new List<BuildingCost>();

            switch (building)
            {
                case BuildingEnum.Hub:
                {
                    return result;
                }
                case BuildingEnum.LumberJack:
                {
                    result.Add(new BuildingCost
                    {
                        Resource = typeof(Stone),
                        Quantity = 10
                    });
                    result.Add(new BuildingCost
                    {
                        Resource = typeof(Wood),
                        Quantity = 10
                    });

                    return result;
                }
                case BuildingEnum.StoneMine:
                {
                    result.Add(new BuildingCost
                    {
                        Resource = typeof(Stone),
                        Quantity = 10
                    });
                    result.Add(new BuildingCost
                    {
                        Resource = typeof(Wood),
                        Quantity = 10
                    });
                        return result;
                }
                case BuildingEnum.IronMine:
                {
                    result.Add(new BuildingCost
                    {
                        Resource = typeof(Stone),
                        Quantity = 10
                    });
                    result.Add(new BuildingCost
                    {
                        Resource = typeof(Wood),
                        Quantity = 10
                    });
                        return result;
                }
                case BuildingEnum.Storage:
                {
                    result.Add(new BuildingCost
                    {
                        Resource = typeof(Stone),
                        Quantity = 20
                    });
                    result.Add(new BuildingCost
                    {
                        Resource = typeof(Wood),
                        Quantity = 20
                    });
                        return result;
                }
            }

            return result;
        }
    }
}