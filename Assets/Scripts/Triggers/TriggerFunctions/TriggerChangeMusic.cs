using UnityEngine;
using System.Collections;

public class TriggerChangeMusic : TriggerBaseFunction 
{

    public AudioSource audioSource;
    public AudioClip audioClip;
    private AudioClip oldClip;
	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    public override void DoAction(Mover mover)
    {
        if (audioSource.clip != audioClip)
        {
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        /*
        if (oldClip == null && audioSource.clip != audioClip)
        {
            oldClip = audioSource.clip;
        }

        if (audioSource.clip == oldClip)
        {
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = oldClip;
            audioSource.Play();
        }
         * */
        base.DoAction(mover);
    }
}
