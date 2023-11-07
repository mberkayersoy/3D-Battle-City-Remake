using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTarget : MonoBehaviour, ITarget, IShooting
{
    [SerializeField] private ParticleSystem explosionVFX;
    public void Shot()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Projectile projectile))
        {
            explosionVFX.Play();
            explosionVFX.transform.SetParent(null, true);
            EventBus.PublishExplosionAction(this, transform.position);
            EventBus.PublishLevelEnd(this, false);
            Destroy(gameObject);
        }
    }
}
