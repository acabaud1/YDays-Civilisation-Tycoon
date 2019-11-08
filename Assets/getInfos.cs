using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class getInfos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("player");
        int playerWood = player.GetComponents<PlayerInfos>()[0].wood;

        this.GetComponent<TextMeshProUGUI>().text = playerWood.ToString();
    }
}
