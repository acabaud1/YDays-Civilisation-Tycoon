using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    private readonly List<GameObject> _buildings;
    private GameObject _ghostGameObject;
    private Color _ghostMaterialColor;
    private bool _isInDeleteMode;
    private bool _isInGostMode;
    private GameObject _selectedGameObject;

    /// <summary>
    ///     Initialise une nouvelle instance de la classe <see cref="BuildingManager" />
    /// </summary>
    public BuildingManager()
    {
        _buildings = new List<GameObject>();
        _isInGostMode = false;
    }

    public void ToggleDeleteMod()
    {
        _isInDeleteMode = !_isInDeleteMode;
    }

    /// <summary>
    ///     Supprime les batiments.
    /// </summary>
    public void CleanMod()
    {
        Destroy(_ghostGameObject);
        _ghostGameObject = null;
        _isInGostMode = false;
        _isInDeleteMode = false;
    }

    /// <summary>
    ///     Définit le bâtiment à placer.
    /// </summary>
    /// <param name="building">Bâtiment.</param>
    /// <param name="ghost">Fantome du bâtiment.</param>
    public void SetBuilding(GameObject buildingManager)
    {
        var script = buildingManager.GetComponent<Building>();
        if (script != null)
        {
            if (_ghostGameObject != null) Destroy(_ghostGameObject);
            _selectedGameObject = script.FullBuilding;
            _isInGostMode = true;
            _ghostGameObject = Instantiate(script.GhostBuilding, new Vector3(0, -2, 0), Quaternion.identity);
            _ghostMaterialColor = _ghostGameObject.GetComponent<MeshRenderer>().material.color;
        }
    }

    /// <summary>
    ///     Fonction de mise à jour appelé chaque frame.
    /// </summary>
    private void Update()
    {
        if ((_isInGostMode || _isInDeleteMode) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            var ray = Camera.main?.ScreenPointToRay(Input.mousePosition);

            if (ray.HasValue && Physics.Raycast(ray.Value, out hit, 100.0f))
                if (hit.transform != null)
                {
                    var mousePosition = hit.point;

                    var localScale = _selectedGameObject.transform.localScale;
                    mousePosition.x = (int) Math.Round(mousePosition.x) + (localScale.x - 1) / 2;
                    mousePosition.y = 1;
                    mousePosition.z = (int) Math.Round(mousePosition.z) + (localScale.z - 1) / 2;

                    var meshRenderer = _ghostGameObject != null ? _ghostGameObject.GetComponent<MeshRenderer>() : null;

                    if (meshRenderer != null && IsPositionEmpty(mousePosition, _ghostGameObject.transform.localScale))
                    {
                        if (!meshRenderer.material.color.Equals(_ghostMaterialColor))
                            meshRenderer.material.color = _ghostMaterialColor;

                        if (Input.GetMouseButtonDown(0))
                        {
                            var building = Instantiate(_selectedGameObject, mousePosition, Quaternion.identity);
                            _buildings.Add(building);

                            CleanMod();
                        }
                        else
                        {
                            _ghostGameObject.transform.position = mousePosition;
                        }
                    }
                    else if (_isInDeleteMode && Input.GetMouseButtonDown(0))
                    {
                        var building = GetBuildingAtPosition(mousePosition, Vector3.one);

                        Debug.Log("building : " + building);

                        _buildings.Remove(building);
                        Destroy(building);
                    }
                    else if (meshRenderer != null)
                    {
                        meshRenderer.material.color = new Color(255, 0, 0, 0.5f);
                        mousePosition.y = 1.1f;
                        _ghostGameObject.transform.position = mousePosition;
                    }
                }
        }
    }

    /// <summary>
    ///     Définis si la zone est libre pour un batiment.
    /// </summary>
    /// <param name="position">Positon du batiment.</param>
    /// <param name="scaleOfObject">Taille du batiment.</param>
    /// <returns>Définit si l'on peut poser l'objet.''</returns>
    private bool IsPositionEmpty(Vector3 position, Vector3 scaleOfObject)
    {
        return GetBuildingAtPosition(position, scaleOfObject) == null;
    }

    private GameObject GetBuildingAtPosition(Vector3 position, Vector3 scaleOfObject)
    {
        foreach (var building in _buildings)
        {
            var buildingtranform = building.transform;

            var buildingLeftX = CalculateNegativeZone(buildingtranform.position.x, buildingtranform.localScale.x);
            var buildingTopZ = CalculateNegativeZone(buildingtranform.position.z, buildingtranform.localScale.z);

            var ghostBuildingLeftX = CalculateNegativeZone(position.x, scaleOfObject.x);
            var ghostBuildingTopZ = CalculateNegativeZone(position.z, scaleOfObject.z);

            var buildingRightX = CalculateZone(buildingtranform.position.x, buildingtranform.localScale.x);
            var buildingBottomZ = CalculateZone(buildingtranform.position.z, buildingtranform.localScale.z);

            var ghostBuildingRightX = CalculateZone(position.x, scaleOfObject.x);
            var ghostBuildingBottomZ = CalculateZone(position.z, scaleOfObject.z);

            // Regarde si le batiment à placer est dans le batiment à comparer.
            // OU
            // Regarde si le batiment à comparer est dans le batiment à placer.
            if ((buildingLeftX <= ghostBuildingLeftX && ghostBuildingLeftX <= buildingRightX ||
                 buildingLeftX <= ghostBuildingRightX && ghostBuildingRightX <= buildingRightX) &&
                (buildingTopZ <= ghostBuildingTopZ && ghostBuildingTopZ <= buildingBottomZ ||
                 buildingTopZ <= ghostBuildingBottomZ && ghostBuildingBottomZ <= buildingBottomZ) ||
                (ghostBuildingLeftX <= buildingLeftX && buildingLeftX <= ghostBuildingRightX ||
                 ghostBuildingLeftX <= buildingRightX && buildingRightX <= ghostBuildingRightX) &&
                (ghostBuildingTopZ <= buildingTopZ && buildingTopZ <= ghostBuildingBottomZ ||
                 ghostBuildingTopZ <= buildingBottomZ && buildingBottomZ <= ghostBuildingBottomZ))
                return building;
        }

        return null;
    }

    /// <summary>
    ///     Calcul le point le plus à droite du batiment.
    /// </summary>
    /// <param name="x">Position.</param>
    /// <param name="xs">Taille.</param>
    /// <returns>Point à l'extreme droite.'</returns>
    private float CalculateZone(float x, float xs)
    {
        return x + (xs - 1) / 2;
    }

    /// <summary>
    ///     Calcul le point le plus à gauche du batiment.
    /// </summary>
    /// <param name="x">Position.</param>
    /// <param name="xs">Taille.</param>
    /// <returns>Point à l'extreme gauche.'</returns>
    private float CalculateNegativeZone(float x, float xs)
    {
        return x - (xs - 1) / 2;
    }
}