using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 PositionCamera;
    public GameObject Camera;
    public List<Resources> Stock;

    public void Save()
    {
        PositionCamera = Camera.transform.position;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Save();
    }
}
