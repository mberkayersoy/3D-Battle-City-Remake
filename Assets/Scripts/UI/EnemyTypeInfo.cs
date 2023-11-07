using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTypeInfo : MonoBehaviour
{
    [SerializeField] private Image enemySprite;

    public void SetEnemyType(EnemyType enemyType)
    {
        enemySprite.color = enemyType switch
        {
            EnemyType.Green => Color.green,
            EnemyType.Blue => Color.blue,
            EnemyType.Red => Color.red,
            _ => Color.gray,
        };
    }
}
