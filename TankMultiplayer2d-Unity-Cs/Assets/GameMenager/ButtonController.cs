using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] bool isControlBtn;
    [SerializeField] bool isNControlBtn;
    [SerializeField] bool isForceBtn;
    [SerializeField] bool isNForceBtn;
    [SerializeField] bool isAngleBtn;
    [SerializeField] bool isNAngleBtn;
    bool isPointerDown;
    Player player;

    public void OnPointerDown(PointerEventData pointerEventData) { isPointerDown = true; }
    public void OnPointerUp(PointerEventData pointerEventData) { isPointerDown = false; }

    void Update()
    {
        if (isControlBtn && isPointerDown) { Control(); return; }
        if (isNControlBtn && isPointerDown) { NControl(); return; }
        if (isForceBtn && isPointerDown) { Force(); return; }
        if (isNForceBtn && isPointerDown) { NForce(); return; }
        if (isAngleBtn && isPointerDown) { Angle(); return; }
        if (isNAngleBtn && isPointerDown) { NAngle(); return; }
        if (isPointerDown) { Debug.Log("Fire"); return; }
    }

    void Control()
    {
        FindPlayer();
        if (player != null) player.CharacterMove(1f);
    }
    void NControl()
    {
        FindPlayer();
        if (player != null) player.CharacterMove(-1f);
    }
    void Force()
    {
        FindPlayer();
        if (player != null) player.SetForce(Time.deltaTime/10f);
    }
    void NForce()
    {
        FindPlayer();
        if (player != null) player.SetForce(-Time.deltaTime / 10f);
    }
    void Angle()
    {
        FindPlayer();
        if (player != null) player.MathCannonAngle(1f);
    }
    void NAngle()
    {
        FindPlayer();
        if (player != null) player.MathCannonAngle(-1f);
    }
    public void Fire()
    {
        FindPlayer();
        if (player != null) player.CharacterShoot();
    }
    void FindPlayer()
    {
        player = FindObjectOfType<GameMenager>().GetActivePlayer();
    }
}

