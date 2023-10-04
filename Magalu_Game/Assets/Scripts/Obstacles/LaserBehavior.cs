using System;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
     [SerializeField] private float laserVelocity;
    
       public event Action<bool> OnPlayerDeath;

       private bool _playerIsDeath;

    public delegate float LaserVelocityReference();

    public static LaserVelocityReference LaserVelocity;

    private void Awake()
    {
        _playerIsDeath = false;
    }

    void Update()
    {
        transform.Translate(Vector3.right.normalized * (laserVelocity * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("laser acertou o player");
            _playerIsDeath = true;
            OnPlayerDeath?.Invoke(_playerIsDeath);
        }
        else if (collision.gameObject.CompareTag("Enviroment"))
        {
            laserVelocity *= -1;
            _playerIsDeath = false;
            OnPlayerDeath?.Invoke(_playerIsDeath);
        }
    }
}
