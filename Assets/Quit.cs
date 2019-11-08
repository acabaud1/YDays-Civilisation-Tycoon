using UnityEngine;

public class Quit : MonoBehaviour
{
    public void OnQuit()
    {
        Debug.Log("Cocou tout le monde ! ");
        Application.Quit();
    }
}
