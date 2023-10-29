using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform firePointTransform;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rayDistance;
    [SerializeField] private float shotTimeOut;
    private float remainingShotTime;
    NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        CheckFront();

        if (remainingShotTime > 0)
        {
            remainingShotTime -= Time.deltaTime;
        }
    }

    private void Shot()
    {
        if (remainingShotTime <= 0)
        {
            Instantiate(projectile, firePointTransform.position, transform.rotation);
            remainingShotTime = shotTimeOut;
        }
    }

    private void CheckFront()
    {
        if (Physics.Raycast(firePointTransform.position, transform.forward, rayDistance))
        {
            Debug.Log("hit");
            Debug.DrawLine(firePointTransform.position, transform.forward * rayDistance);
            Shot();
        }
        else
        {
            Debug.Log("not hit");
            agent.SetDestination(target.position);
           // agent.Move((target.position - transform.position) * Time.deltaTime * moveSpeed);
            //transform.LookAt(target.position);
        }
    }
}
