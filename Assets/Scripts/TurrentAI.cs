using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentAI : MonoBehaviour
{
    private GameObject _player;
    [SerializeField]
    private GameObject _bossBullet;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ShootMethod());
    }

    // Update is called once per frame
    void Update()
    {
        AimAtPlayer();


    }

    private void AimAtPlayer()
    {
        Vector3 offset = _player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, offset);
    }
    IEnumerator ShootMethod()
    {
        float shootWaitTime = Random.Range(2f, 4f);
        while (true)
        {
            yield return new WaitForSeconds(shootWaitTime);
            Instantiate(_bossBullet, transform.position, Quaternion.identity);
           
        }
    }
}
