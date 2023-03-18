using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    private float _speed = 4;
    private GameObject _target;
    private float _rSpeed = 400;
    private Rigidbody2D _missleRB;
    void Start()
    {
        _missleRB= GetComponent<Rigidbody2D>();
        
        
    }

    void Update()
    {
        if(_target == null)//if the _target is empty, look for a target.
        {
            _target = TargetScanner();
        }

        if(_target !=null)//if _target has a value, start HomingMovement
        {
            HomingMovement();
        }

        else//if there is no target just fly up.
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        if(transform.position.y > 8)
        {
            Destroy(gameObject);
        }

    }
    private void HomingMovement()// How to fly to _target
    {
        Vector2 direction = (Vector2)_target.transform.position - _missleRB.position;
        direction.Normalize();
        float rotator = Vector3.Cross(direction, transform.up).z;
        _missleRB.angularVelocity = -rotator * _rSpeed;
        _missleRB.velocity = transform.up * _speed;
    }

    private GameObject TargetScanner()// scans for nearest enemy and returns closest.
    {
        GameObject[] targets;
        targets = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;
        float range = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach(GameObject target in targets)
        {
            Vector3 difference = target.transform.position - position;
            float currentRange = difference.sqrMagnitude;
            if (currentRange < range)
            {
                closest = target;
                range = currentRange;
            }
        }
        return closest;
    }
    public void OnHit()
    {
        Destroy(gameObject);
    }
}
