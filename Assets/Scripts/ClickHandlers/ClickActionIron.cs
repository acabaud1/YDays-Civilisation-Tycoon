using UnityEngine;

public class ClickActionIron : ClickAction
{

    public override void HandleClick()
    {
        drawer.OnOpen();
    }
}
