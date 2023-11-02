using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    public static event Action<PlayerController> OnPlayerDeathAction;
    public static event Action<EnemyController> OnEnemyDeathAction;
    public static event EventHandler<OnLevelSelectedEventArgs> OnLevelSelectedAction;
    public static event EventHandler<OnLevelSuccessfullyEndEventArgs> OnLevelSuccessfullyEndAction;
    public static event EventHandler<OnTransitionEventArgs> OnTransitionFinishAction;

    public class OnLevelSelectedEventArgs : EventArgs { public int selectedLevel; }
    public class OnLevelSuccessfullyEndEventArgs : EventArgs { public bool isSuccess; }
    public class OnTransitionEventArgs : EventArgs { public string panel; }

    public static void PublishPlayerDeath(PlayerController player) { OnPlayerDeathAction?.Invoke(player); }
    public static void PublishEnemyDeath(EnemyController enemy) { OnEnemyDeathAction?.Invoke(enemy); }
    public static void PublishLevelEnd(LevelManager levelManager, bool isSuccess) 
    {
        OnLevelSuccessfullyEndAction?.Invoke(levelManager, new OnLevelSuccessfullyEndEventArgs
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

    public static void PublishTransitionFinish(TransitionUI transitionUI, string panel)
    {
        OnTransitionFinishAction?.Invoke(transitionUI, new OnTransitionEventArgs 
        {
            panel = panel
        });
    }


}
