using UnityEngine;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// This script is used for hide the button when the drawer is open.
/// It use UnixRx lib
/// </summary>
public class BottomButtonsActivate : MonoBehaviour
{
    public GameObject drawer;
    // Start is called before the first frame update
    void Start()
    {
        if (drawer.TryGetComponent<Drawer>(out Drawer drawerScript))
        {
            Button[] buttons = gameObject.GetComponentsInChildren<Button>();
            drawerScript.Obs.Subscribe(drawerState =>
            {
                foreach (Button btn in buttons)
                {
                    btn.interactable = !drawerState;
                }
            });
        }       
    }

}
