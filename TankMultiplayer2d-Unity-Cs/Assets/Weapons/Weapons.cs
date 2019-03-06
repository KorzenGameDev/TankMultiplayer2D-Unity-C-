using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 1)]
public class Weapons : ScriptableObject
{
    //descriptions
    public string wName;
    public string description;

    //damage
    public float damage;

    //tiles
    public float speed;
    public GameObject tiles;
    public GameObject effect;

    //special
    public enum Type { Single, Big, Tri, Time, Roller, Jumper, Heat, Laser, Satelite, Chain, Shield, RepairKit };
    public Weapons.Type type;

    public float timer;
    public float price;
    public Sprite img;

}
