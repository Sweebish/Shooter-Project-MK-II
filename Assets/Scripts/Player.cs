//using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{   
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shieldSprite;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject[] _damageVisualizer;
    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _playerExplosion;
    [SerializeField]
    private AudioSource _audioSource;
    private float _iFrames;
    private float _iFramesDuration = 0.2f;
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
    }
    void Update()
    {
        PlayerMovement();//Player movement controls
        PlayerBounds();//Limits on player movement
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shooting();//Code for firing player weapons
            _audioSource.Play();
        }
        
    }
    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (_isSpeedUpActive == true)
        {
            _playerSpeed = 8.5f;
        }
        else if (_isSpeedUpActive == false)
        {
            _playerSpeed = 5f;
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
    void Shooting()
    {
        Vector3 offset = new Vector3(0, 1f, 0);
        if (_isTripleshotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _canFire = Time.time + _fireRate;
            
        }
        else if (_isTripleshotActive == false)
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }
    public void Damage()
    {
        if(_isShieldActive == true)//If the shield Powerup is active, prevent damage.
        {
            _isShieldActive = false;
            _shieldSprite.SetActive(false);
            return;
        }
        if(Time.time < _iFrames)// if the playe rtook damage recently, prevent further damage
        {
            return;
        }
        _lives--;
        _uimanager.UpdateLives(_lives);
        _iFrames = Time.time + _iFramesDuration;

        switch(_lives)
        {
            case 1:
                _damageVisualizer[0].SetActive(true);
                _damageVisualizer[1].SetActive(true);
                break;
            case 2:
                _damageVisualizer[0].SetActive(true);
                break;
        }

        if (_lives < 1)
        {
            _spawnManager.OnplayerDeath();
            _uimanager.GameOver();
            _playerExplosion.Play();
            Destroy(this.gameObject);
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
        if(_audioSource == null)
        {
            Debug.LogError("Player AudioSource == NULL");
        }
        if(_uimanager == null)
        {
            Debug.LogError("Player Failed to Call UIManager");
        }
        if(_spawnManager== null)
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
        switch(powerUpID)
        {
            case 0:
                _isTripleshotActive = true;
                StartCoroutine(PowerUpCooldown(0));
                break;
            case 1:
                _isSpeedUpActive=true;
                StartCoroutine(PowerUpCooldown(1));
                break;
            case 2: 
                _isShieldActive=true;
                _shieldSprite.SetActive(true);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyLaser")
        {
            Damage();
        }
    }

    IEnumerator PowerUpCooldown(int powerUpID)
    {
        yield return new WaitForSeconds(5f);
        switch(powerUpID)
        {
            case 0: _isTripleshotActive = false;
                break;
            case 1: _isSpeedUpActive = false;
                break;
        }
    }
}

