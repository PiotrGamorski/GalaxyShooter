using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosionAudioClip;

    [SerializeField]
    private float _speed = 4f;

    private AudioSource _explosionAudioSource = null;

    public AudioSource ExplosionAudioSource
    {
        get
        {
            if (!_explosionAudioSource)
            {
                _explosionAudioSource = this.gameObject.GetComponent<AudioSource>();
            }
            return _explosionAudioSource;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ExplosionAudioSource.clip = _explosionAudioClip;
        ExplosionAudioSource.Play();
        Destroy(this.gameObject, 2.8f);
    }

    void Update()
    {
        //this.transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
}
