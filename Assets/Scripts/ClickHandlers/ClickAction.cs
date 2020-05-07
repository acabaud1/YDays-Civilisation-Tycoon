using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Abstract Class
/// It define the default behavior of a interactable gameObject.
/// </summary>
public abstract class ClickAction : MonoBehaviour
{
    protected Drawer drawer;
    protected GameObject bottomButtons;

    public void Start()
    {
        GameObject drawerGameObject = GameObject.Find("Drawer");
        bottomButtons = GameObject.Find("BottomButtons");

        drawer = drawerGameObject.GetComponent<Drawer>();
    }

    public virtual void HandleClick()
    {
        drawer.OnOpen();
    }

}
