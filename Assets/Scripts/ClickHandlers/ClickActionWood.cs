using UnityEngine;

/// <summary>
/// The specific behavior of LumberJack when you clicked on it.
/// </summary>
public class ClickActionWood : ClickAction
{
    private string _listName = "LumberjackList";

    public override void HandleClick()
    {
        base.HandleClick();

        Transform allTransform = drawer.GetComponentInChildren<Transform>();

        foreach (Transform child in allTransform)
        {
            child.gameObject.SetActive(false);
            if (child.name == _listName)
            {
                child.gameObject.SetActive(true);
            }
        }

    }
}
