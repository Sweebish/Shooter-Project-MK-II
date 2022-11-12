using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 4f;
    float _hPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }
    }
}
