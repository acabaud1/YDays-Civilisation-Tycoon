using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine;

/// <summary>
/// Méthode d'extension pour le tableau de case.
/// </summary>
public static class TileArrayExtension
{
    /// <summary>
    /// Définit si le tableau comprend l'occurence du modèle.
    /// </summary>
    /// <param name="list">Tableau.</param>
    /// <param name="tileModel">Modèle.</param>
    /// <returns>Vrai / Faux.</returns>
    public static bool Contain(this TileModel[,] list, TileModel tileModel)
    {
        for (int i = 0; i < list.Length; i++)
        {
            for (int j = 0; j < list.GetLength(1); j++)
            {
                if (list[i, j] == tileModel)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Définit si un element correspond au prédica.
    /// </summary>
    /// <param name="list">Tableau.</param>
    /// <param name="check">Predica.</param>
    /// <returns>Vrai / Faux.</returns>
    public static bool Any(this TileModel[,] list, Func<TileModel, bool> check)
    {
        for (int i = 0; i < list.Length; i++)
        {
            for (int j = 0; j < list.GetLength(1); j++)
            {
                if (check(list[i, j]))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Définit si un element est dans la zone.
    /// </summary>
    /// <param name="list">Tableau.</param>
    /// <param name="mousePosition">Position de la souris.</param>
    /// <param name="zone">Zone autour de la souris.</param>
    /// <param name="tileEnums">Type de sol de case accepté.</param>
    /// <returns>Vrai / Faux.</returns>
    public static bool AnyInZone(this TileModel[,] list, Vector3 mousePosition, Vector3 zone,
        List<TileEnum> tileEnums = null)
    {
        if (tileEnums == null)
        {
            tileEnums = new List<TileEnum>();
        }

        if ((int) mousePosition.x + (int) zone.x < 0 || (int) mousePosition.x + (int) zone.x >= list.Length ||
            (int) mousePosition.z + (int) zone.z < 0 || (int) mousePosition.z + (int) zone.z >= list.GetLength(1))
        {
            return true;
        }

        for (int i = (int) mousePosition.x; i < (int) mousePosition.x + (int) zone.x; i++)
        {
            for (int j = (int) mousePosition.z; j < (int) mousePosition.z + (int) zone.z; j++)
            {
                if (list[i, j].Building != null || list[i, j].Resource != null ||
                    tileEnums.Any( te => te == list[i, j].TileEnum))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Récupère le premier element dans la zone.
    /// </summary>
    /// <param name="list">Tableau.</param>
    /// <param name="mousePosition">Position de la souris.</param>
    /// <param name="zone">Zone autour de la souris.</param>
    /// <returns>Modèle de Case.</returns>
    public static TileModel FirstInZone(this TileModel[,] list, Vector3 mousePosition, Vector3 zone)
    {
        for (int i = (int) mousePosition.x; i < (int) mousePosition.x + (int) zone.x; i++)
        {
            for (int j = (int) mousePosition.z; j < (int) mousePosition.z + (int) zone.z; j++)
            {
                if (list[i, j].Building != null || list[i, j].Resource != null)
                {
                    return list[i, j];
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Définit si un element est dans la zone.
    /// </summary>
    /// <param name="list">Tableau.</param>
    /// <param name="mousePosition">Position de la souris.</param>
    /// <param name="zone">Zone autour de la souris.</param>
    /// <returns>Vrai / Faux.</returns>
    public static bool AnyBuildingInZone(this TileModel[,] list, Vector3 mousePosition, Vector3 zone)
    {
        if ((int) mousePosition.x + (int) zone.x < 0 || (int) mousePosition.x + (int) zone.x >= list.Length ||
            (int) mousePosition.z + (int) zone.z < 0 || (int) mousePosition.z + (int) zone.z >= list.GetLength(1))
        {
            return true;
        }

        for (int i = (int) mousePosition.x; i < (int) mousePosition.x + (int) zone.x; i++)
        {
            for (int j = (int) mousePosition.z; j < (int) mousePosition.z + (int) zone.z; j++)
            {
                if (list[i, j].Building != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Récupère le premier element dans la zone.
    /// </summary>
    /// <param name="list">Tableau.</param>
    /// <param name="mousePosition">Position de la souris.</param>
    /// <param name="zone">Zone autour de la souris.</param>
    /// <returns>Modèle de Case.</returns>
    public static TileModel FirstBuildingInZone(this TileModel[,] list, Vector3 mousePosition, Vector3 zone)
    {
        for (int i = (int) mousePosition.x; i < (int) mousePosition.x + (int) zone.x; i++)
        {
            for (int j = (int) mousePosition.z; j < (int) mousePosition.z + (int) zone.z; j++)
            {
                if (list[i, j].Building != null)
                {
                    return list[i, j];
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Définit le batiment dans la zone.
    /// </summary>
    /// <param name="list">Tableau.</param>
    /// <param name="mousePosition">Position de la souris.</param>
    /// <param name="zone">Zone autour de la souris.</param>
    /// <param name="building">Batiment.</param>
    public static void SetBuildingInZone(this TileModel[,] list, Vector3 mousePosition, Vector3 zone,
        GameObject building)
    {
        for (int i = (int) mousePosition.x; i < (int) mousePosition.x + (int) zone.x; i++)
        {
            for (int j = (int) mousePosition.z; j < (int) mousePosition.z + (int) zone.z; j++)
            {
                list[i, j].Building = building;
            }
        }
    }


    /// <summary>
    /// Détruit les doodads dans la zone.
    /// </summary>
    /// <param name="list">Tableau.</param>
    /// <param name="mousePosition">Position de la souris.</param>
    /// <param name="zone">Zone autour de la souris.</param>
    public static void ClearDoodadsInZone(this TileModel[,] list, Vector3 mousePosition, Vector3 zone)
    {
        for (int i = (int) mousePosition.x; i < (int) mousePosition.x + (int) zone.x; i++)
        {
            for (int j = (int) mousePosition.z; j < (int) mousePosition.z + (int) zone.z; j++)
            {
                if (list[i, j].Doodad != null)
                {
                    GameObject.Destroy(list[i, j].Doodad);
                    list[i, j].Doodad = null;
                }
            }
        }
    }

    /// <summary>
    /// Récupère le premier element dans la zone.
    /// </summary>
    /// <param name="list">Tableau.</param>
    /// <param name="mousePosition">Position de la souris.</param>
    /// <returns>Modèle de Case.</returns>
    public static void DeleteBuilding(this TileModel[,] list, GameObject building)
    {
        for (int i = 0; i < list.Length; i++)
        {
            for (int j = 0; j < list.GetLength(1); j++)
            {
                if (list[i, j].Building == building)
                {
                    list[i, j].Building = null;
                }
            }
        }
    }
}