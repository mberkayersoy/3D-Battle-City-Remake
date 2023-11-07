using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsRefSO audioClipRefsSO;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        //volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    void Start()
    {
        EventBus.OnShotAction += EventBus_OnShotAction;
        EventBus.OnBigExplosionAction += EventBus_OnExplosionAction;
        //EventBus.OnTinyExplosionAction += EventBus_OnTinyExplosionAction;
        // To do: onbrickexplosionaction
    }

    private void EventBus_OnTinyExplosionAction(object sender, EventBus.OnExplosionEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void EventBus_OnExplosionAction(object sender, EventBus.OnExplosionEventArgs e)
    {
        PlaySound(audioClipRefsSO.explosion, e.explosionPosition, volume);
    }

    private void EventBus_OnShotAction(object sender, EventBus.OnShotEventArgs e)
    {
        PlaySound(audioClipRefsSO.shot, e.shooterPosition, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void ChangeVolume()
    {
        volume += .1f;

        if (volume > 1f)
        {
            volume = 0f;
        }

        //PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        //PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
