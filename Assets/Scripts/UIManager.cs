using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField] 
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartMessageText;
    [SerializeField]
    private TMP_Text _waveText;
    private int _waveCount;
    [SerializeField]
    private Slider _fuelBar;
    private float _fuelValue;
    private int _score;
    [SerializeField]
    private TMP_Text _ammoText;
    private int _ammoCount = 15;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private TMP_Text _missileText;
    private int _missileCount = 0;
    private void Start()
    {
        _waveText.enabled= false;
        _restartMessageText.enabled= false;
        _gameOverText.enabled= false;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        _scoreText.text = "Score: " + _score;
        _ammoText.text = "Ammo: " + _ammoCount + "/15";
        AmmoTextColor();
        _waveText.text = "Wave " + _waveCount + " Start";
        _fuelBar.value = _fuelValue;
        _missileText.text = "Missiles: " + _missileCount;
    }
    public void UpdateScore(int enemyValue)//Get's called by Enemy when it dies to update the player score.
    {
        _score += enemyValue;
    }

    public void UpdateLives(int currentLives)//Is called to update the lives sprite
    {
        _livesImage.sprite = _liveSprites[currentLives];
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoCount = ammo;
    }

    public void WaveUpdate(int Wave)
    {
        _waveCount= Wave;
    }
    
    public void WaveTextActive(bool Bool)
    {
        _waveText.enabled = Bool;
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        _restartMessageText.enabled = true;
        StartCoroutine(BlinkingGameOver());
    }

    public void UpdateFuel(float fuelValue)
    {
        _fuelValue = fuelValue;
    }

    private void AmmoTextColor()
    {
        switch(_ammoCount)
        {
            case 0: 
                _ammoText.GetComponent<TMP_Text>().color= Color.red;
                break;
            case 5:
                _ammoText.GetComponent<TMP_Text>().color= Color.yellow;
                break;
            case 15:
                _ammoText.GetComponent<TMP_Text>().color= Color.white;
                break;
        }
    }

    public void UpdateMissiles (int missileCount)
    {
        _missileCount = missileCount;
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
