using System;
using UnityEngine;

/// <summary>
/// This class is used for trigger action on interactable objects
/// </summary>
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

	/// <summary>
	/// Called by the globalManager.
	/// This function listen every click of user and trigger the clickAction of the clicked gameObject.
	/// If the gameObject haven't go ClickAction it do nothing.
	/// </summary>
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
