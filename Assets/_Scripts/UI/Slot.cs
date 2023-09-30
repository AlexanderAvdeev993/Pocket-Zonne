
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item { get;  set; }
    public int amount;
    public DeleteItemMenu _deleteItemMenu;

    public bool isEmpty = true;
    private Image slotImage;
    private Button _button;
    private Sprite _spriteDefoult;
    public TextMeshProUGUI _amountText;
   
   

    private void Awake()
    {
        slotImage = GetComponent<Image>();
        _spriteDefoult = slotImage.sprite;
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OpenDeleteItemMenu);
    }
    private void OpenDeleteItemMenu()
    {
        if (isEmpty) return;

        _deleteItemMenu.gameObject.SetActive(true);       
        _deleteItemMenu._slot = this;
        _deleteItemMenu.UpdateText();
    }
    public void DeleteItem()
    {
        item = null;
        amount = 0;
        isEmpty = true;
        slotImage.sprite = _spriteDefoult;
        _amountText.text = "";
    }
    public void ChangeSprite(Sprite sprite)
    {
        slotImage.sprite = sprite;
    }
}
