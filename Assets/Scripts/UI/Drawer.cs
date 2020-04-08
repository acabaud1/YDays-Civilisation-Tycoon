using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class containt the behavior of the drawer
/// </summary>
public class Drawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _open = false;
    private bool _mouseOver = false;
    private Animator _drawerAnimationController;
    public GameObject BottomButtons;

    /// <summary>
    /// Get the animator component in Drawer prefab GameObject.
    /// </summary>
    void Start()
    {
        _drawerAnimationController = GetComponent<Animator>();
    }

    /// <summary>
    /// Click away listener behavior for close the drawer.
    /// </summary>f
    public void Update()
    {
        if (_drawerAnimationController && Input.GetMouseButtonDown(0) && _open && !_mouseOver)
        {
            Close();
        }
    }

    /// <summary>
    /// Close the drawer.
    /// </summary>
    public void Close()
    {
        ReactivateButtons();
        _drawerAnimationController.Play("DrawerRewind");
        _open = false;
    }

    /// <summary>
    /// Desactivate all buttons in the bottom of the screen when you activate the drawer.
    /// </summary>
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

    /// <summary>
    /// Open the drawer with the DrawerAnim state in animator
    /// </summary>
    public void OnOpen()
    {
        if (_drawerAnimationController && !_open)
        {
            var bottomButtons = BottomButtons.GetComponentsInChildren<Button>();
            foreach (Button btn in bottomButtons)
            {
                btn.interactable = false;
            }
            _drawerAnimationController.Play("DrawerAnim");
            _open = true;
        }
    }

    /// <summary>
    /// Implementation of IPointerEnterHandler.OnPointerEnter
    /// </summary>
    /// <param name="eventData">Unity's event system of eventData</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseOver = true;
    }

    /// <summary>
    /// Implementation of IPointerExitHandler.OnPointerExit
    /// </summary>
    /// <param name="eventData">Unity's event system of eventData</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseOver = false;
    }

}
