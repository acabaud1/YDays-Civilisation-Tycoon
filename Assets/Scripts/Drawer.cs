using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// This class containt the behavior of the drawer
public class Drawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    bool open = false;
    bool mouseOver = false;
    private Animator drawerAnimationController;
    public GameObject BottomButtons;

    /// Get the animator component in Drawer prefab GameObject.
    void Start()
    {
        drawerAnimationController = GetComponent<Animator>();
    }

    /// Click away listener behavior for close the drawer.
    public void Update()
    {
        if (drawerAnimationController && Input.GetMouseButtonDown(0) && open && !mouseOver)
        {
            ReactivateButtons();
            drawerAnimationController.Play("DrawerRewind");
            open = false;
        }
    }

    /// Desactivate all buttons in the bottom of the screen when you activate the drawer.
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

    /// Open the drawer with the DrawerAnim state in animator
    public void OnOpen()
    {
        if (drawerAnimationController && !open)
        {
            drawerAnimationController.Play("DrawerAnim");
            open = true;
        }
    }

    /// Implementation of IPointerEnterHandler.OnPointerEnter
    public void OnPointerEnter(PointerEventData eventData) 
    {
        mouseOver = true;
    }

    /// Implementation of IPointerExitHandler.OnPointerExit
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

}
