using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image imgItem = null;
    [SerializeField] Text nameItem = null;
    [SerializeField] Text priceItem = null;
    [SerializeField] Text stockItem = null;
    [SerializeField] int indexSlot;
    Weapons weapons = null;
    Shop shop = null;

    float price;
    string itemName;

    void SlotSetup()
    {
        shop = GetComponentInParent<Shop>();
        weapons = shop.GetComponentWeapons(indexSlot);

        price = weapons.price;
        itemName = weapons.wName;

        imgItem.sprite = weapons.img;
        nameItem.text = itemName;
        priceItem.text = price.ToString();
    }
    void Start() { SlotSetup(); }
}
