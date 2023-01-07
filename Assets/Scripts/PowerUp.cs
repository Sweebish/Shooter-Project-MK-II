using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int _powerUpID;
    private bool _moveToPlayer;
    private Transform _target;
    private Vector3 _powerUpMovement;
    //PowerUpIDs: 0=TripleShot | 1=SpeedUp | 2=Shield | 3 = Ammo Refill | 4 = Health Refill | 5 = Beam Laser
    private void Start()
    {
        _powerUpMovement= Vector3.down;
    }
    void Update()
    {
        
        if(_moveToPlayer == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }
        if(_moveToPlayer == false)
        {
            transform.Translate(_powerUpMovement * _speed * Time.deltaTime);
        }
        if(transform.position.y <= -5.4)
        {
            Destroy(this.gameObject);  
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.ActivatePowerup(_powerUpID);//calls and passes the powerupID to the named function
             
            }

            Destroy(this.gameObject);
        }
        if (other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
    public void GoToPlayer(Transform target, bool isTrue)//Enables the behavior to make the power up float to the player
    {
        _target= target;
       _moveToPlayer= isTrue;
    }
}
