using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class AmmoCounter : MonoBehaviour
{
    private TextMeshProUGUI _counterText;
    private Weapon _weapon; 

    [Inject]
    private void Construct(Player player)
    {
        _weapon = player.GetComponentInChildren<Weapon>();
    }

    private void Awake()
    {
        _counterText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        _weapon.OnWasteAmmunition += UpdateCounter;
    }
    private void OnDisable()
    {
        _weapon.OnWasteAmmunition -= UpdateCounter;
    }

    public void UpdateCounter(int value)
    {
        _counterText.text = value.ToString();
    }
}
