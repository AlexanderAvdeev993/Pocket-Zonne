
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _slider;
    private Entity _entity;
    private float _currentFillAmount;


    private void Awake()
    {
        _entity = GetComponentInParent<Entity>();
        _currentFillAmount = _entity.Health;

        _slider.fillAmount = 1;
    }
    void OnEnable()
    {
        _entity.OnTakeDamage += ChangeHealth;
    }
    void OnDisable()
    {
        _entity.OnTakeDamage -= ChangeHealth;
    }

    private void ChangeHealth(int currentHealth)
    {
        _slider.fillAmount = currentHealth / _currentFillAmount;
    }


}
