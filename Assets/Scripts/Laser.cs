using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 40f;
    private bool _isEnemyLaser;
    void Update()
    {
        if(_isEnemyLaser == false)
        {
            MoveUp();
            RemoveSelf();
        }
        else
        {
            MoveDown();
            RemoveSelf();
        }
        
        
    }
    void MoveUp()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);     
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
    }
    void RemoveSelf()
    {
        if(transform.position.y > 8 || transform.position.y < -8)
        {
            if(transform.parent != null )
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    public void AssignAsEnemy()
    {
        _isEnemyLaser = true;
    }
}
