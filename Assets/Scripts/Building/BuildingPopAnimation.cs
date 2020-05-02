using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPopAnimation : MonoBehaviour
{
    private Animation animation;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animation>();
        Debug.Log(animation.clip);
        if (!animation.isPlaying)
        {
            animation.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
