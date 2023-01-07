using System.Collections;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    private bool _shotFired;
    [SerializeField]
    private int _shootSelector;
    private string _hitTag;
    private bool _laserTag;
    private int _eStrength;
    private int _scoreValue;
    private bool _inDanger;
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
    [SerializeField]
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
        _scoreValue = 10; // default score value
        _laserTag = true; //Directes lasers up or down, set by firing conditions.
        _hPosition = transform.position.x;// sets initial spawn coord for WaggleMovement to use. 
        ShieldFlip();//rolls for an active enemy shield
        MovementSelector();//decides which movement style teh enemy will use
        ShootSelector();//decides if and which which augmented firing behavior the enemy will have
        _enemyExplosion = GameObject.Find("Explosion Sound").GetComponent<AudioSource>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _enemyShooter = ShootMethod();

        
    }

    void Update()
    {
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down);//casts a ray down
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up);//casts a ray up
        if(hitDown.collider != null)
        {
            _hitTag = hitDown.collider.tag;
            Debug.Log(hitDown.collider.tag);
        }
        if(hitUp.collider!= null && hitUp.collider.tag == "Player")//Detects if player is behind enemy.
        {
            _laserTag = false;//sets the shoot method to fire behind the enemy.
        }
        
        EnemyMovements();
        PowerUpShot();
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
                case 2://Enemy Charge behavior
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    if (_hitTag == "Player")
                    {
                        _speed = 12;
                    }
                    break;
                case 3:
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    float DodgeDirection;
                    
                    if (transform.position.x > 0)
                    {
                        DodgeDirection = -1.25f;
                    }
                    else
                    {
                        DodgeDirection = 1.25f;
                    }
                    if(_inDanger == true)
                    {
                    transform.position = new Vector3(transform.position.x + DodgeDirection, transform.position.y, 0);
                    _inDanger= false;
                    }
                    break;
            }   
        
        if (transform.position.y <= -5.4)
        {
            _hPosition = Random.Range(-8f, 8f);
            _shotFired = false;
            _speed = 4;
            _laserTag= true;
            transform.position = new Vector3(_hPosition, 8, 0);
        }
    }//Handles enemy movements

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
        
    }//handles various collisions

    IEnumerator ShootMethod()
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
                lasers[i].UpOrDown(_laserTag);//False is down, True is up.
                lasers[i].tag = "EnemyLaser";
            }
        }
    }//handles enemy shooting

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
    }//handles all functions death related activity

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
        
    }//this movement causes the enemy to strafe side to side

    void TryForRevenge()
    {
        int rollForRevenge = Random.Range(0,10);
        if (rollForRevenge == 9)
        {
            Instantiate(_shieldDrain, new Vector3(Random.Range(-8f,8f), 8, 0), Quaternion.identity);
        }
    }//rolls to spawn a damaging powerup to trick player

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
        _movementType = Random.Range(0, 4);//Pick a movement type to use
        if(_movementType > 0)
        {
            _eStrength++;
            EnemyValueHandler();
        }
    }//chooses which type of movement to use

    void ShootSelector()
    {
        _shootSelector = Random.Range(0, 3);
        //0 = normal shots | 1 = shoot backwards at player | 2 = shoot power ups
    }//activates different shot behaviors

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
                    GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                }
                case 3:
                {
                    GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                }
        }
    }//changes enemy color based on how many upgrades it has

    void PowerUpShot()
    {
        if(_shotFired == true)
        {
            return;
        }
        Vector3 offset = new Vector3(0f, -1.5f, 0);
        if (_hitTag == "PowerUp" && _shootSelector == 2)
        {
            _shotFired= true;
            GameObject ELaser = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            Laser[] lasers = ELaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].UpOrDown(_laserTag);//False is down, True is up.
                lasers[i].tag = "EnemyLaser";
            }
        }
    }//uses the raycast to spot and fire at powerups
    void CheckForDanger()
    {
        GameObject[] Lasers;
        Lasers = GameObject.FindGameObjectsWithTag("Laser");
        for(int i = 0; i > Lasers.Length; i++)
        {
            float Distance = Vector3.Distance(Lasers[i].transform.position, transform.position);
            if(Distance < 1f)
            {
                _inDanger= true;
            }
        }
    }

}
