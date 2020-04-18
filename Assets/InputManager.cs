using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public GameObject Drawer;

    private Drawer _drawerScript;

    // Start is called before the first frame update
    void Start()
    {
        _drawerScript = Drawer.GetComponent<Drawer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_drawerScript.IsOpen())
            {
                _drawerScript.Close();
            }

            if (BuildingManager.GetInstance().IsInGhostMode())
            {
                BuildingManager.GetInstance().CleanMod();
            }
        }
    }
}
