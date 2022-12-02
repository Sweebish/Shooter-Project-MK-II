using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 4f;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _hPosition;
    private AudioSource _enemyExplosion;
    private Collider2D _collider;
    private UIManager _uimanager;
    private Animator _animator;
    private int _movementType;
    private float _bounder;
    private int _bounderResult = 0;
    private IEnumerator _enemyShooter;

    private void Start()
    {
        AssignComponents();
        StartCoroutine(_enemyShooter);
    }

    private void AssignComponents()
    {
        _hPosition = transform.position.x;
        _movementType = Random.Range(0, 2);
        _enemyExplosion = GameObject.Find("Explosion Sound").GetComponent<AudioSource>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _enemyShooter = EnemyShooting();
    }

    void Update()
    {      
        EnemyMovement();
    }

    private void EnemyMovement()
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
            //_movementType = Random.Range(0, 2);
            transform.position = new Vector3(_hPosition, 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _uimanager.UpdateScore(10);
            Destroy(other.gameObject);
            
            DeathSequence();
        }
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Damage();
           
            DeathSequence();
        }
        if(other.tag == "BeamLaser")
        {
            
            DeathSequence();
        }
        
    }

    IEnumerator EnemyShooting()
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
            _speed = 1f;
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
}
