using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _playerShieldVisualizer;

    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private float _fireRate = 0.1f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _speedMultiplier = 2;

    [SerializeField]
    private int _shieldLives;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private bool _isPlayerOneDead = false;

    [SerializeField]
    private bool _isPlayerTwoDead = false;

    [SerializeField]
    private AudioClip _laserAudioClip;

    [SerializeField]
    private GameObject _explosionPrefab;

    private float _canFire = -1;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _singleTripleShot = false;
    private int _numberOfDestroyedEnemies = 0;
    private float _shieldDuration;
    private int _bestScore = 0;
    private SpawnManager _spawnManager = null;
    private UIManager _uIManager = null;
    private AudioSource _laserAudioSource = null;
    private Game_Manager _gameManager = null;
    private PlayersInfoOnDeath _playersInfoOnDeath = null;
    private Animator _animator = null;

    public bool isHitFromLeft = false;
    public bool isHitFromRight = false;
    public bool IsHitByEnemyLaser = false;
    public bool IsPlayerOne = false;
    public bool IsPlayerTwo = false;


    public int Lives
    {
        get => _lives;
    }

    public int NumberOfDestroyedEnemies
    {
        get => _numberOfDestroyedEnemies;
        set
        {
            _numberOfDestroyedEnemies = value;
        }
    }

    public bool SingleTripleShot
    {
        get
        {
            if (_numberOfDestroyedEnemies < 3)
            {
                return _singleTripleShot;
            }
            else
            {
                _singleTripleShot = true;
                return _singleTripleShot;
            }
        }
    }

    public Animator Animator
    {
        get
        {
            if (!_animator)
            {
                _animator = this.gameObject.GetComponent<Animator>();
            }
            return _animator;
        }
    }

    public SpawnManager SpawnManager
    {
        get
        {
            if (!_spawnManager)
            {
                _spawnManager = GameObject.FindGameObjectWithTag("Spawn_Manager").GetComponent<SpawnManager>();
            }
            return _spawnManager;
        }
    }

    public UIManager UIManager
    {
        get
        {
            if (!_uIManager)
            {
                _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            }
            return _uIManager;
        }
    }

    public AudioSource LaserAudioSource
    {
        get
        {
            if (!_laserAudioSource)
            {
                _laserAudioSource = this.gameObject?.GetComponent<AudioSource>();
            }
            return _laserAudioSource;
        }
    }

    public Game_Manager GameManager
    {
        get
        {
            if (!_gameManager)
            {
                _gameManager = GameObject.Find("Canvas")?.GetComponent<Game_Manager>();
            }
            return _gameManager;
        }
    }

    public PlayersInfoOnDeath PlayersInfoOnDeath
    {
        get
        {
            if (!_playersInfoOnDeath)
            {
                _playersInfoOnDeath = GameObject.Find("Canvas").GetComponent<PlayersInfoOnDeath>();
            }
            return _playersInfoOnDeath;
        }
    }

    public bool IsPlayerOneDead
    {
        get
        {
            if (IsPlayerOne)
            {
                if (_lives < 1)
                    return _isPlayerOneDead = true;
                else return false;
            }
            else
                return false;
        }
    }

    public bool IsPlayerTwoDead
    {
        get
        {
            if (IsPlayerTwo)
            {
                if (_lives < 1)
                    return _isPlayerTwoDead = true;
                else
                    return false;
            }
            else
                return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.BestScore = PlayerPrefs.GetInt("HighScore", 0);

        this.LaserAudioSource.clip = _laserAudioClip;
        if (GameManager?.isCoOpMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerOne == true)
        {
            AnimatePlayerOneMovement();
            CalculateMovementForPlayerOne();
            if (Input.GetKey(KeyCode.Space) && Time.time > _canFire && IsPlayerOne == true)
            {
                FireLaser();
            }
        }

        if (IsPlayerTwo == true)
        {
            CalculateMovementForPlayerTwo();
            if (Input.GetKey(KeyCode.Return) && Time.time > _canFire && IsPlayerTwo == true)
            {
                FireLaser();
            }
        }
    }

    private void CalculateMovementForPlayerOne()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f));

        if (transform.position.x > 14.5f)
        {
            transform.position = new Vector3(-14.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -14.5f)
        {
            transform.position = new Vector3(14.5f, transform.position.y, 0);
        }
    }

    private void CalculateMovementForPlayerTwo()
    {
        if (Input.GetKey(KeyCode.J))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
            this.Animator.SetBool("Turn_Left", true);
        }
        else
        {
            this.Animator.SetBool("Turn_Left", false);
        }

        if (Input.GetKey(KeyCode.L))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
            this.Animator.SetBool("Turn_Right", true);
        }
        else
        {
            this.Animator.SetBool("Turn_Right", false);
        }

        if (Input.GetKey(KeyCode.I))
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.K))
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f));

        if (transform.position.x > 14.5f)
        {
            transform.position = new Vector3(-14.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -14.5f)
        {
            transform.position = new Vector3(14.5f, transform.position.y, 0);
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive || SingleTripleShot)
        {
            Instantiate(_tripleShotPrefab, this.transform.position, Quaternion.identity);
            NumberOfDestroyedEnemies = 0;
            _singleTripleShot = false;
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        LaserAudioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _shieldLives = _shieldLives - 1;
            _isShieldActive = (_shieldLives > 0) ? true : false;
            _playerShieldVisualizer.SetActive(_isShieldActive);
            return;
        }

        _lives -= 1;

        if (IsHitByEnemyLaser == true)
        {
            StartCoroutine(BothEnginesFailureByEnemyLaser(IsHitByEnemyLaser));
        }
        else
        {
            StartCoroutine(RightEngineFailureRoutine());
            StartCoroutine(LeftEngineFailureRoutine());
        }

        UIManager.UpdateLives(_lives, IsPlayerOne, IsPlayerTwo);

        if (UIManager.GameManager.isCoOpMode == false)
        {
            if (IsPlayerOne && IsPlayerOneDead)
            {
                OnPlayerDeathProcedure();
            }
        }

        if (UIManager.GameManager.isCoOpMode == true) 
        {
            if (IsPlayerOne == true && IsPlayerOneDead)
            {
                SendDataBeforeDeathOfPlayerOne();
                OnPlayerDeathProcedureCoOpMode();
            }

            if (IsPlayerTwo == true && IsPlayerTwoDead)
            {
                SendDataBeforeDeathOfPlayerTwo();
                OnPlayerDeathProcedureCoOpMode();
            }

            RestartLevelForCoOpMode();
        }
    }

    private void OnPlayerDeathProcedure()
    {
        UIManager.GameOverSequence();
        SpawnManager.OnPlayerDeath();
        Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
        CheckForBestScore();
        Destroy(this.transform.GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.15f);
    }

    private void OnPlayerDeathProcedureCoOpMode()
    {
        Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.transform.GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.15f);
    }

    private void SendDataBeforeDeathOfPlayerOne()
    {
        PlayersInfoOnDeath.IsPlayerOneDeadInfo = this.IsPlayerOneDead;
    }

    private void SendDataBeforeDeathOfPlayerTwo()
    {
        PlayersInfoOnDeath.IsPlayerTwoDeadInfo = this.IsPlayerTwoDead;
    }

    private void RestartLevelForCoOpMode()
    {
        if (PlayersInfoOnDeath.IsPlayerOneDeadInfo == true && PlayersInfoOnDeath.IsPlayerTwoDeadInfo == true)
        {
            SpawnManager.OnPlayerDeath();
            UIManager.GameOverSequence();
        }
    }

    private void AnimatePlayerOneMovement()
    {
        if (Input.GetKey(KeyCode.A))
            this.Animator.SetBool("Turn_Left", true);
        else
            this.Animator.SetBool("Turn_Left", false);

        if (Input.GetKey(KeyCode.D))
            this.Animator.SetBool("Turn_Right", true);
        else
            this.Animator.SetBool("Turn_Right", false);
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActivate()
    {
        _isSpeedBoostActive = true;
        _speed = _speed * _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(4.0f);
        _isSpeedBoostActive = false;
        _speed = _speed / _speedMultiplier;
    }

    public void ShiledActivate()
    {
        _isShieldActive = true;
        _shieldLives = 2;
        _playerShieldVisualizer.SetActive(_isShieldActive);
        StartCoroutine(ShieldPowerUpDownRoutine());
    }

    IEnumerator ShieldPowerUpDownRoutine()
    {
        yield return new WaitForSeconds(8f);
        _isShieldActive = false;
        _playerShieldVisualizer.SetActive(_isShieldActive);
    }

    public void AddScore(int points)
    {
        _score = _score + points;
        UIManager.UpdateScore(_score, IsPlayerOne, IsPlayerTwo);
    }

    public void CheckForBestScore()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("HighScore", _bestScore);
        }

        this.UIManager.BestScoreText.text = "Best: " + _bestScore.ToString();
    }

    IEnumerator RightEngineFailureRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        _rightEngine.SetActive(isHitFromRight);
    }

    IEnumerator LeftEngineFailureRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        _leftEngine.SetActive(isHitFromLeft);
    }

    IEnumerator BothEnginesFailureByEnemyLaser(bool IsHitByEnemyLaser)
    {
        yield return new WaitForSeconds(0.25f);
        _rightEngine.SetActive(IsHitByEnemyLaser);
        _leftEngine.SetActive(IsHitByEnemyLaser);
    }
}
