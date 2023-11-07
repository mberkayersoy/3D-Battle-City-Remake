using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParticleEffectsRefSO")]
public class ParticleEffectsRefSO : ScriptableObject
{
    [SerializeField] public ParticleSystem tinyExplosion;
    [SerializeField] public ParticleSystem bigExplosion;
    [SerializeField] public ParticleSystem brickExplosion;
}
