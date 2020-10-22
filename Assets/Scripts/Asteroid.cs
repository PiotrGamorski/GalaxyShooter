using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;

    [SerializeField]
    private float _rotateSpeed = 20.0f;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private AudioClip _asteroidAudioClip;

    private SpawnManager _spawnManager = null;

    public SpawnManager SpawnManager
    {
        get
        {
            if (!_spawnManager)
            {
                _spawnManager = GameObject.FindGameObjectWithTag("Spawn_Manager").GetComponent<SpawnManager>();
            }
            return _spawnManager;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        //this.transform.Translate(Vector3.down * _speed * Time.deltaTime);
        this.transform.RotateAround(this.transform.position, Vector3.forward, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(this.gameObject.GetComponent<CircleCollider2D>());
            Instantiate(this._explosionPrefab, this.transform.position, Quaternion.identity);
            StartCoroutine(AsteroidDestroyRoutine());
            Destroy(this.gameObject, 2.8f);
            Destroy(other.gameObject);
            this.SpawnManager.StartSpawning();
        }
    }

    private IEnumerator AsteroidDestroyRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        this.gameObject.SetActive(false);
    }
}
