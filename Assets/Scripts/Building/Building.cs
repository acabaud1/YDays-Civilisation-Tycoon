using Assets.Scripts.Building;
using Boo.Lang;
using UnityEngine;

/// <summary>
///     Modèles d'un batiment .
/// </summary>
public class Building : MonoBehaviour
{
    /// <summary>
    ///     Obtient ou définit le batiment à poser.
    /// </summary>
    public GameObject FullBuilding;

    /// <summary>
    ///     Obtient ou définit le fantome du batiment.
    /// </summary>
    public GameObject GhostBuilding;

    /// <summary>
    ///     Obtient ou définit le fantome du batiment.
    /// </summary>
    public GameObject GhostBuildingBlocked;

    /// <summary>
    ///     Obtient ou définit le batiment cible.
    /// </summary>
    public BuildingEnum BuildingEnum;
}