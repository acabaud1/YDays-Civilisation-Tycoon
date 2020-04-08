using UnityEngine;

public class ClickActionStone : ClickAction
{
    public override void HandleClick()
    {
        drawer.OnOpen();
    }
}
