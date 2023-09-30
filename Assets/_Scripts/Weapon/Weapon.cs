using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;



public class Weapon : MonoBehaviour 
{
    public Action<int> OnWasteAmmunition;
    [SerializeField] private int _damage;
    [SerializeField] private int _speedAttack;
    [SerializeField] private int _amountAmmo;   
    private CancellationTokenSource _token;

    [SerializeField] private float _cooldownTime;
    private bool canDoAction = true; 
    
    private IEnumerator ICooldown()                          // этой корутиной ограничиваю количество вызовов атаки в секунду
    {
        canDoAction = false;
        yield return new WaitForSeconds(_cooldownTime);
        canDoAction = true;
    }


    public void StartAttack(Entity nearestTarget)
    {
        if (canDoAction)
        {          
            StartCoroutine(ICooldown());
            _token = new CancellationTokenSource();
            _ = Shot(nearestTarget);
        }           
    }
    private async UniTaskVoid Shot(Entity nearestTarget)
    {      
        while (!_token.IsCancellationRequested)
        {
            if (nearestTarget == null || _amountAmmo == 0)
            {
                StopAttack();
                return;
            }
           
            _amountAmmo--;
            OnWasteAmmunition?.Invoke(_amountAmmo);
            nearestTarget.TakeDamage(_damage);

            await UniTask.Delay(TimeSpan.FromMinutes(ShotDelay(_speedAttack)), cancellationToken: _token.Token)
                                        .SuppressCancellationThrow();                                                        
        }
    }
    public void StopAttack()
    {        
        _token?.Cancel();                
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
