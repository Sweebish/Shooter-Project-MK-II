using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private int _initialDrift;
    private int _speed;
    private Vector3 _driftDirection;
    [SerializeField]
    private int _life = 5;
    private Collider2D _collider;
    [SerializeField]
    private GameObject _explosion;
    private Transform _turret;
    private UIManager _uiManager;
    void Start()
    {
        SetDrift();
        _speed = 5;
        _collider = GetComponent<Collider2D>();
        _turret = transform.GetChild(0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

    }

    private void SetDrift()
    {
        if (Random.value < 0.5f)
        {
            _driftDirection = Vector3.left;
        }
        else
        {
            _driftDirection = Vector3.right;
        }
    }
    void Update()
    {

        BossMovement();
        DriftFlipper();

    }
    private void BossMovement()
    {
        
        if (transform.position.y > 4)
        {
            transform.Translate(Vector3.down * 1 * Time.deltaTime);
        }
        if(transform.position.y <= 4)
        {
            transform.Translate(_driftDirection * _speed * Time.deltaTime);
        }
        
    }
    private void DriftFlipper()
    {
        if(transform.position.x > 8f)
        {
            _driftDirection = Vector3.left;
        }
        if(transform.position.x < -8f)
        {
            _driftDirection = Vector3.right;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser" && transform.position.y <= 4)
        {
            _life--;
        }
        if(other.tag == "Missile" && transform.position.y <= 4)
        {
            _life--;
        }
        if(_life < 1)
        {
            DeathSequence();
        }
    }
    private void DeathSequence()
    {
        _collider.enabled = false;
        GameObject Explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
        GetComponent<SpriteRenderer>().enabled = false;
        _speed = 0;
        _uiManager.GameOver();
        Destroy(Explosion, 2.5f);
        Destroy(_turret, 2.5f);
        Destroy(this.gameObject, 2.6f);
    }
}
