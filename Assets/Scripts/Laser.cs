using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private bool _isEnemyLaser = false;

    [SerializeField]
    private float _speed = 8f;

    [SerializeField]
    private GameObject _expolsionPrefab;

    public bool IsEnemyLaser
    {
        get => _isEnemyLaser;
        set { _isEnemyLaser = value; }
    }

    public float Speed
    {
        get => _speed;
        set { _speed = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        this.transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (this.transform.position.y >= 8f)
        {
            if (this.transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void MoveDown()
    {
        this.transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (this.transform.position.y < -8f)
        {
            if (this.transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Destroy(this.transform.GetComponent<BoxCollider2D>());
            Destroy(this.transform.GetComponent<SpriteRenderer>(), 0.15f);

            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.IsHitByEnemyLaser = true;
                player.Damage();
            }
            //Instantiate(_expolsionPrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
