using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = .15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager SpawnManager;

    // Start is called before the first frame update
    void Start()
    {
        SpawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        transform.position = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();//Player movement controls
        PlayerBounds();//Limits on playe rmovement
        Shooting();//Code for firing player weapons
    }
    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * _playerSpeed);
        
    }
    void PlayerBounds()
    {
        if(transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if(transform.position.y <= -3.8f)
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
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }
    public void Damage()
    {
        _lives--;
        if(_lives < 1)
        {
            SpawnManager.OnplayerDeath();
            Destroy(this.gameObject);
        }
    }
}   

