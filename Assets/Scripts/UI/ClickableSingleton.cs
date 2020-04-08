using UnityEngine;

public class ClickableSingleton
{

    private static ClickableSingleton _instance; 


    private ClickableSingleton()
    {

    }

    public static ClickableSingleton GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ClickableSingleton();
        }

        return _instance;
    }


    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}
