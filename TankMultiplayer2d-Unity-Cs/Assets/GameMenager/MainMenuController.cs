using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Canvas mainMenuCanvas=null;
    [SerializeField] Canvas playerMenuCanvas = null;
    [SerializeField] Canvas creditsCanvas = null;

    [SerializeField] Sprite[] playerSprite = null;
    [SerializeField] Image[] placeToPlayerSprite = null;
    [SerializeField] GameObject[] playerPref = null;
    [SerializeField] InputField[] playerName = new InputField[2];
    [SerializeField] Text moneyText = null;
    [SerializeField] Text turnText = null;
    int[] index = new int[2];
    int startMoney = 3000;
    int turn = 3;

    private void Start()
    {
        index[0] = 0; index[1] = 0; //Todo out this
    }

    public void NewGameButton()
    {   //Todo Move to game menager
        mainMenuCanvas.enabled = false;
        playerMenuCanvas.enabled = true;

        SetTurnText();
        SetMoneyText();
    }
    public void Quit() { Application.Quit(); }
    public void Credits ()
    {
        creditsCanvas.enabled = true;
        mainMenuCanvas.enabled = false;
    }
    public void StartNewGame()
    {
        GameMenager gameMenager = GetComponent<GameMenager>();
        gameMenager.SetPlayer(playerPref[index[0]], playerPref[index[1]]);
        gameMenager.SetPlayerName(playerName[0].text, playerName[1].text);
        gameMenager.SetCoin(startMoney);
        gameMenager.SetMaxTurn(turn);
        gameMenager.ResetScore();

        playerMenuCanvas.enabled = false;
    }

    public void NextTank1Sprite()
    {
        int iP = 0;
        index[iP]++;
        if (index[iP] > playerSprite.Length-1) index[iP] = 0;
        placeToPlayerSprite[iP].overrideSprite = playerSprite[index[iP]];
    }
    public void PrevTank1Sprite()
    {
        int iP = 0;
        index[iP]--;
        if (index[iP] < 0) index[iP] = playerSprite.Length-1;
        placeToPlayerSprite[iP].overrideSprite = playerSprite[index[iP]];
    }
    public void NextTank2Sprite()
    {
        int iP = 1;
        index[iP]++;
        if (index[iP] > playerSprite.Length-1) index[iP] = 0;
        placeToPlayerSprite[iP].overrideSprite = playerSprite[index[iP]];
    }
    public void PrevTank2Sprite()
    {
        int iP = 1;
        index[iP]--;
        if (index[iP] < 0) index[iP] = playerSprite.Length - 1;
        placeToPlayerSprite[iP].overrideSprite = playerSprite[index[iP]];
    }
    public void AddStartMoney()
    {
        startMoney += 500;
        if (startMoney > 5000) startMoney = 2000;
        SetMoneyText();
    }
    public void AddTurn()
    {
        turn++;
        if (turn > 9) turn = 3;
        SetTurnText();
    }
    void SetTurnText()
    {
        turnText.text = "ROUNDS TO WIN: "+ turn.ToString();
    }
    void SetMoneyText()
    {
        moneyText.text = "MONEY: " + startMoney.ToString();
    }

}
