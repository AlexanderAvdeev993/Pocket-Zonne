
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private DeleteItemMenu _deleteItemMenu;
    [SerializeField] private Item _item;  
    [SerializeField] private Image _slotImage;
    [SerializeField] private int _amount;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private bool _isEmpty = true;
    private Sprite _spriteDefoult;
    
    public Item Item { get => _item; set => _item = value; }
    public int Amount { get => _amount; set => _amount = value; }
    public bool IsEmpty { get => _isEmpty; set => _isEmpty = value; }
    public TextMeshProUGUI AmountText { get => _amountText; set => _amountText = value; }


    private void Awake()
    {
        _slotImage = GetComponent<Image>();
        _spriteDefoult = _slotImage.sprite;
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OpenDeleteItemMenu);
    }
    private void OpenDeleteItemMenu()
    {
        if (_isEmpty) return;

        _deleteItemMenu.gameObject.SetActive(true);       
        _deleteItemMenu._slot = this;
        _deleteItemMenu.UpdateText();
    }
    public void DeleteItem()
    {
        _item = null;
        _amount = 0;
        _isEmpty = true;
        _slotImage.sprite = _spriteDefoult;
        _amountText.text = "";
    }
    public void ChangeSprite(Sprite sprite)
    {
        _slotImage.sprite = sprite;
    }
}
