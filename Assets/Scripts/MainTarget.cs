using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTarget : MonoBehaviour, ITarget, IEffectCreator
{
    public void Shot()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Projectile projectile))
        {
            EventBus.PublishBigExplosionAction(this, transform.position);
            if (GameManager.Instance.PlayingDefaultLevel)
            {
                EventBus.PublishDefaultLevelEnd(this, false);
            }
            else
            {
                EventBus.PublishConstructedLevelEnd(this, false);
            }

            Destroy(gameObject);
        }
    }
}
