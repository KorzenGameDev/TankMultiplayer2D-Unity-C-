using UnityEngine;
using UnityEngine.UI;

public class WeaponsChange : MonoBehaviour
{
    GameMenager gameMenager;
    Shop shop;
    int index = 12;

    public int[] stack1 = new int[12];
    public int[] stack2 = new int[12];
    public bool wasShootInThisTurn = false;

    public bool CanShot()
    {
        if (!wasShootInThisTurn)
        {
            if (gameMenager.ActivePlayer1())
            {
                if (stack1[index] > 0)
                {
                    stack1[index]--;
                    return true;
                }
                else return false;
            }

            if (!gameMenager.ActivePlayer1())
            {
                if (stack2[index] > 0)
                {
                    stack2[index]--;
                    return true;
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }
    //ButtonFunction
    public void NextWeapon()
    {
        index++;
        if (index > 11) index = 0;
        DisplayWeapon();
    }
    public void PrevWeapon()
    {
        index--;
        if (index < 0) index = 11;
        DisplayWeapon();
    }

    void StartSetup()
    {
        index = 0;
        for (int i = 0; i < 12; i++)
        {
            if(i==0)
            {
                stack1[i] = 100;
                stack2[i] = 100;
            }
            else
            {
                stack1[i] = 0;
                stack2[i] = 0;
            }
        }

        DisplayWeapon();
    }
    void Start() { StartSetup(); }
    void DisplayWeapon()
    {
        gameMenager = FindObjectOfType<GameMenager>();
        Text text = gameMenager.GetWeaponText();
        if(shop==null)
            shop = FindObjectOfType<Shop>();
        var weapon = shop.GetComponentWeapons(index);
        if(text!=null && weapon!=null)
        {
            text.text = weapon.wName;
        }
    }
    
    
}
