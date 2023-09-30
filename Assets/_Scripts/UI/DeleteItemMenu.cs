using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeleteItemMenu : MonoBehaviour
{
    [SerializeField] private Button _buttonYes;
    [SerializeField] private Button _buttonNo;
    [SerializeField] private TextMeshProUGUI _textMenuDelete;

    public Slot _slot; 

    private void Start()
    {
        _buttonYes.onClick.AddListener(DeleteItem);
        _buttonNo.onClick.AddListener(CloseMenu);
        UpdateText();
    }
    public void UpdateText()
    {
        _textMenuDelete.text = "Delete " + _slot.item.name;
    }

    private void DeleteItem()
    {
        _slot.DeleteItem();
        CloseMenu();
    }

    private void CloseMenu()
    {
        gameObject.SetActive(false);
    }



}
