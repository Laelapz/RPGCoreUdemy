using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeDamageSound : MonoBehaviour
{
    [SerializeField] AudioClip[] _damageClips = null;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.pitch = 1 + Random.Range(-0.2f, 0.3f);

    }

    public void PlayRandomDamageSound()
    {
        if(_damageClips == null) return;

        _audioSource.clip = _damageClips[Random.Range(0, _damageClips.Length)];
        _audioSource.Play();
    }
}
