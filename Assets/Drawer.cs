using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    bool open = false;
    bool mouseOver = false;
    private Animator drawerAnimationController;
    private List<Delegate> callbacks = new List<Delegate>();
    public GameObject BottomButtons;
    public GameObject BuildingList;

    void Start()
    {
        drawerAnimationController = GetComponent<Animator>();
    }

    public void Update()
    {
        if (drawerAnimationController && Input.GetMouseButtonDown(0) && open && !mouseOver)
        {
            ReactivateButtons();
            drawerAnimationController.Play("DrawerRewind");
            open = false;
        }
    }

    private void ReactivateButtons()
    {
        if (BottomButtons)
        {
            var bottomButtons = BottomButtons.GetComponentsInChildren<Button>();
            foreach (Button btn in bottomButtons)
            {
                btn.interactable = true;
            }
        }
    }

    public void OnOpen()
    {
        if (drawerAnimationController && !open)
        {
            drawerAnimationController.Play("DrawerAnim");
            open = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        mouseOver = true;
        Debug.Log("Mouse is over the drawer !");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        Debug.Log("Mouse is not over the drawer !");
    }

}
