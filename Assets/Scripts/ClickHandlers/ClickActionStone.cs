using UnityEngine;

/// <summary>
/// The specific behavior of StonMine when you clicked on it.
/// </summary>
public class ClickActionStone : ClickAction
{

    private string _listName = "StoneMineList";

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
