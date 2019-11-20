﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 20.0f;
    public float borderThickness = 10.0f;
    public float scrollSpeed = 20.0f;

    private float inputHor;
    private float inputVer;

    public Vector2 xLimit;
    public Vector2 zLimit;
    public Vector2 yLimit;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
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

        //Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100.0f * Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, yLimit.x, yLimit.y);
        pos.x = Mathf.Clamp(pos.x, xLimit.x, xLimit.y);
        pos.z = Mathf.Clamp(pos.z, zLimit.x, zLimit.y);

        transform.position = pos;
    }
}
