using UnityEngine;

public class IncomingBuilding : MonoBehaviour
{

    public string ResourceName;
    private ParticleSystemAnimationType.

    void Start()
    {
        try
        {
            
        } 
        catch (System.NullReferenceException e)
        {
            Debug.LogError($"Le type {ResourceName} n'existe pas.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
