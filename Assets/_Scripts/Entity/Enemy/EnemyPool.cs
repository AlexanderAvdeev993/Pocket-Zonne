using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyPool 
{
    public event Action<bool> OnLastEnemyDie;
    private List<Enemy> _enemyList = new List<Enemy>();
    

    public List<Enemy> EnemyList()
    {
        return _enemyList;
    }
    public void AddEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.OnEnemyDie += DeleteEnemy;
            _enemyList.Add(enemy);
        }
    }
    private void DeleteEnemy(Enemy enemy)
    {
        _enemyList.Remove(enemy);
        if (_enemyList.Count == 0) OnLastEnemyDie?.Invoke(true);
    }
    public void ClearEnemyList()
    {
        /*foreach (var enemy in _enemyList)
        {
            enemy.DestroyEnemy();
        }*/
        _enemyList.Clear();
    }

}
