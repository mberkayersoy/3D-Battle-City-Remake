using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    public int damage;
    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamagable iDamagable))
        {
            iDamagable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
