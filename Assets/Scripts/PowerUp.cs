using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float _speed = 3f;
    [SerializeField]
    private int _powerUpID;
    //PowerUpIDs: 0=TripleShot 1=SpeedUp 2=Shield
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
                /*if (_powerUpID == 0)
                {
                    player.ActivateTripleShot();
                }
                if (_powerUpID == 1)
                {
                    player.ActivateSpeedUp();
                }*\
                /*switch(_powerUpID)
                {
                    case 0: //Triple Shot
                        player.ActivatePowerup(0);
                        break;
                    case 1://Speed Up
                        player.ActivatePowerup(1);
                        break;
                    case 2://Shields
                        player.ActivatePowerup(2);
                        break;
                }*/
            }
            Destroy(this.gameObject);
        }
    }
}
