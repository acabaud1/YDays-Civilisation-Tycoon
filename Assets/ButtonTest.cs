using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public GameObject player;

    public void OnClick()
    {
        player = GameObject.Find("player");
        player.GetComponent<PlayerInfos>().wood += 10;
    }
}
