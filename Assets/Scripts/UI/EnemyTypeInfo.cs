using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTypeInfo : MonoBehaviour
{
    [SerializeField] private Image enemySprite;
    [SerializeField] private Sprite[] enemySpriteList; 

    public void SetEnemyType(EnemyType enemyType)
    {
        enemySprite.sprite = enemyType switch
        {
            EnemyType.Green => enemySpriteList[1],
            EnemyType.Blue => enemySpriteList[2],
            EnemyType.Red => enemySpriteList[3],
            _ => enemySpriteList[0],
        };
    }
}
