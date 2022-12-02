using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float _speed = 3f;
    [SerializeField]
    private int _powerUpID;
    //PowerUpIDs: 0=TripleShot | 1=SpeedUp | 2=Shield | 3 = Ammo Refill | 4 = Health Refill | 5 = Beam Laser
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
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
    }
}
