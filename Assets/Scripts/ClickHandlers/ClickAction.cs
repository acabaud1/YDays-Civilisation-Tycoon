using UnityEngine;
using UnityEngine.EventSystems;

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
        // bottomButtons.SetActive(false);
    }

}
