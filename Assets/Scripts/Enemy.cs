using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _eStrength;
    private int _scoreValue;
    [SerializeField]
    float _speed = 4f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _enemyShield;
    private float _hPosition;
    private AudioSource _enemyExplosion;
    private Collider2D _collider;
    private UIManager _uimanager;
    private Animator _animator;
    [SerializeField]
    private GameObject _shieldDrain;
    private int _movementType;
    private float _bounder;
    private int _bounderResult = 0;
    private bool _isShieldActive;
    private IEnumerator _enemyShooter;

    private void Start()
    {
        AssignValues();// Assign values to their handles.
        EnemyStrengthColor();
        StartCoroutine(_enemyShooter);
    }

    private void AssignValues()
    {
        _scoreValue = 10;
        _hPosition = transform.position.x;// sets initial spawn coord for WaggleMovement to use. 
        ShieldFlip();
        MovementSelector();
        _enemyExplosion = GameObject.Find("Explosion Sound").GetComponent<AudioSource>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _enemyShooter = ShootMethods();
        
    }

    void Update()
    {      
        EnemyMovements();
    }

    private void EnemyMovements()
    {
            switch (_movementType)
            {
                case 0: //Default Movement Type
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    break;
                case 1: //Enemy side to side stafe momement
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                _bounder = transform.position.x;
                WaggleMovement();
                break;
            }   
        
        if (transform.position.y <= -5.4)
        {
            _hPosition = Random.Range(-8f, 8f);
            transform.position = new Vector3(_hPosition, 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if(_isShieldActive == true)
            {
                _isShieldActive = false;
                _enemyShield.SetActive(false);
                return;
            }
            _uimanager.UpdateScore(_scoreValue);
            Destroy(other.gameObject);
            
            DeathSequence();
        }
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Damage();
            _enemyShield.SetActive(false);
            DeathSequence();
        }
        if(other.tag == "BeamLaser")
        {
            _enemyShield.SetActive(false);
            DeathSequence();
        }
        
    }

    IEnumerator ShootMethods()
    {
        float shootWaitTime = Random.Range(3f, 7f);
        Vector3 offset = new Vector3(0f, -1.5f, 0);
        while (true)
        {
            yield return new WaitForSeconds(shootWaitTime);
            GameObject ELaser = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            Laser[] lasers = ELaser.GetComponentsInChildren<Laser>();
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignAsEnemy();
                lasers[i].tag = "EnemyLaser";
            }
        }
    }

    private void DeathSequence()
    {
            StopCoroutine(_enemyShooter);
            GetComponent<SpriteRenderer>().color= Color.white;
            _speed = 1f;
            TryForRevenge();
            _collider.enabled = false;
            _enemyExplosion.Play();
            _animator.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2f);
    }

    void WaggleMovement()
    {       
        if(_bounder > _hPosition + 1)
        {
            _bounderResult= 0;
        }
        if(_bounder < _hPosition - 1)
        {
            _bounderResult= 1;
        }
        switch(_bounderResult)
        {
            case 0:
                transform.Translate(Vector3.left *  _speed * Time.deltaTime);
                break;
            case 1:
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
                break;
        }
        
    }

    void TryForRevenge()
    {
        int rollForRevenge = Random.Range(0,10);
        if (rollForRevenge == 9)
        {
            Instantiate(_shieldDrain, new Vector3(Random.Range(-8f,8f), 8, 0), Quaternion.identity);
        }
    }

    void ShieldFlip()
    {
        if(Random.value < .33f)
        {
            _isShieldActive= true;
            _enemyShield.SetActive(true);
            _eStrength++;
            EnemyValueHandler();
        }
        else
        {
            _isShieldActive= false;
        }
    }//Roll to turn shield on or off

    void MovementSelector()
    {
        _movementType = Random.Range(0, 2);//Pick a movement type to use
        if(_movementType > 0)
        {
            _eStrength++;
            EnemyValueHandler();
        }
    }

    void EnemyValueHandler()
    {
        _scoreValue += 10;
    }//increase enemy score value

    void EnemyStrengthColor()
    {
        switch(_eStrength)
        {
            case 0:
                break; 
            case 1:
                {
                    GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                }
                case 2:
                {
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                }
                case 3:
                {
                    GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                }
        }
    }

}
