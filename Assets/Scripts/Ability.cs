using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField] private AbilityType abilityType;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Destroy(gameObject);
        }
    }
}
