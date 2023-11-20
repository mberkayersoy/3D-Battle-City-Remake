using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStaticObstacle : MonoBehaviour, IEffectCreator
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Projectile projectile))
        {
            Vector3 impactPosition = collision.contacts[0].point;
            EventBus.PublishTinyExplosionAction(this, impactPosition);
        }
    }
}
