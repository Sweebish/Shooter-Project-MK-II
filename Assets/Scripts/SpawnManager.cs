using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject[] _powerUps;//Array for powerups can be filled with inspector
    [SerializeField]
    private GameObject _enemyContainer;//Empty container to avoid clutter in Hierarchy from enemy spawns
    private bool _canSpawn = false;//turns spawning on and off
    private bool _delayActive;
    [SerializeField]
    private int _waveCounter;
    [SerializeField]
    private int _enemyCounter;
    private int _waveLimit;
    private GameObject _newEnemy;
    private IEnumerator _eSpawner;
    private IEnumerator _pSpawner;

    private void Start()
    {
        _eSpawner = EnemySpawner();
        _pSpawner = PowerUpSpawner();
        _delayActive = true;
    }

    private void Update()
    {
        WaveManager();
        if (_enemyCounter == _waveLimit)
        {
            _delayActive = true;
            StopCoroutine(_eSpawner);
            StartDelay();
        }

    }

    private void WaveManager()
    {
        switch (_waveCounter)
        {
            case 0:
                _waveLimit = 5000;
                break;
            case 1:
                _waveLimit = 3;
                break;
            case 2:
                _waveLimit = 5;
                break;
            case 3:
                _waveLimit = 7;
                break;
            case 4:
                _waveLimit = 10;
                break;
            case 5:
                _waveLimit = 15;
                break;
            case 6:
                _waveLimit = 1;
                break;
            default:
                break;
        }
    }

    public void OnplayerDeath()
    {
        _canSpawn = false;
        
    }
    IEnumerator EnemySpawner()
    {
        int waitTime = 5;
        while(_canSpawn == true)
        {
            float hPosToSpawn = Random.Range(-8f, 8f);
            _newEnemy = Instantiate(enemy, new Vector3(hPosToSpawn, 8, 0), Quaternion.identity);
            _enemyCounter++;
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator PowerUpSpawner()
    {
        while (_canSpawn == true)
        {
            float pWaitTime = Random.Range(3f, 7f);//Random wait time between powerup spawns
            yield return new WaitForSeconds(pWaitTime);//how long to wait before spawning a new powerup
            int randomPowerUp = Random.Range(0, 17);//Call random PowerUp
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);//Where to spawn powerups
            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);//spawns a randomly selected powrup from array.
            
        }
    }
    public void StartDelay()
    {
        if(!_newEnemy)
        {
            StopCoroutine(_pSpawner);
            StartCoroutine(SpawnDelay());
        }
    }
    void StartSpawning()
    {
        StartCoroutine(_eSpawner);
        StartCoroutine(_pSpawner);
    }
    IEnumerator SpawnDelay()
    {
        while (_delayActive == true)
        {
            _waveCounter++;
            _uiManager.WaveUpdate(_waveCounter);
            _uiManager.WaveTextActive(true);
            _canSpawn= false;
            _enemyCounter= 0;
            Debug.Log("Spawn Delay Started");
            yield return new WaitForSeconds(5f);
            _uiManager.WaveTextActive(false);
            _delayActive= false;
            _canSpawn= true;
            Debug.Log("Spawn Delay Done");
            StartSpawning();
            yield break;
        }
    }
}
