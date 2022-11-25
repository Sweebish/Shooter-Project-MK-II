using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private AudioSource _asteroidExplosion;
    SpawnManager _spawnmanager;
    [SerializeField]
    GameObject _explosionPrefab;
    void Start()
    {
        _asteroidExplosion = GameObject.Find("Explosion Sound").GetComponent<AudioSource>();
        _spawnmanager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 40 * Time.deltaTime));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        if(other.tag == "Laser")
        {
            _spawnmanager.StartSpawning();
            _asteroidExplosion.Play();
            GetComponent<Collider2D>().enabled = false;
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity) ;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(explosion, 2.5f);
            Destroy(this.gameObject, 2.6f);
        }
    }
}
