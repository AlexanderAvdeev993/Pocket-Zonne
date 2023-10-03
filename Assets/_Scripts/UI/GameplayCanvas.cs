
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour
{
    [SerializeField] private Image _slider;
    [SerializeField] private Image _detectionZona;

    private Entity _entity;
    private float _currentFillAmount;

    private void Awake()
    {
        _entity = GetComponentInParent<Entity>();
      
        _slider.fillAmount = (float)_entity.CurrentHealth / _entity.MaxHealth;
    }
    public void ActiveDetectionZona(bool active)
    {
        _detectionZona.gameObject.SetActive(active);
    }

    public void InstallationDetectionZona(float value)
    {
        Vector2 newScale = _detectionZona.transform.localScale;
        newScale.x = value*2;
        newScale.y = value*2;
        _detectionZona.transform.localScale = newScale;
    }   
    void OnEnable()
    {
        _entity.OnTakeDamage += ChangeHealth;
    }
    void OnDisable()
    {
        _entity.OnTakeDamage -= ChangeHealth;
    }

    public void ChangeHealth(int currentHealth)
    {        
        _slider.fillAmount = (float)currentHealth / _entity.MaxHealth;
    }
}
