using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject resourceManagerGameObject = GameObject.Find("ResourceManager");
        ResourceManager resourceManager = resourceManagerGameObject.GetComponent<ResourceManager>();
        var iron = resourceManager.Get(typeof(Iron));
        Debug.Log(iron.Name);
        Debug.Log(iron.Quantity);
        resourceManager.Add(typeof(Iron), 10);
        Debug.Log(iron.Quantity);
        iron.Quantity = iron.Quantity - 5;
        Debug.Log(iron.Quantity);
  }

    // Update is called once per frame
    void Update()
    {

    }
}
