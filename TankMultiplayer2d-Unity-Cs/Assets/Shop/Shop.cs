using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] Text[] stackText = new Text[12];
    [SerializeField] Weapons[] weapons = new Weapons[12];
    [SerializeField] Text playerText = null;
    [SerializeField] Text coinText = null;
    GameMenager gameMenager = null;
    int playerMoney = 0;
    int indexItem = 12;

    public bool player1Buy = true;
    public void Setup()
    {
        var weapon = FindObjectOfType<WeaponsChange>();
        gameMenager = FindObjectOfType<GameMenager>();
        if (player1Buy && weapon != null)
        {
            for (int i = 0; i < 12; i++)
            {
                stackText[i].text = weapon.stack1[i].ToString();
            }
            playerText.text = "PLayer 1"; //TODO auto
            UpdateCoinText(gameMenager.coin1);
        }
        if (!player1Buy && weapon != null)
        {
            for (int i = 0; i < 12; i++)
            {
                stackText[i].text = weapon.stack2[i].ToString();
            }
            playerText.text = "PLayer 2"; //TODO auto
            UpdateCoinText(gameMenager.coin2);
        }
    }
    public void BuyItem()
    {
        if (indexItem >= 0 && indexItem <= 11)
        {
            if (gameMenager == null)
                gameMenager = FindObjectOfType<GameMenager>();
            if (gameMenager != null)
            {
                if (player1Buy) { playerMoney = gameMenager.coin1; }
                if (!player1Buy) { playerMoney = gameMenager.coin2; }
            }

            if (playerMoney - weapons[indexItem].price >= 0)
            {
                playerMoney -= (int)weapons[indexItem].price;
                if (player1Buy)
                {
                    gameMenager.coin1 = playerMoney;
                    UpdateCoinText(gameMenager.coin1);
                    var weapon = FindObjectOfType<WeaponsChange>();
                    weapon.stack1[indexItem]++;
                    stackText[indexItem].text = weapon.stack1[indexItem].ToString();
                }
                if (!player1Buy)
                {
                    gameMenager.coin2 = playerMoney;
                    UpdateCoinText(gameMenager.coin2);
                    var weapon = FindObjectOfType<WeaponsChange>();
                    weapon.stack2[indexItem]++;
                    stackText[indexItem].text = weapon.stack2[indexItem].ToString();
                }
                Debug.Log("You Buy sth");
            }
            else
            {   //mesage on screen
                Debug.Log("You dont have enought money");
            }
        }
    }
    public void SellItem()
    {
        if (indexItem >= 0 && indexItem <= 11)
        {
            gameMenager = FindObjectOfType<GameMenager>();
            var weapon = FindObjectOfType<WeaponsChange>();
            if (gameMenager != null)
            {
                if (player1Buy) { playerMoney = gameMenager.coin1; }
                if (!player1Buy) { playerMoney = gameMenager.coin2; }
            }
            if (player1Buy && weapon.stack1[indexItem] > 0)
            {
                gameMenager.coin1 = playerMoney + (int)((weapons[indexItem].price) * .6f);
                UpdateCoinText(gameMenager.coin1);
                weapon.stack1[indexItem]--;
                stackText[indexItem].text = weapon.stack1[indexItem].ToString();
            }
            if (!player1Buy && weapon.stack2[indexItem] > 0)
            {
                gameMenager.coin2 = playerMoney + (int)((weapons[indexItem].price) * .6f);
                UpdateCoinText(gameMenager.coin2);
                weapon.stack2[indexItem]--;
                stackText[indexItem].text = weapon.stack2[indexItem].ToString();
            }
        }
    }
    public void Done()
    {
        if (player1Buy)
        {
            player1Buy = !player1Buy;
            Setup();
        }
        else if (!player1Buy)
        {
            player1Buy = !player1Buy;
            Debug.Log("Start new turn"); //TODO start new turn
        }

    }
    public Weapons GetComponentWeapons(int index) { return weapons[index]; }
    //ButtonFunction
    public void Item0() { indexItem = 0; }
    public void Item1() { indexItem = 1; }
    public void Item2() { indexItem = 2; }
    public void Item3() { indexItem = 3; }
    public void Item4() { indexItem = 4; }
    public void Item5() { indexItem = 5; }
    public void Item6() { indexItem = 6; }
    public void Item7() { indexItem = 7; }
    public void Item8() { indexItem = 8; }
    public void Item9() { indexItem = 9; }
    public void Item10() { indexItem = 10; }
    public void Item11() { indexItem = 11; }

    void UpdateCoinText(int coin) { coinText.text = "Coins: " + coin.ToString(); }
}
