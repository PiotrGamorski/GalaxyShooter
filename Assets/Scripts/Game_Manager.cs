using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pausePanel;

    [SerializeField]
    private GameObject _audioManager;

    private Animator _pauseAnimator = null;

    public bool isCoOpMode = false;
    public GameObject PausePanel { get => _pausePanel; }
    public AudioSource AudioSource
    {
        get => _audioManager.GetComponentInChildren<AudioSource>();
    }

    public Animator PauseAnimator
    {
        get
        {
            if (!_pauseAnimator)
            {
                _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
            }
            return _pauseAnimator;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _pausePanel.SetActive(true);
            PauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            PauseAnimator.SetBool("isPaused", true);
            PauseGame();
        }
    }

    private void PauseGame()
    {
        this.AudioSource.Pause();
        Time.timeScale = 0f;
    }
}
