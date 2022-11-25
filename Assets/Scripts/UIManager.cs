using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField] 
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartMessageText;
    private int _score;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Image _livesImage;
    private void Start()
    {
        _restartMessageText.enabled= false;
        _gameOverText.enabled= false;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        _scoreText.text = "Score: " + _score;
    }
    public void UpdateScore(int enemyValue)//Get's called by Enemy when it dies to update the player score.
    {
        _score += enemyValue;
    }
    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
    }
    public void GameOver()
    {
        _gameManager.GameOver();
        _restartMessageText.enabled = true;
        StartCoroutine(BlinkingGameOver());
    }
    IEnumerator BlinkingGameOver()
    {
        /*while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if(_gameOverText.enabled == false)
            {
                _gameOverText.enabled = true;
            }
            else if (_gameOverText.enabled == true)
            {
                _gameOverText.enabled = false;
            }
        }*/

        /*while(true)
        {
            _gameOverText.enabled = true;
            yield return new WaitForSeconds(.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(.5f);
        }*/

        while(true) 
        {
            _gameOverText.enabled = !_gameOverText.enabled;
            yield return new WaitForSeconds(.5f);
        }

    }
}
