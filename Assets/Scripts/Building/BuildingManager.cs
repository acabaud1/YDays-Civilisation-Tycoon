﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BuildingManager
{
    /// <summary>
    /// Liste des cases.
    /// </summary>
    public TileModel[,] TileArray { get; set; }

    private bool _isInDeleteMode;
    private bool _isInGostMode;

    private Color? _lastHoverGameObjectColor;
    private GameObject _lastHoverGameObject;
    private GameObject _selectedGameObject;
    private GameObject _ghostGameObject;
    private Color? _ghostGameObjectColor;
    private Building _lastBuilding;

    [FormerlySerializedAs("LayerMask")] public LayerMask layerMask;
    public PNJManager pnjManager;

    /// <summary>
    ///     Initialise une nouvelle instance de la classe <see cref="BuildingManager" />
    /// </summary>
    private BuildingManager()
    {
    }

    /// <summary>
    /// Instance de la gestion des bâtiments.
    /// </summary>
    private static BuildingManager instance;

    /// <summary>
    /// Récupère l'instance de building manager.
    /// </summary>
    /// <returns>Building Manager.</returns>
    public static BuildingManager GetInstance()
    {
        if (instance == null)
        {
            instance = new BuildingManager();
        }

        return instance;
    }

    /// <summary>
    ///     Active / Désactive le mode de suppression.
    /// </summary>
    public void ToggleDeleteMod()
    {
        GameObject.Destroy(_ghostGameObject);
        _ghostGameObject = null;
        _lastBuilding = null;
        _isInGostMode = false;
        _isInDeleteMode = !_isInDeleteMode;
    }

    /// <summary>
    ///     Supprime les batiments.
    /// </summary>
    public void CleanMod()
    {
        GameObject.Destroy(_ghostGameObject);
        _ghostGameObject = null;
        _isInGostMode = false;
        _isInDeleteMode = false;
        _lastBuilding = null;
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
            _lastBuilding = script;
            if (_ghostGameObject != null) GameObject.Destroy(_ghostGameObject);
            _selectedGameObject = script.FullBuilding;
            _isInGostMode = true;
            _ghostGameObject = GameObject.Instantiate(script.GhostBuilding, new Vector3(0, -2, 0), Quaternion.identity);
        }
    }

    /// <summary>
    ///     Fonction de mise à jour appelé chaque frame.
    /// </summary>
    public void Update()
    {
        if ((_isInGostMode || _isInDeleteMode) && EventSystem.current != null && !EventSystem.current.IsPointerOverGameObject())
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
                    mousePosition.z = (int) Math.Floor(mousePosition.z) + (localScale.z - 1) / 2;

                    //Si en mode suppression et un batiment est dans la zone.
                    if (_isInDeleteMode && TileArray.AnyBuildingInZone(mousePosition, Vector3.one))
                    {
                        var tileModel = TileArray.FirstBuildingInZone(mousePosition, Vector3.one);
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Suppression
                            GameObject building = tileModel.Building;
                            TileArray.DeleteBuilding(building);
                            GameObject.Destroy(building);
                        }
                        else
                        {
                            // fantome de suppression
                            var buildingMeshRenderer = tileModel.Building.GetComponent<MeshRenderer>();
                            // Enregistrement de la couleur et ajout de la couleur rouge.
                            if (buildingMeshRenderer != null)
                            {
                                var color = buildingMeshRenderer.material.color;
                                if (!color.Equals(new Color(255, 0, 0, 0.5f)))
                                {
                                    _lastHoverGameObjectColor = color;
                                }

                                buildingMeshRenderer.material.color = new Color(255, 0, 0, 0.5f);
                            }

                            var lastHoverGameObjectMeshRenderer = tileModel.Building.GetComponent<MeshRenderer>();
                            if (_lastHoverGameObject != tileModel.Building && lastHoverGameObjectMeshRenderer != null &&
                                _lastHoverGameObjectColor.HasValue)
                            {
                                var color = lastHoverGameObjectMeshRenderer.material.color;
                                if (!color.Equals(_lastHoverGameObjectColor.Value))
                                {
                                    lastHoverGameObjectMeshRenderer.material.color = _lastHoverGameObjectColor.Value;
                                }
                            }
                        }

                        _lastHoverGameObject = tileModel.Building;
                    }
                    else if (_isInGostMode)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Pose du batiment
                            GameObject created = GameObject.Instantiate(_selectedGameObject, mousePosition,
                                Quaternion.identity);

                            // Désactivation temporaire du bâtiment jusqu'à ce que le robot arrive
                            created.SetActive(false);

                            TileArray.SetBuildingInZone(mousePosition, created.transform.localScale, created);
                            TileArray.ClearDoodadsInZone(mousePosition, created.transform.localScale);
                            CleanMod();

                            Debug.Log("Bâtiment posé, robot destination OK");

                            // Création du robot et déplacement jusqu'au batiment
                            var robot = pnjManager.CreateRobot(new Vector3(1, 1, 1));
                            robot.attachedBuilding = created;
                            robot.Move(mousePosition);
                        }
                        else
                        {
                            // Deplacement du batiment.
                            _ghostGameObject.transform.position = mousePosition;

                            if (TileArray.AnyInZone(mousePosition, _ghostGameObject.transform.localScale,
                                new List<TileEnum>() {TileEnum.Water}))
                            {
                                // fantome de suppression
                                var ghostGameObjectMeshRenderer = _ghostGameObject.GetComponent<MeshRenderer>();
                                // Enregistrement de la couleur et ajout de la couleur rouge.
                                if (ghostGameObjectMeshRenderer != null)
                                {
                                    var color = ghostGameObjectMeshRenderer.material.color;
                                    if (!color.Equals(new Color(255, 0, 0, 0.5f)))
                                    {
                                        _ghostGameObjectColor = color;
                                    }

                                    ghostGameObjectMeshRenderer.material.color = new Color(255, 0, 0, 0.5f);
                                }
                            }
                            else
                            {
                                var ghostGameObjectMeshRenderer = _ghostGameObject.GetComponent<MeshRenderer>();
                                if (ghostGameObjectMeshRenderer != null && _ghostGameObjectColor.HasValue)
                                {
                                    var color = ghostGameObjectMeshRenderer.material.color;
                                    if (!color.Equals(_ghostGameObjectColor.Value))
                                    {
                                        ghostGameObjectMeshRenderer.material.color = _ghostGameObjectColor.Value;
                                    }
                                }
                            }
                        }
                    }
                }
        }
    }
}
