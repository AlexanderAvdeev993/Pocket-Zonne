using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;



public class Weapon : MonoBehaviour 
{
    [SerializeField] private int _damage;
    [SerializeField] private int _speedAttack;

    private CancellationTokenSource _token;
   

    public void StartAttack(Entity nearestTarget)
    {   
        if(nearestTarget == null) return;

        _token = new CancellationTokenSource();
        _ = Shot(nearestTarget);
    }
    private async UniTaskVoid Shot(Entity nearestTarget)
    {      
        while (!_token.IsCancellationRequested)
        {
            if (nearestTarget == null)
            {
                StopAttack();
                return;
            }
               
            Debug.Log("Attack");
            nearestTarget.TakeDamage(_damage);

            await UniTask.Delay(TimeSpan.FromMinutes(ShotDelay(_speedAttack)), cancellationToken: _token.Token)
                                        .SuppressCancellationThrow();                
        }
    }
    public void StopAttack()
    {
        _token?.Cancel();
        Debug.Log("Stop Attack");
    }  
    private double ShotDelay(float _speedAttack)
    {
        if (_damage <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(_speedAttack));
        }
        return 1 / _speedAttack;
    }
}
