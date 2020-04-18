using UnityEngine;

public class ResourceUndergroundMineIncome : MonoBehaviour
{

    public ResourcesGame[] Resources;
    public float msBetweenGain = 2000;
    public int quantity = 1;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GainResources", 0, msBetweenGain);
    }

    private void GainResources()
    {
        ResourceManager.GetInstance().Add(Resources[Random.Range(0, Resources.Length)].GetType(), quantity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
