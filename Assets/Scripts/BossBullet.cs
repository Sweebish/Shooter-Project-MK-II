using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Vector3 Target;
    private float _lifeSpan = 5;
    private float _birth;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Target = player.transform.position;
        _birth = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.position = Vector3.MoveTowards(transform.position, Target, 10 * Time.deltaTime);
        if(Time.time - _birth > _lifeSpan)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GetComponent<CircleCollider2D>().enabled = false;
            other.GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }
    }
}
