using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 1.5f;
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Vector3 RandomizeItensity = new Vector3(0.5f, 0, 0);

    void Start()
    {
        Destroy(gameObject, DestroyTime);
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeItensity.x, RandomizeItensity.x),
            Random.Range(-RandomizeItensity.y, RandomizeItensity.y),
            Random.Range(-RandomizeItensity.z, RandomizeItensity.z)
        );
    }
}
