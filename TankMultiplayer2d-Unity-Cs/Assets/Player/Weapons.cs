using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName ="Weapons",order =1)]
public class Weapons : ScriptableObject
{
    //descriptions
    public string wName;
    public string description;

    //damage
    public float demage;

    //tiles
    public float speed;
    public GameObject tiles;
    // [SerializeField] GameObject effect;

    //special
    public bool triShot;
    public bool singleShoot;
    public bool timerBomb;
    public bool bounceShoot;
    public float timer;

    public float price;
    public Sprite img;

}
