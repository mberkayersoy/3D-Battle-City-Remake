using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IDamagable
{
    private Rigidbody rb;
    private GameInput gameInput;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float shotTimeOut;
    private float remainingShotTime;
    [SerializeField] private int health;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePointTransform;
    [SerializeField] private ParticleSystem muzzleFlashVFX;
    [SerializeField] private ParticleSystem explosionVFX;
    private bool isRotating = false;
    [SerializeField] private GameObject shild;
    [SerializeField] private Transform[] tankWheels;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        gameInput = GameInput.Instance;
        gameInput.OnPlayerShotAction += GameInput_OnPlayerShotAction;
        ActivateShild();
    }

    private void ActivateShild()
    {
        shild.SetActive(true);
        StartCoroutine(DeactivateShild());
    }
    private IEnumerator DeactivateShild()
    {
        yield return new WaitForSeconds(2f);
        shild.SetActive(false);
    }

    private void Update()
    {
        if (remainingShotTime > 0)
        {
            remainingShotTime -= Time.deltaTime; 
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void GameInput_OnPlayerShotAction(object sender, EventArgs e)
    {
        if (isRotating) return;

        if (remainingShotTime <= 0)
        {
            Shot();
            remainingShotTime = shotTimeOut;
        }
    }

    private void Shot()
    {
        muzzleFlashVFX.Play();
        Instantiate(projectile, firePointTransform.position, transform.rotation);
    }

    private void Move()
    {
        Vector2 movementVector = gameInput.GetMovementVectorNormalized();
        Vector3 direction;

        if (movementVector != Vector2.zero)
        {
            if (Mathf.Abs(movementVector.x) > Mathf.Abs(movementVector.y))
            {
                direction = new Vector3(Mathf.Sign(movementVector.x), 0, 0);
            }
            else
            {
                direction = new Vector3(0, 0, Mathf.Sign(movementVector.y));
            }

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            rb.MovePosition(transform.position + direction.normalized * movementSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        if (shild.activeSelf) return;
        explosionVFX.Play();
        explosionVFX.transform.SetParent(null, true);
        EventBus.PublishPlayerDeath(this);
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        gameInput.OnPlayerShotAction -= GameInput_OnPlayerShotAction;
    }
}
