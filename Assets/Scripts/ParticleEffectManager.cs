using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
    public static ParticleEffectManager Instance { get; private set; }

    [SerializeField] private ParticleEffectsRefSO particleEffectsRefSO;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        EventBus.OnTinyExplosionAction += EventBus_OnTinyExplosionAction;
        EventBus.OnBigExplosionAction += EventBus_OnBigExplosionAction;
        EventBus.OnBrickExplosionAction += EventBus_OnBrickExplosionAction;
    }

    private void EventBus_OnBrickExplosionAction(object sender, EventBus.OnExplosionEventArgs e)
    {
        CreateBrickExplosionEffect(e.explosionPosition);
    }

    private void EventBus_OnBigExplosionAction(object sender, EventBus.OnExplosionEventArgs e)
    {
        CreateBigExplosionEffect(e.explosionPosition);
    }

    private void EventBus_OnTinyExplosionAction(object sender, EventBus.OnExplosionEventArgs e)
    {
        CreateTinyExplosionEffect(e.explosionPosition);
    }

    public void CreateBigExplosionEffect(Vector3 effectPosition)
    {
        Instantiate(particleEffectsRefSO.bigExplosion, effectPosition, Quaternion.identity, null);
    }
    public void CreateTinyExplosionEffect(Vector3 effectPosition)
    {
        Instantiate(particleEffectsRefSO.tinyExplosion, effectPosition, Quaternion.identity, null);
    }
    public void CreateBrickExplosionEffect(Vector3 effectPosition)
    {
        Instantiate(particleEffectsRefSO.brickExplosion, effectPosition, Quaternion.identity, null);
    }

    private void OnDestroy()
    {
        EventBus.OnTinyExplosionAction -= EventBus_OnTinyExplosionAction;
        EventBus.OnBigExplosionAction -= EventBus_OnBigExplosionAction;
        EventBus.OnBrickExplosionAction -= EventBus_OnBrickExplosionAction;
    }

}
