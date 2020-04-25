using System;
using UnityEngine;

public class ClickableSingleton
{

    private static ClickableSingleton _instance;
    private GameObject _lastClickedGameObject { get; set; }


    private ClickableSingleton()    
    {

    }

    public static ClickableSingleton GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ClickableSingleton();
        }

        return _instance;
    }


    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject target = hit.collider.gameObject;

                if (target != _lastClickedGameObject)
                {
                    if (target.TryGetComponent(out ClickAction action))
                    {
                        action.HandleClick();
                    }
                }
                _lastClickedGameObject = target;
            }
        }
    }
}
