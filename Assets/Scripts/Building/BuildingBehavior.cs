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

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
