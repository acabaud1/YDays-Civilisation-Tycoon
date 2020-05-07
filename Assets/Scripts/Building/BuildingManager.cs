using System;
using System.Collections.Generic;
using Map;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Assets.Scripts.Building
{
    public class BuildingManager
    {
        /// <summary>
        /// Obtient ou définit la liste des cases.
        /// </summary>
        public TileModel[,] TileArray { get; set; }

        /// <summary>
        /// Obtient ou définit le ressource manager.
        /// </summary>
        public ResourceManager ResourceManager { get; set; }

        /// <summary>
        /// Obtient ou définit le Hub.
        /// </summary>
        public GameObject Hub { get; set; }

        private bool _isInDeleteMode;
        private bool _isInGostMode;

        private GameObject _lastHoverGameObject;
        private GameObject _selectedGameObject;
        private GameObject _ghostGameObject;
        private GameObject _ghostBlockedGameObject;

        private Building _ghostBuildingManager;

        [FormerlySerializedAs("LayerMask")] public LayerMask LayerMask;
        public PNJManager PnjManager;

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="BuildingManager" />
        /// </summary>
        private BuildingManager()
        {
        }

        /// <summary>
        /// Instance de la gestion des bâtiments.
        /// </summary>
        private static BuildingManager _instance;

        /// <summary>
        /// Récupère l'instance de building manager.
        /// </summary>
        /// <returns>Building Manager.</returns>
        public static BuildingManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BuildingManager();
            }

            return _instance;
        }

        public bool IsInGhostMode()
        {
            return _isInGostMode;
        }

        /// <summary>
        ///     Active / Désactive le mode de suppression.
        /// </summary>
        public void ToggleDeleteMod()
        {
            GameObject.Destroy(_ghostGameObject);
            GameObject.Destroy(_ghostBlockedGameObject);
            _ghostGameObject = null;
            _ghostBlockedGameObject = null;
            _ghostBuildingManager = null;
            _isInGostMode = false;
            _isInDeleteMode = !_isInDeleteMode;
        }

        /// <summary>
        ///     Supprime les batiments.
        /// </summary>
        public void CleanMod()
        {
            GameObject.Destroy(_ghostGameObject);
            GameObject.Destroy(_ghostBlockedGameObject);
            _ghostGameObject = null;
            _ghostBlockedGameObject = null;
            _isInGostMode = false;
            _isInDeleteMode = false;
            _ghostBuildingManager = null;
        }

        /// <summary>
        ///     Définit le bâtiment à placer.
        /// </summary>
        /// <param name="buildingManager">Gestionnaire du bâtiment.</param>
        public void SetBuilding(GameObject buildingManager)
        {
            var script = buildingManager.GetComponent<Building>();
            if (script != null)
            {
                CleanMod();
                _ghostBuildingManager = script;
                if (_ghostGameObject != null) GameObject.Destroy(_ghostGameObject);
                if (_ghostBlockedGameObject != null) GameObject.Destroy(_ghostBlockedGameObject);
                _selectedGameObject = script.FullBuilding;
                _isInGostMode = true;
                _ghostGameObject = GameObject.Instantiate(script.GhostBuilding, new Vector3(0, -2, 0), Quaternion.identity);
                _ghostBlockedGameObject =
                    GameObject.Instantiate(script.GhostBuildingBlocked, new Vector3(0, -2, 0), Quaternion.identity);
                _ghostBlockedGameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Définit si l'on peut poser le batiment.
        /// </summary>
        /// <returns>Peux on poser le batiment.</returns>
        private bool HasResourcesToPlace()
        {
            bool result = true;
            foreach (var buildingCost in BuildingCost.GetCost(_ghostBuildingManager.BuildingEnum))
            {
                if (!ResourceManager.CanAddAndDistribute(buildingCost.Resource, -buildingCost.Quantity))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Enleve les ressources du stock.
        /// </summary>
        private void TakeResourcesToPlace()
        {
            var resourceManager = ResourceManager;
            foreach (var buildingCost in BuildingCost.GetCost(_ghostBuildingManager.BuildingEnum))
            {
                resourceManager.AddAndDistribute(buildingCost.Resource, -buildingCost.Quantity);
            }
        }

        /// <summary>
        ///     Fonction de mise à jour appelé chaque frame.
        /// </summary>
        public void Update()
        {
            if ((_isInGostMode || _isInDeleteMode) && EventSystem.current != null &&
                !EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                var ray = Camera.main?.ScreenPointToRay(Input.mousePosition);
                if (ray.HasValue && Physics.Raycast(ray.Value, out hit, 200.0f, LayerMask))
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
                                var buildingBehaviorScript = tileModel.Building.GetComponent<BuildingBehavior>();

                                if (!buildingBehaviorScript.IsInError())
                                {
                                    buildingBehaviorScript.ToggleMaterial();
                                }

                                if (_lastHoverGameObject != tileModel.Building && buildingBehaviorScript.IsInError())
                                {
                                    buildingBehaviorScript.ToggleMaterial();
                                }
                            }

                            _lastHoverGameObject = tileModel.Building;
                        }
                        else if (_isInGostMode)
                        {
                            if (Input.GetMouseButtonDown(0) && !TileArray.AnyInZone(mousePosition,
                                _ghostGameObject.transform.localScale,
                                new List<TileEnum>() {TileEnum.Water}) && HasResourcesToPlace())
                            {
                                // Pose du batiment
                                GameObject created = GameObject.Instantiate(_selectedGameObject, mousePosition,
                                    Quaternion.identity);

                                TakeResourcesToPlace();


                                TileArray.SetBuildingInZone(mousePosition, created.transform.localScale, created, _ghostBuildingManager.BuildingEnum);
                                TileArray.ClearDoodadsInZone(mousePosition, created.transform.localScale);
                                CleanMod();

                                if (Hub)
                                {
                                    // Désactivation temporaire du bâtiment jusqu'à ce que le robot arrive
                                    created.SetActive(false);
                                    // Création du robot et déplacement jusqu'au batiment

                                    var hubPos = Hub.transform.position;
                                    var robotPos = new Vector3(hubPos.x, hubPos.y, hubPos.z);
                                
                                    var robot = PnjManager.CreateRobot(robotPos);
                                    robot.attachedBuilding = created;
                                    robot.Move(mousePosition);
                                }
                            }
                            else
                            {
                                // Deplacement du batiment.
                                _ghostGameObject.transform.position = mousePosition;
                                _ghostBlockedGameObject.transform.position = mousePosition;

                                if (TileArray.AnyInZone(mousePosition, _ghostGameObject.transform.localScale,
                                    new List<TileEnum>() {TileEnum.Water}))
                                {
                                    _ghostGameObject.SetActive(false);
                                    _ghostBlockedGameObject.SetActive(true);
                                }
                                else
                                {
                                    _ghostGameObject.SetActive(true);
                                    _ghostBlockedGameObject.SetActive(false);
                                }
                            }
                        }
                    }
            }
        }
    }
}