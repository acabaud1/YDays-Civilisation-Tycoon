using UnityEngine;

public abstract class ClickAction : MonoBehaviour
{
    protected Drawer drawer;

    public void Start()
    {
        GameObject drawerGameObject = GameObject.Find("Drawer");

        drawer = drawerGameObject.GetComponent<Drawer>();

    }

    public virtual void HandleClick()
    {
    }
}
