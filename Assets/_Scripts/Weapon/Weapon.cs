using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;

public class Weapon : MonoBehaviour
{   
    [SerializeField] private int _damage;
    [SerializeField] private int _speedAttack;
    [SerializeField] private int _currentAmountAmmo;
    [SerializeField] private int _maxAmountAmmo;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private GameObject _shotEffect;
    [SerializeField] private float _shotEffectDelay;

    private CancellationTokenSource _token;
    private AmmoCounter _ammoCounter;
    public bool canDoAction = true;

    public int CurrentAmountAmmo { get => _currentAmountAmmo; set => _currentAmountAmmo = value; }
    public int MaxAmountAmmo
    {
        get { return _maxAmountAmmo; }
        set
        {
            _maxAmountAmmo = Mathf.Max(value, _maxAmountAmmo);
        }
    }
    [Inject]
    private void Construct(AmmoCounter ammoCounter)
    {
        _ammoCounter = ammoCounter;
    }

    private IEnumerator ICooldown()                          // этой корутиной ограничиваю количество вызовов атаки в секунду
    {         
        canDoAction = false;
        yield return new WaitForSeconds(_cooldownTime);
        canDoAction = true;
    }

    private IEnumerator ShotEffect()
    {
        _shotEffect.SetActive(true);
        yield return new WaitForSeconds(_shotEffectDelay);
        _shotEffect.SetActive(false);
    }

    public void StartAttack(Entity nearestTarget)
    {       
        if (canDoAction)
        {          
            StartCoroutine(ICooldown());
            _ = Shot(nearestTarget);
        }           
    }
    private async UniTaskVoid Shot(Entity nearestTarget)
    {
        _token = new CancellationTokenSource();

        while (!_token.IsCancellationRequested)
        {
            if (nearestTarget == null || _currentAmountAmmo == 0)
            {
                PauseAttack();
                return;
            }             
            _currentAmountAmmo--;
            _ammoCounter.UpdateCounter(_currentAmountAmmo);

            StartCoroutine(ShotEffect());

            nearestTarget.TakeDamage(_damage);

            await UniTask.Delay(TimeSpan.FromMinutes(ShotDelay(_speedAttack)), cancellationToken: _token.Token)
                                        .SuppressCancellationThrow();                                                        
        }
    }

    
    public void StopAttack()
    {
        _token?.Cancel();
        canDoAction = false;
    }

    public void PauseAttack()
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
