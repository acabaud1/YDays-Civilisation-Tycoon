using UnityEngine;

public class ClickActionIron : ClickAction
{

    private string _listName = "IronMineList";

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
