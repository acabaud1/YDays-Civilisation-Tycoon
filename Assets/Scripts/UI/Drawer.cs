using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// This class containt the behavior of the drawer
/// </summary>
public class Drawer : MonoBehaviour
{
    private bool _open = false;
    public ReactiveProperty<bool> Obs { get; set; }
    private Animator _drawerAnimationController;

    public bool IsOpen()
    {
        return _open;
    }

    /// <summary>
    /// Get the animator component in Drawer prefab GameObject.
    /// </summary>
    void Start()
    {
        _drawerAnimationController = GetComponent<Animator>();
        Obs = new ReactiveProperty<bool>(_open);
    }

    /// <summary>
    /// Click away listener behavior for close the drawer.
    /// </summary>f
    public void Update()
    {
    }

    /// <summary>
    /// Close the drawer.
    /// </summary>
    public void Close()
    {
        _drawerAnimationController.Play("DrawerRewind");
        _open = false;
        Obs.Value = false;
    }

    /// <summary>
    /// Open the drawer with the DrawerAnim state in animator
    /// </summary>
    public void OnOpen()
    {
        if (_drawerAnimationController)
        {
            Transform allTransform = gameObject.GetComponentInChildren<Transform>();

            foreach (Transform child in allTransform)
            {
                child.gameObject.SetActive(false);
                if (child.name == "BuildingList")
                {
                    child.gameObject.SetActive(true);
                }
            }

            _drawerAnimationController.Play("DrawerAnim");
            _open = true;
            Obs.Value = true;
        }
    }
    
}
