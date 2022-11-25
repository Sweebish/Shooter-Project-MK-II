using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 4f;
    [SerializeField]
    private GameObject _laserPrefab;
    float _hPosition;
    private AudioSource _enemyExplosion;
    private Collider2D _collider;
    private UIManager _uimanager;
    private Animator _animator;
    private bool dead = false;
    private void Start()
    {
        _enemyExplosion = GameObject.Find("Explosion Sound").GetComponent<AudioSource>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        StartCoroutine(EnemyShooting());
    }
    void Update()
    {
        EnemyMovement();
    }
    private void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -5.4)
        {
            _hPosition = Random.Range(-8f, 8f);
            transform.position = new Vector3(_hPosition, 8, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit" + other.transform.name);
        if (other.tag == "Laser")
        {
            _uimanager.UpdateScore(10);
            Destroy(other.gameObject);
            StartCoroutine(DeathSequence());
        }
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Damage();

            StartCoroutine(DeathSequence());
        }
        IEnumerator DeathSequence()
        {
            while (dead == false)
            {
                dead = true;
                _speed = 0f;
                _collider.enabled = false;
                _enemyExplosion.Play();
                _animator.SetTrigger("OnEnemyDeath");
                yield return new WaitForSeconds(2f);
                Destroy(this.gameObject);
            }
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
}
