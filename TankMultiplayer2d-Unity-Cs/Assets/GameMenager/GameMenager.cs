using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class GameMenager : MonoBehaviour
{
    //player
    Player player1 = null;
    Player player2 = null;
    GameObject player1GO = null;
    GameObject player2GO = null;
    Shop shop = null;

    [Header("ControlCanvasUp")]
    [SerializeField] Canvas control = null;
    [SerializeField] GameObject hpBar1 = null;
    [SerializeField] Text namePlayer1 = null;
    [SerializeField] Text hpText1 = null;
    [SerializeField] Text coin1Text = null;
    public int coin1 = 0;
    [SerializeField] GameObject hpBar2 = null;
    [SerializeField] Text namePlayer2 = null;
    [SerializeField] Text hpText2 = null;
    [SerializeField] Text coin2Text = null;
    public int coin2 = 0;
    [SerializeField] Text turnText = null;
    int maxTurn = 0;
    int cTurn = 0;
    [SerializeField] Text scoreText = null;
    int[] score = new int[2];


    [Header("ControlCanvasDown")]
    [SerializeField] GameObject fuelBar = null;
    [SerializeField] Text fuelText = null;
    [SerializeField] GameObject forceBar = null;
    [SerializeField] Text forceText = null;
    [SerializeField] Text angleText = null;
    [SerializeField] Text weaponText = null;

    [SerializeField] Canvas turnCanvas = null;
    CameraFollow myCamera = null;
    Canvas canvasShop = null;

    const string player1s = "Player1";
    const string player2s = "Player2";
    const string coinString = "COINS: ";
    //Public

    public void StartFirstTurn() { StartCoroutine(FirstTurn()); }
    public void StartNewTurn() { StartCoroutine(NextTurn()); }
    public void SetCoin(int add)
    {
        coin1 = coin2 = add;
        coin1Text.text = coinString + coin1.ToString();
        coin2Text.text = coinString + coin2.ToString();
    }
    public void SetCoin(string tag, int add)
    {
        if (tag == player2s)
        {
            coin1 += add;
            coin1Text.text = coinString + coin1.ToString();
        }
        if (tag == player1s)
        {
            coin2 += add;
            coin2Text.text = coinString + coin2.ToString();
        }

    }
    public void SetMaxTurn(int max) { maxTurn = max; SetTurnText(); }
    public void ResetScore() { score[0] = 0; score[1] = 0; }
    public void PlaySounds(AudioClip audio)
    {
        AudioSource source = GetComponent<AudioSource>();
        source.PlayOneShot(audio);
    }
    public void Kill(string tag)
    {
        if (player1GO.CompareTag(tag))
            score[1]++;
        else
            score[0]++;

        Destroy(player1GO);
        Destroy(player2GO);

        cTurn++;
        if(cTurn>maxTurn)
        {
            //TODO display score for winner
            control.enabled = false;
            return;
        }
        control.enabled = false;
        canvasShop.enabled = true;
        shop.Setup();
    }
    public void SetPlayer(GameObject _player1, GameObject _player2)
    {   //create player
        player1GO = Instantiate(_player1, StartRandomPosition(), Quaternion.identity);
        player2GO = Instantiate(_player2, StartRandomPosition(), Quaternion.identity);
        //getscripts
        player1 = player1GO.GetComponent<Player>();
        player2 = player2GO.GetComponent<Player>();
        //add tag
        player1GO.tag = player1s;
        player2GO.tag = player2s;

        player1.StartSetup();
        player2.StartSetup();
    }
    public void SetPlayerName(string _player1, string _player2)
    {
        if (_player1 == "")
            _player1 = player1s;
        namePlayer1.text = _player1;

        if (_player2 == "")
            _player2 = player2s;
        namePlayer2.text = _player2;
    }
    public bool ActivePlayer1() { return player1.enabled; }
    public Player GetActivePlayer()
    {
        if (player1.enabled) return player1;
        else if (player2.enabled) return player2;
        else { Debug.Log("I cant find some player!"); return null; }
    }
    public GameObject GetHPBar(string tag)
    {
        if (tag == player1s) return hpBar1;
        if (tag == player2s) return hpBar2;
        else return null;
    }
    public GameObject GetFuelBar() { return fuelBar; }
    public GameObject GetForceBar() { return forceBar; }
    public Text GetFuelText() { return fuelText; }
    public Text GetForceText() { return forceText; }
    public Text GetAngleText() { return angleText; }
    public Text GetWeaponText() { return weaponText; }
    public Text GetHPText(string tag)
    {
        if (tag == player1s) return hpText1;
        if (tag == player2s) return hpText2;
        else return null;
    }
    //Private
    void Start()
    {
        shop = FindObjectOfType<Shop>();
        canvasShop = shop.GetComponent<Canvas>();
        canvasShop.enabled = false;
        myCamera = GetComponent<CameraFollow>();
        control.enabled = false;
        turnCanvas.enabled = false;
    }
    void SetupPlayer1()
    {
        player1.SetForceBar();
        player1.SetFuelBar();
        player1.SetAngleText();
    }
    void SetupPlayer2()
    {
        player2.SetForceBar();
        player2.SetFuelBar();
        player2.SetAngleText();
    }
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameMenager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    void SetTurnText() { turnText.text = cTurn.ToString() + "/" + maxTurn.ToString(); }
    void SetScoreText() { scoreText.text = score[0].ToString() + ":" + score[1].ToString(); }
    IEnumerator NextTurn()
    {
        Debug.Log("Start New turn");
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Now Start");
        if (player1 != null & player2 != null)
        {
            if (player1.enabled)
            {
                if (myCamera == null)
                    myCamera = GetComponent<CameraFollow>();
                player1.enabled = false;
                player2.enabled = true;
                myCamera.SetTargetToCamera(player2GO.transform);
                SetupPlayer2();
            }
            else if (player2.enabled)
            {
                if (myCamera == null)
                    myCamera = GetComponent<CameraFollow>();
                player1.enabled = true;
                player2.enabled = false;
                myCamera.SetTargetToCamera(player1GO.transform);
                SetupPlayer1();
            }
        }

        WeaponsChange weaponsChange = FindObjectOfType<WeaponsChange>();
        weaponsChange.wasShootInThisTurn = false;
    }
    IEnumerator FirstTurn()
    {
        turnCanvas.enabled = true;
        yield return new WaitForSecondsRealtime(5f);
        turnCanvas.enabled = false;
        control.enabled = true;
        myCamera = GetComponent<CameraFollow>();
        if (player1GO == null || player2GO == null)
        {
            player1GO = GameObject.FindGameObjectWithTag(player1s);
            player2GO = GameObject.FindGameObjectWithTag(player2s);
            player1 = player1GO.GetComponent<Player>();
            player2 = player2GO.GetComponent<Player>();
        }

        player1.enabled = true;
        player2.enabled = false;
        SetTurnText();
        SetScoreText();
        myCamera.SetTargetToCamera(player1GO.transform);
    }
    Vector3 StartRandomPosition()
    {
        Vector3 startPos=new Vector3(Random.Range(-70f,70f), 20f, 0f);
        if(player1GO!=null && Vector3.Distance(player1GO.transform.position, startPos)<=10f)
        {
            StartRandomPosition();
        }
        return startPos;
    }
}
