using UnityEngine;

public class ClickActionWood : ClickAction
{
    public override void HandleClick()
    {
        drawer.OnOpen();
    }
}
