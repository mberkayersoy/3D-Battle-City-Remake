using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    public static event Action<PlayerController> PlayerDeath;
    public static event Action<EnemyController> EnemyDeath;

    public static void PublishPlayerDeath(PlayerController player) { PlayerDeath?.Invoke(player); }
    public static void PublishEnemyDeath(EnemyController enemy) { EnemyDeath?.Invoke(enemy); }
}
