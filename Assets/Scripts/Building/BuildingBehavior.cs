using UnityEngine;

public class BuildingBehavior : MonoBehaviour
{
    public Material DefaultMaterial;
    public Material ErrorMaterial;

    private bool isInError = false;
    private MeshRenderer meshRenderer;


    public bool IsInError()
    {
        return isInError;
    }

    public void ToggleMaterial()
    {
        if (isInError)
        {
            meshRenderer.material = ErrorMaterial;
        }
        else
        {
            meshRenderer.material = DefaultMaterial;
        }

        isInError = !isInError;
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
}
