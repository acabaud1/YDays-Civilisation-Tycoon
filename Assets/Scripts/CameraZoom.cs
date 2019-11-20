using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    //public variables paremeters for Unity inpector inputs
    public float initialFOV;
    public float zoomInFOV;
    public float speed;

    //private variables for script mod
    private float currentFOV;
    private float scrollWheelInput;

    // Use this for initialization
    void Start()
    {
        //set initial FOV at start
        Camera.main.fieldOfView = initialFOV;
    }

    // Update is called once per frame
    void Update()
    {
        scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        //store current field of view value in variable
        currentFOV = Camera.main.fieldOfView;
        if (scrollWheelInput != 0)
        {
            if (currentFOV <= initialFOV && currentFOV >= zoomInFOV)
            {
                Camera.main.fieldOfView += (speed * -scrollWheelInput) * Time.deltaTime;
            }
            else if (currentFOV < zoomInFOV && scrollWheelInput < 0)
            {
                Camera.main.fieldOfView += (speed * -scrollWheelInput) * Time.deltaTime;
            }
            else if (currentFOV > initialFOV && scrollWheelInput > 0)
            {
                Camera.main.fieldOfView += (speed * -scrollWheelInput) * Time.deltaTime;
            }
        }

    }
}
