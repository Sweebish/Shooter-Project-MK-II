using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool canSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnplayerDeath()
    {
        canSpawn = false;
    }
    IEnumerator SpawnRoutine()
    {
        float _waitTime = 5.0f;
        while(canSpawn == true)
        {
            GameObject newEnemy = Instantiate(enemy, new Vector3(0, 8, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_waitTime);
        }
    }

}
