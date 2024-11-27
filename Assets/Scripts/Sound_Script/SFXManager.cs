using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] private AudioSource SFXObject;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawnar gameObject
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        //atribuir audioclip
        audioSource.clip = audioClip;

        //atribuir volume
        audioSource.volume = volume;

        //tocar som
        audioSource.Play();

        //obter duracao do som
        float clipLength = audioSource.clip.length;

        //destruir gameObject do audio 
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        //atribuir valor aleatorio
        int rand = Random.Range(0, audioClip.Length);
        
        //spawnar gameObject
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        //atribuir audioclip
        audioSource.clip = audioClip[rand];

        //atribuir volume
        audioSource.volume = volume;

        //tocar som
        audioSource.Play();

        //obter duracao do som
        float clipLength = audioSource.clip.length;

        //destruir gameObject do audio 
        Destroy(audioSource.gameObject, clipLength);
    }
}
