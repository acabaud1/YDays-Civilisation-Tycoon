using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BuildingManager : MonoBehaviour
{
    public List<GameObject> Buildings { get; set; }
    public List<GameObject> Doodads { get; set; }
    private GameObject _ghostGameObject;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
    private Color _ghostMaterialColor;
    private bool _isInDeleteMode;
    private bool _isInGostMode;
    private GameObject _lastHoverGameObject;
    private Color? _lastHoverMaterialColor;
    private GameObject _selectedGameObject;

    [FormerlySerializedAs("LayerMask")] public LayerMask layerMask;

    /// <summary>
    ///     Initialise une nouvelle instance de la classe <see cref="BuildingManager" />
    /// </summary>
    public BuildingManager()
    {
        Buildings = new List<GameObject>();
        Doodads = new List<GameObject>();
        _isInGostMode = false;
    }

    /// <summary>
    ///     Active / Désactive le mode de suppression.
    /// </summary>
    public void ToggleDeleteMod()
    {
        Destroy(_ghostGameObject);
        _ghostGameObject = null;
        _isInGostMode = false;
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
        var lastHoverGameObjectMesh = _lastHoverGameObject != null
            ? _lastHoverGameObject.GetComponent<MeshRenderer>()
            : null;

        if (lastHoverGameObjectMesh != null && _lastHoverMaterialColor.HasValue)
            lastHoverGameObjectMesh.material.color = _lastHoverMaterialColor.Value;
        _lastHoverGameObject = null;
        _lastHoverMaterialColor = null;
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
            CleanMod();
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

            if (ray.HasValue && Physics.Raycast(ray.Value, out hit, 200.0f, layerMask))
                if (hit.transform != null)
                {
                    var mousePosition = hit.point;

                    Vector3 localScale = _selectedGameObject.transform.localScale;
                    if (_isInDeleteMode)
                    {
                        localScale = Vector3.one;
                    }
                    
                    mousePosition.x = (int) Math.Round(mousePosition.x) + (localScale.x - 1) / 2;
                    mousePosition.y = 1;
                    mousePosition.z = (int) Math.Round(mousePosition.z) + (localScale.z - 1) / 2;

                    var building = GetBuildingAtPosition(mousePosition, Vector3.one);

                    if (building == null || building != null && !building.Equals(_lastHoverGameObject))
                    {
                        var lastHoverGameObjectMesh = _lastHoverGameObject != null
                            ? _lastHoverGameObject.GetComponent<MeshRenderer>()
                            : null;

                        if (lastHoverGameObjectMesh != null && _lastHoverMaterialColor.HasValue)
                            lastHoverGameObjectMesh.material.color = _lastHoverMaterialColor.Value;
                    }

                    if (_isInDeleteMode && Input.GetMouseButtonDown(0))
                    {
                       if (Buildings.Contains(building))
                       {
                            Buildings.Remove(building);
                            Destroy(building);
                       }
                    }
                    else if (_isInDeleteMode && building != null)
                    {
                        var buildingGameObjectMesh = building != null
                            ? building.GetComponent<MeshRenderer>()
                            : null;

                        if (buildingGameObjectMesh != null)
                        {
                            var material = buildingGameObjectMesh.material;
                            if (!material.color.Equals(new Color(255, 0, 0, 0.5f)))
                            {
                                _lastHoverMaterialColor = material.color;
                                material.color = new Color(255, 0, 0, 0.5f);
                            }
                        }
                    }
                    else
                    {
                        Cursor.visible = false;
                        var meshRenderer = _ghostGameObject != null
                            ? _ghostGameObject.GetComponent<MeshRenderer>()
                            : null;

                        if (meshRenderer != null &&
                            IsPositionEmpty(mousePosition, _ghostGameObject.transform.localScale))
                        {
                            if (!meshRenderer.material.color.Equals(_ghostMaterialColor))
                                meshRenderer.material.color = _ghostMaterialColor;

                            if (Input.GetMouseButtonDown(0))
                            {
                                var buildingToCreate = Instantiate(_selectedGameObject, mousePosition,
                                    Quaternion.identity);
                                Buildings.Add(buildingToCreate);
                                Cursor.visible = true;
                                CleanMod();
                            }
                            else
                            {
                                _ghostGameObject.transform.position = mousePosition;
                            }
                        }
                        else if (meshRenderer != null)
                        {
                            meshRenderer.material.color = new Color(255, 0, 0, 0.5f);
                            mousePosition.y = 1.1f;
                            _ghostGameObject.transform.position = mousePosition;
                        }
                    }

                    if (building != null) _lastHoverGameObject = building;
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

    /// <summary>
    ///     Récupère le batiment à la position.
    /// </summary>
    /// <param name="position">Position à vérifier.</param>
    /// <param name="scaleOfObject">taille de l'objet.'</param>
    /// <returns><see cref="GameObject" /> à la position.</returns>
    private GameObject GetBuildingAtPosition(Vector3 position, Vector3 scaleOfObject)
    {
        List<GameObject> allEntities = new List<GameObject>(Buildings);
        allEntities.AddRange(Doodads);
        foreach (var building in allEntities)
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