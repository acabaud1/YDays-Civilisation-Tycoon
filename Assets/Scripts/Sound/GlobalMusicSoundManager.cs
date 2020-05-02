using UnityEngine;

public class GlobalMusicSoundManager : MonoBehaviour
{
    private AudioSource AudioSource;
    public AudioClip[] AudioClips;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void playNextMusic()
    {
        AudioSource.clip = AudioClips[index];
        AudioSource.Play();
        index++;
        if (index == AudioClips.Length)
        {
            index = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!AudioSource.isPlaying)
        {
            playNextMusic();
        }
    }
}
