using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPopAnimation : MonoBehaviour
{
    private Animation animation;
    public string Animation = "BuildingPop1";
    private string LastPlayedAnimation;
    private bool isAnimationPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animation>();
        PlayAnimation(Animation);
        if (isAnimationPlaying && !animation.isPlaying)
        {
            OnAnimationEnd();
        }
    }

    public void PlayAnimation(string animationName)
    {
        animation.Play(animationName);
        LastPlayedAnimation = animationName;
        isAnimationPlaying = true;
    }

    private void OnAnimationEnd()
    {
        if (LastPlayedAnimation == "BuildingDepop1" || LastPlayedAnimation == "BuildingDepop2")
        {
            Destroy(gameObject);
            isAnimationPlaying = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
