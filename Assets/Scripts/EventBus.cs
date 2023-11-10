using System;
using UnityEngine;
public class EventBus
{
    public static event Action<PlayerController> OnPlayerDeathAction;
    public static event Action<EnemyController> OnEnemyDeathAction;
    public static event EventHandler<OnLevelSelectedEventArgs> OnLevelSelectedAction;
    public static event EventHandler<OnLevelEndEventArgs> OnLevelEndAction;
    public static event EventHandler<OnTransitionEventArgs> OnTransitionFinishAction;
    public static event EventHandler<OnScoreUpdateEventArgs> OnScoreUpdateAction;
    public static event EventHandler<OnShotEventArgs> OnShotAction;
    public static event EventHandler<OnExplosionEventArgs> OnBigExplosionAction;
    public static event EventHandler<OnExplosionEventArgs> OnTinyExplosionAction;
    public static event EventHandler<OnExplosionEventArgs> OnBrickExplosionAction;
    public static event Action<int> OnSelectConstructMapAction;

    public class OnLevelSelectedEventArgs : EventArgs { public int selectedLevel; }
    public class OnLevelEndEventArgs : EventArgs { public bool isSuccess; }
    public class OnTransitionEventArgs : EventArgs { public string panel; }
    public class OnScoreUpdateEventArgs : EventArgs { public int addScore; }
    public class OnShotEventArgs : EventArgs { public Vector3 shooterPosition; }
    public class OnExplosionEventArgs : EventArgs { public Vector3 explosionPosition; }

    public static void PublishPlayerDeath(PlayerController player) { OnPlayerDeathAction?.Invoke(player); }
    public static void PublishEnemyDeath(EnemyController enemy) { OnEnemyDeathAction?.Invoke(enemy); }
    public static void PublishLevelEnd(object obj, bool isSuccess) 
    {
        OnLevelEndAction?.Invoke(obj, new OnLevelEndEventArgs
        {
            isSuccess = isSuccess
        });
    }

    public static void PublishSelectedLevel(LevelUIElement levelUIElement, int selectedLevel) {

        OnLevelSelectedAction?.Invoke(levelUIElement, new OnLevelSelectedEventArgs
        {
            selectedLevel = selectedLevel
        });
    }
    public static void PublishUpdateScore(IDamagable iDamagable , int addScore)
    {
        Debug.Log("PublishUpdateScore ve addscore is => " + addScore);
        OnScoreUpdateAction?.Invoke(iDamagable, new OnScoreUpdateEventArgs
        {
            addScore = addScore
        });
    }

    public static void PublishTransitionFinish(TransitionUI transitionUI, string panel)
    {
        OnTransitionFinishAction?.Invoke(transitionUI, new OnTransitionEventArgs 
        {
            panel = panel
        });
    }

    public static void PublishShotAction(IShooting iShooting, Vector3 shooterPosition)
    {
        OnShotAction?.Invoke(iShooting, new OnShotEventArgs
        {
            shooterPosition = shooterPosition
        });
    }
    public static void PublishBigExplosionAction(IEffectCreator iEffectCreator, Vector3 explosionPosition)
    {
        OnBigExplosionAction?.Invoke(iEffectCreator, new OnExplosionEventArgs
        {
            explosionPosition = explosionPosition
        });
    }

    public static void PublishTinyExplosionAction(IEffectCreator iEffectCreator, Vector3 explosionPosition)
    {
        OnTinyExplosionAction?.Invoke(iEffectCreator, new OnExplosionEventArgs
        {
            explosionPosition = explosionPosition
        });
    }
    public static void PublishBrickExplosionAction(IEffectCreator iEffectCreator, Vector3 explosionPosition)
    {
        OnBrickExplosionAction?.Invoke(iEffectCreator, new OnExplosionEventArgs
        {
            explosionPosition = explosionPosition
        });
    }

    public static void PublishSelectConstructMap(int mapID)
    {
        OnSelectConstructMapAction?.Invoke(mapID);
    }
}
