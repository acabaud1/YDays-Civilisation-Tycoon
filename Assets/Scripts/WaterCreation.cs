using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LPWAsset;

public class WaterCreation : MonoBehaviour
{

    public MapGeneration map;
    // public LPWAsset lowPolyAsset;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(map.width / 2, 0, map.height / 2);
    }
}
