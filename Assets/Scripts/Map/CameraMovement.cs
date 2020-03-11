using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestion des mouvement de caméra.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 20.0f;
    public float borderThickness = 5.0f;
    public float scrollSpeed = 20.0f;

    private float inputHor;
    private float inputVer;

    public Vector2 xLimit;
    public Vector2 zLimit;
    public Vector2 yLimit;

    private Vector3 lastDragPosition;
    private bool isPanning;
    public float panSpeed;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        Vector3 pos = transform.position;
        if (!isPanning)
        {
            inputHor = Input.GetAxis("Horizontal");
            inputVer = Input.GetAxis("Vertical");

            //Horizontal axis
            if (inputVer > 0 || Input.mousePosition.y >= Screen.height - borderThickness)
            {
                pos.z += moveSpeed * Time.deltaTime;
            }
            if (inputVer < 0 || Input.mousePosition.y <= borderThickness)
            {
                pos.z -= moveSpeed * Time.deltaTime;
            }

            //Vertical axis
            if (inputHor > 0 || Input.mousePosition.x >= Screen.width - borderThickness)
            {
                pos.x += moveSpeed * Time.deltaTime;
            }
            if (inputHor < 0 || Input.mousePosition.x <= borderThickness)
            {
                pos.x -= moveSpeed * Time.deltaTime;
            } 
        }


        if (Input.GetMouseButtonDown(0))
        {
            lastDragPosition = Input.mousePosition;
            isPanning = true;
        }
        if (Input.GetMouseButton(0))
        {
            var delta = lastDragPosition - Input.mousePosition;
            pos += new Vector3(delta.x * Time.deltaTime * panSpeed, 0, delta.y * Time.deltaTime * panSpeed);
            lastDragPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

        //Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100.0f * Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, yLimit.x, yLimit.y);
        pos.x = Mathf.Clamp(pos.x, xLimit.x, xLimit.y);
        pos.z = Mathf.Clamp(pos.z, zLimit.x, zLimit.y);

        transform.position = pos;
    }
}
