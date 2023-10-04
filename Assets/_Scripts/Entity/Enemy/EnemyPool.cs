using System;
using System.Collections.Generic;


public class EnemyPool 
{    
    private List<Enemy> _enemyList = new List<Enemy>();
    
    public List<Enemy> EnemyList()
    {
        return _enemyList;
    }
    public void AddEnemy(Enemy enemy)
    {
        if (enemy != null)
        {           
            _enemyList.Add(enemy);
        }
    }
    public void ClearEnemyList()
    {      
        _enemyList.Clear();
    }

}
