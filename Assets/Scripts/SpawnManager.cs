using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject[] _powerUps;//Array for powerups can be filled with inspector
    [SerializeField]
    private GameObject _enemyContainer;//Empty container to avoid clutter in Hierarchy from enemy spawns
    private bool canSpawn = false;//turns spawning on and off
    //private AudioSource _audioManager;
    private void Start()
    {
        //_audioManager = GameObject.Find("Explosion Sound").GetComponent<AudioSource>();
    }
    public void OnplayerDeath()
    {
        canSpawn = false;
        //_audioManager.Play();
    }
    IEnumerator EnemySpawner()
    {
        float waitTime = 5.0f;
        
        
        while(canSpawn == true)
        {
            yield return new WaitForSeconds(waitTime);
            float hPosToSpawn = Random.Range(-8f, 8f);
            GameObject newEnemy = Instantiate(enemy, new Vector3(hPosToSpawn, 8, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            
        }
    }
    IEnumerator PowerUpSpawner()
    {
        
        while (canSpawn == true)
        {
            int chanceModifier;
            float pWaitTime = Random.Range(3f, 7f);//Random wait time between powerup spawns
            yield return new WaitForSeconds(pWaitTime);//how long to wait before spawning a new powerup
            int randomPowerUp = Random.Range(0, 6);//Call random PowerUp
            if(randomPowerUp == 6)
            {
                chanceModifier = Random.Range(0, 2);
                randomPowerUp -= chanceModifier;
                Debug.Log("ChanceModifier " + chanceModifier + " was Used.");
            }
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);//Where to spawn powerups
            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);//spawns a randomly selected powrup from array.
            
        }
    }
    public void StartSpawning()
    {
        canSpawn = true;
        StartCoroutine(EnemySpawner());
        StartCoroutine(PowerUpSpawner());

    }

}
