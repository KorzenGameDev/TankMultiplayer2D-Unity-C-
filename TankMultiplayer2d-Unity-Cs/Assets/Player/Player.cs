using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour, IDamageable<float>
{
    //Move
    [SerializeField] GameObject body = null;
    //TODO add audio move
    Rigidbody2D rb = null;
    float mSpeed = 4000f;
    float fuel=100f;
    bool facingRight = true;

    //Cannon
    [SerializeField] Transform tCannon = null;
    [SerializeField] Transform tShoot = null;
    [SerializeField] Weapons weapons = null;
    [SerializeField] float force = 1f;
    [SerializeField] AudioClip audioShoot = null;
    float angle = 0f;

    //Health
    [SerializeField]float maxHealth = 100f;
    float cHealth = 100f;

    //GameHandler
    CameraFollow myCamera = null;
    GameMenager gameMenager = null;
    WeaponsChange weaponsChange = null;

    public void CharacterMove(float hMove)
    {
        float multiple = 5f;

        if (hMove == 0)
            hMove = Input.GetAxisRaw("Horizontal");

        if (hMove != 0 && Mathf.Round(fuel - Time.deltaTime * multiple) > 0f)
        {
            rb.velocity = new Vector2(hMove, rb.velocity.y);
            CharacterFlip(hMove);
            fuel -= Time.deltaTime * multiple;
            SetFuelBar();
        }

        if (Mathf.Round(fuel - Time.deltaTime) <= 0)
        {
            fuel = 0;
            Debug.Log("Empty"); //TODO text on screen
        }

        if (gameObject.transform.rotation.z > 30f) gameObject.transform.rotation =new  Quaternion(0f, gameObject.transform.rotation.y, 30f, 1f);
        if (gameObject.transform.rotation.z < -30f) gameObject.transform.rotation =new  Quaternion(0f, gameObject.transform.rotation.y, -30f, 1f);
    }
    public void MathCannonAngle(float dMoveCannon)
    {
        if (dMoveCannon == 0)
            dMoveCannon = Input.GetAxisRaw("Vertical");

        if (dMoveCannon != 0)
        {
            if (angle + dMoveCannon <= 0) { angle = 0; return; }
            if (angle + dMoveCannon >= 90) { angle = 90f; return; }
            angle += dMoveCannon;
        }

        SetAngleText();
    }
    public void SetForce(float add)
    {
        force += add * 5f;
        if (force <= 0) force = 0;
        if (force >= 2f) force = 2f;
        SetForceBar();

    }
    public void Damage(float _damage)
    {
        cHealth -= _damage;
        if (cHealth <= 0f) cHealth = 0f;
        SetHPBar();
        SetCoin(_damage);
        if (cHealth == 0) Dead();
    }

    //Weapons
    public void CharacterShoot()
    {
        if (weaponsChange == null)
            weaponsChange = FindObjectOfType<WeaponsChange>();
        if (weaponsChange.CanShot())
        {
            gameMenager.PlaySounds(audioShoot);
            weaponsChange.wasShootInThisTurn = true;
            TripleShot();
            SingleShot();
        }
    }
    //TypeOfWeapon
    void SingleShot()
    {
        if (weapons.type == Weapons.Type.Single)
        {   //create bullet and focus camera on then
            GameObject bullet = weapons.tiles;
            GameObject tile = Instantiate(bullet, tShoot.position, Quaternion.identity);
            SetWeaponsSystemToBullet(bullet.GetComponent<Bullet>());
            myCamera.SetTargetToCamera(tile.transform);
            //add force to bullet
            Vector2 dir = DirToBullet();
            Rigidbody2D componentRb = tile.GetComponent<Rigidbody2D>();
            componentRb.AddForce(dir, ForceMode2D.Impulse);
        }
    }
    void TripleShot()
    {
        if (weapons.type == Weapons.Type.Tri)
        {
            GameObject bullet = weapons.tiles;
            GameObject[] tile = new GameObject[3];
            for (int i = 0; i < 3; i++)
            {
                tile[i] = Instantiate(bullet, tShoot.position, Quaternion.identity);
                Vector2 dir = DirToBullet();
                Debug.Log(dir);
                Rigidbody2D componentRb = tile[i].GetComponent<Rigidbody2D>();
                componentRb.AddForce(dir * (1f + i / 10f), ForceMode2D.Impulse);
                if (i == 1)
                {
                    myCamera.SetTargetToCamera(tile[1].transform);
                }
            }
        }
    }
    void RepairKit() { cHealth += weapons.damage; if (cHealth > maxHealth) cHealth = maxHealth; }
    void SetWeaponsSystemToBullet(Bullet bullet) { bullet.SetWeapons(weapons); }
    //UI Setup
    public void SetFuelBar()
    {
        var bar = gameMenager.GetFuelBar();
        var text = gameMenager.GetFuelText();
        if (bar != null && text != null)
        {
            var scale = bar.GetComponent<RectTransform>();
            scale.localScale = new Vector3(fuel / 100f, scale.localScale.y, scale.localScale.z);
            text.text = ((int)fuel).ToString();
        }
    }
    public void SetForceBar()
    {
        var bar = gameMenager.GetForceBar();
        var text = gameMenager.GetForceText();
        if (bar != null)
        {
            var scale = bar.GetComponent<RectTransform>();
            scale.localScale = new Vector3(force / 2f, scale.localScale.y, scale.localScale.z);
            text.text = ((int)((force / 2f) * 100f)).ToString();
        }
    }
    public void SetHPBar()
    {
        var bar = gameMenager.GetHPBar(gameObject.tag);
        var text = gameMenager.GetHPText(gameObject.tag);
        if (bar != null && text != null)
        {
            var scale = bar.GetComponent<RectTransform>();
            scale.localScale = new Vector3(cHealth / maxHealth, scale.localScale.y, scale.localScale.z);
            text.text = ((int)cHealth).ToString();
        }
    }
    public void SetAngleText()
    {
        var text = gameMenager.GetAngleText();
        if (text != null)
        {
            text.text = ((int)angle).ToString();
        }
    }

    public void StartSetup()
    {
        rb = GetComponent<Rigidbody2D>();
        gameMenager = FindObjectOfType<GameMenager>();
        myCamera = FindObjectOfType<CameraFollow>();
        weaponsChange = FindObjectOfType<WeaponsChange>();
        gameMenager.StartFirstTurn();
        SetForceBar();
        SetFuelBar();
        SetAngleText();
        SetHPBar();

        cHealth = maxHealth;
        fuel = 100f;
    }
    void UpdateSetup()
    {
        CharacterMove(0);
        CharacterMoveCannon();
    }
    void FixedUpdate() { UpdateSetup(); }
    void CharacterFlip(float dMove)
    {
        if (dMove > 0 && !facingRight)
        {
            facingRight = !facingRight;
            Quaternion cRottation = body.transform.rotation;
            body.transform.rotation = new Quaternion(0f, 0f, cRottation.z, cRottation.w);
        }
        else if (dMove < 0 && facingRight)
        {
            facingRight = !facingRight;
            Quaternion cRottation = body.transform.rotation;
            body.transform.rotation = new Quaternion(0f, 180f, cRottation.z, cRottation.w);
        }
    }
    void CharacterMoveCannon()
    {
        Quaternion rCannon = tCannon.transform.localRotation;
        MathCannonAngle(0);
        tCannon.transform.localRotation = Quaternion.Euler(rCannon.x, rCannon.y, angle);
    }
    void SetCoin(float multiple)
    {
        float coin = Random.Range(3f, 6f);
        gameMenager.SetCoin(gameObject.tag, (int)(coin * multiple));//TODO change coin system
    }
    void Dead() { gameMenager.Kill(gameObject.tag); }
    Vector2 DirToBullet()
    {
        float bSpeed = weapons.speed * force;
        float x;
        float y;
        Vector2 dir = new Vector2(0, 0);
        if (facingRight && angle <= 45f)
        {
            x = bSpeed;
            y = bSpeed * (angle / 45f);
            dir = new Vector2(x, y);
        }
        else if (facingRight)
        {
            x = bSpeed * ((90f - angle) / 45f);
            y = bSpeed;
            dir = new Vector2(x, y);
        }
        else if (!facingRight && angle <= 45f)
        {
            x = -bSpeed;
            y = bSpeed * (angle / 45f);
            dir = new Vector2(x, y);
        }
        else if (!facingRight)
        {
            x = -bSpeed * ((90f - angle) / 45f);
            y = bSpeed;
            dir = new Vector2(x, y);
        }
        return dir;
    }
}
