using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField] // 0 = TripleShot, 1 = Speed, 2 = Shields
    private int _powerUpID;

    [SerializeField]
    private AudioClip _powerUpAudioClip;

    [SerializeField]
    private GameObject _explosionPrefab;

    private AudioSource _powerUpAudioSource = null;

    public AudioSource PowerUpAudioSource
    {
        get
        {
            if (!_powerUpAudioSource)
            {
                _powerUpAudioSource = this.gameObject.GetComponent<AudioSource>();
            }
            return _powerUpAudioSource;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PowerUpAudioSource.clip = _powerUpAudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (this.transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PowerUpAudioSource.Play();

            Player player = other.transform?.GetComponent<Player>();

            switch (this._powerUpID)
            {
                case 0:
                    player.TripleShotActive();
                    Destroy(this.gameObject.GetComponent<CircleCollider2D>());
                    break;
                case 1:
                    player.SpeedBoostActivate();
                    Destroy(this.gameObject.GetComponent<BoxCollider2D>());
                    break;
                case 2:
                    player.ShiledActivate();
                    Destroy(this.gameObject.GetComponent<BoxCollider2D>());
                    break;
            }
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            StartCoroutine(DestroyPowerUpOnPlayerRoutine());
        }

        if (other.tag == "Laser" && other.transform.GetComponent<Laser>().IsEnemyLaser == false)
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 2.8f);

            switch (this._powerUpID)
            {
                case 0:
                    this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    break;
                case 1:
                case 2:
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    break;
            }

            StartCoroutine(DestroyPowerUpOnLaserRoutine());
        }
    }

    private IEnumerator DestroyPowerUpOnPlayerRoutine()
    {
        yield return new WaitForSeconds(_powerUpAudioClip.length);
        Destroy(this.gameObject);
    }

    private IEnumerator DestroyPowerUpOnLaserRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}
