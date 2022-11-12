using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DefaultMovement();
        RemoveSelf();
        
    }
    void DefaultMovement()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);
    }
    void RemoveSelf()
    {
        if(transform.position.y >= 8)
        {
            Destroy(this.gameObject);
        }
    }
}
