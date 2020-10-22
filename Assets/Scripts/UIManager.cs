using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _scoreTextForPlayer2;

    [SerializeField]
    private Text _bestScoreText;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartLevelText;

    [SerializeField]
    private Image _livesImgage;

    [SerializeField]
    private Image _livesImgageForPlayer2;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Sprite[] _liveSpritesForPlayer2;

    [SerializeField]
    private Game_Manager _gameManager;

    private bool _isRestartLevelActivated = false;
    
    public int BestScore { get; set; }

    public Game_Manager GameManager
    {
        get => _gameManager;
    }
    
    public Text BestScoreText
    {
        get => this._bestScoreText;
        set { _bestScoreText = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_gameManager.isCoOpMode == false)
        {
            _scoreText.text = "Score: " + 0;
            _bestScoreText.text = "Best: " + this.BestScore; 
        }

        if (_gameManager.isCoOpMode == true)
        {
            _scoreText.text = "Score: " + 0;
            _scoreTextForPlayer2.text = "Score: " + 0;
        }

        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);
    }

    void Update()
    {
        RestartGame();
    }

    private void RestartGame()
    {
        if (_gameManager.isCoOpMode == false)
        {
            if (_isRestartLevelActivated == true && Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            if (_isRestartLevelActivated == true && Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    public void UpdateScore(int playerScore, bool isPlayerOne, bool isPlayerTwo)
    {
        if (isPlayerOne == true)
        {
            _scoreText.text = "Score: " + playerScore.ToString();
        }

        if (isPlayerTwo == true)
            _scoreTextForPlayer2.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentPlayerLives, bool isPlayerOne, bool isPlayerTwo)
    {
        if (isPlayerOne)
        {
            if (currentPlayerLives >= 0)
                _livesImgage.sprite = _liveSprites[currentPlayerLives];
        }

        if (isPlayerTwo)
        {
            if (currentPlayerLives >= 0)
                _livesImgageForPlayer2.sprite = _liveSpritesForPlayer2[currentPlayerLives];
        }
    }

    public void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        RestartLevelSequence();
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = null;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void RestartLevelSequence()
    {
        _restartLevelText.gameObject.SetActive(true);
        _isRestartLevelActivated = true;
    }

    // Triggers on click - Resume Game button
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _gameManager.PausePanel.SetActive(false);
        _gameManager.AudioSource.Play();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
