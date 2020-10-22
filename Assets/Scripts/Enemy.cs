using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    [SerializeField]
    private int _enemyID = 0;

    [SerializeField]
    private AudioClip _enemyAudioClip;

    [SerializeField]
    private GameObject _laserPrefab;

    private int _pointsMultiplier = 10;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;
    private List<Laser> lasersList = new List<Laser>();
    private Player _player = null;
    private Animator _animator = null;
    private AudioSource _enemyAudioSource = null;

    public int PointsBasedOnEnemy
    {
        get => (this._enemyID + 1) * _pointsMultiplier;
    }
    public Player Player
    {
        get
        {
            if (!_player)
            {
                _player = GameObject.Find("Player")?.GetComponent<Player>();
            }
            return _player;
        }
    }

    public Animator Animator
    {
        get
        {
            if (!_animator)
            {
                _animator = this.GetComponent<Animator>();
            }
            return _animator;
        }
    }

    public AudioSource EnemyAudioSource
    {
        get
        {
            if (!_enemyAudioSource)
            {
                _enemyAudioSource = this.gameObject.GetComponent<AudioSource>();
            }
            return _enemyAudioSource;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyAudioSource.clip = _enemyAudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(-2.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, this.transform.position, Quaternion.identity);
            lasersList = new List<Laser>(enemyLaser.GetComponentsInChildren<Laser>());
            lasersList.ForEach(e => { e.Speed = 5.5f; e.AssignEnemyLaser(); });
        }
    }

    private void CalculateMovement()
    {
        if (this.transform.position.y < -5.5f)
        {
            float randomX = Random.Range(-8f, 8f);
            this.transform.position = new Vector3(randomX, 8, 0);
        }
        this.transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Player")
        {
            Player player = other?.transform.GetComponent<Player>();

            if (player != null)
            {
                if (this.gameObject.transform.position.x > other.gameObject.transform.position.x)
                {
                    player.isHitFromRight = true;
                    if (player.Lives < 3)
                        player.isHitFromLeft = true;
                }
                else if (this.gameObject.transform.position.x <= other.gameObject.transform.position.x)
                {
                    player.isHitFromLeft = true;
                    if (player.Lives < 3)
                        player.isHitFromRight = true;
                }

                player.Damage();
                EnemyAudioSource.Play();
            }  


            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.Animator.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.8f);
        }
        
        if (other.tag == "Laser" && other.transform.GetComponent<Laser>().IsEnemyLaser == false)
        {
            Destroy(other.gameObject);
            if (Player != null)
            {
                Player.AddScore(PointsBasedOnEnemy);
                Player.NumberOfDestroyedEnemies = _player.NumberOfDestroyedEnemies + 1;
            }
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.Animator.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.8f);

            EnemyAudioSource.Play();
        }
    }
}
