using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shieldSprite;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _ammoRefillPrefab;
    [SerializeField]
    private GameObject[] _damageVisualizer;
    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _playerExplosion;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _beamLaser;
    [SerializeField]
    private GameObject _fuelBar;
    private float _fuelValue = 30;
    private float _iFrames;
    private float _iFramesDuration = 0.5f;
    private AudioSource _powerUpSound;
    private UIManager _uimanager;
    private SpawnManager _spawnManager;
    private float _playerSpeed = 5f;
    private float _fireRate = .15f;
    private float _canFire = -1f;
    private int _lives = 3;
    private bool _isTripleshotActive;
    private bool _isSpeedUpActive;
    private bool _isShieldActive;
    private int _shieldStrength;
    private bool _isThrusterActive;
    private bool _isBeamLaserActive;
    private int _ammoCount = 15;

    void Start()
    {
        _powerUpSound = GameObject.Find("PowerUpSound").GetComponent<AudioSource>();
        _playerExplosion = GameObject.Find("Explosion Sound").GetComponent<AudioSource>();
        _audioSource = GetComponent<AudioSource>();
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        transform.position = new Vector3(0, 0, 0);
        NullChecks();
        _damageVisualizer[0].SetActive(false);
        _damageVisualizer[1].SetActive(false);
        _audioSource.clip = _laserSound;
        _uimanager.UpdateFuel(_fuelValue);
    }

    void Update()
    {
        PlayerMovement();//Player movement controls

        PlayerBounds();//Limits on player movement

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shooting(_isBeamLaserActive);//Code for firing player weapons
            //_audioSource.Play();
        }

        ThrusterControl();

    }

    private void ThrusterControl()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _fuelValue > 0)
        {
            _isThrusterActive = true;
        }
        else
        {
            _isThrusterActive = false;
            _fuelValue = _fuelValue + 1f * Time.deltaTime;
            if (_fuelValue > 30f)
                _fuelValue = 30f;
            _uimanager.UpdateFuel(_fuelValue);
        }
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        _playerSpeed = 5f;
        if (_isSpeedUpActive == true)
        {
            _playerSpeed = _playerSpeed * 1.5f;
        }
        else if (_isThrusterActive == true)
        {
            _playerSpeed = _playerSpeed * 2f;
            _fuelValue = _fuelValue - 10 * Time.deltaTime;
            _uimanager.UpdateFuel(_fuelValue);
        }
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * _playerSpeed);

    }

    void PlayerBounds()
    {
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void Shooting(bool beam)
    {
        if (_isBeamLaserActive == true)
        {
            return;
        }
        Vector3 offset = new Vector3(0, 1f, 0);
        if (_isTripleshotActive == true && _ammoCount > 0)
        {
            _ammoCount--;
            _uimanager.UpdateAmmo(_ammoCount);
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _canFire = Time.time + _fireRate;
            _audioSource.Play();

        }
        else if (_isTripleshotActive == false && _ammoCount > 0)
        {
            _ammoCount--;
            _uimanager.UpdateAmmo(_ammoCount);
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            _canFire = Time.time + _fireRate;
            _audioSource.Play();
        }

    }

    public void Damage()
    {
        if (_isShieldActive == true)//If the shield Powerup is active, prevent damage.
        {
            if (Time.time < _iFrames)// if the playe rtook damage recently, prevent further damage
            {
                return;
            }
            _shieldStrength--;
            _iFrames = Time.time + _iFramesDuration;
            switch (_shieldStrength)
            {
                case 0:
                    _isShieldActive = false;
                    _shieldSprite.SetActive(false);
                    break;
                case 1:
                    _shieldSprite.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 2:
                    _shieldSprite.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
            }
            return;

        }
        if (Time.time < _iFrames)// if the playe rtook damage recently, prevent further damage
        {
            return;
        }
        _lives--;
        _uimanager.UpdateLives(_lives);
        _iFrames = Time.time + _iFramesDuration;
        DamageVisualizerUpdate();
        StartCoroutine(_CameraShake());

        if (_lives < 1)
        {
            _spawnManager.OnplayerDeath();
            _uimanager.GameOver();
            _playerExplosion.Play();
            Destroy(this.gameObject);
        }
    }

    private void DamageVisualizerUpdate()
    {
        switch (_lives)
        {
            case 1:
                _damageVisualizer[0].SetActive(true);
                _damageVisualizer[1].SetActive(true);
                break;
            case 2:
                _damageVisualizer[0].SetActive(true);
                _damageVisualizer[1].SetActive(false);
                break;
            case 3:
                _damageVisualizer[0].SetActive(false);
                _damageVisualizer[1].SetActive(false);
                break;
        }
    }

    /*public void ActivateTripleShot()
    {
        _isTripleshotActive = true;
        StartCoroutine(PowerUpCooldown(1));
    }
    public void ActivateSpeedUp()
    {
        _isSpeedUpActive = true;
        StartCoroutine(PowerUpCooldown(2));
    }*/

    private void NullChecks()
    {
        if (_audioSource == null)
        {
            Debug.LogError("Player AudioSource == NULL");
        }
        if (_uimanager == null)
        {
            Debug.LogError("Player Failed to Call UIManager");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("Player Failed to Call SpawnManager");
        }
        foreach (GameObject PDV in _damageVisualizer)
        {
            if (PDV == null)
            {
                Debug.LogError("A Player DamageVisualizer index is NULL");
            }
        }

    }

    public void ActivatePowerup(int powerUpID)
    {
        _powerUpSound.Play();
        switch (powerUpID)
        {
            case 0:
                _isTripleshotActive = true;
                StartCoroutine(PowerUpCooldown(0));
                break;
            case 1:
                _isSpeedUpActive = true;
                StartCoroutine(PowerUpCooldown(1));
                break;
            case 2:
                _isShieldActive = true;
                _shieldStrength = 3;
                _shieldSprite.GetComponent<SpriteRenderer>().color = Color.white;
                _shieldSprite.SetActive(true);
                break;
            case 3:
                _ammoCount = 15;
                _uimanager.UpdateAmmo(_ammoCount);
                break;
            case 4:
                _lives++;
                if (_lives > 3)
                {
                    _lives = 3;
                }
                _uimanager.UpdateLives(_lives);
                DamageVisualizerUpdate();
                break;
            case 5:
                _beamLaser.SetActive(true);
                _isBeamLaserActive = true;
                StartCoroutine(PowerUpCooldown(5));
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Damage();
        }
    }


    IEnumerator PowerUpCooldown(int powerUpID)
    {
        yield return new WaitForSeconds(5f);
        switch (powerUpID)
        {
            case 0:
                _isTripleshotActive = false;
                break;
            case 1:
                _isSpeedUpActive = false;
                break;
            case 5:
                _beamLaser.SetActive(false);
                _isBeamLaserActive = false;
                break;
        }
    }
    IEnumerator _CameraShake()
    {
        int i = 5;
        Vector3 _originalPos = new Vector3(0, 0, -10);

        while (i >0)
        {
            float x = Random.Range (-0.5f, 0.5f);
            float y = Random.Range (-0.5f, 0.5f);
            float z = _camera.transform.position.z;
            _camera.transform.position = new Vector3(x, y, z);
            i--;
            yield return new WaitForSeconds(0.1f);
        }
        _camera.transform.position = _originalPos;
    }

}