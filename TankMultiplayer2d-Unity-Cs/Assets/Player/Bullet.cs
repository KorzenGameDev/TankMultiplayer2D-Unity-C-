using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Weapons weapon=null;
    [SerializeField] CircleCollider2D destructionArea = null;
    [SerializeField] AudioClip audioExplosion = null;
    public static GroundController groundController;

    public void GiveDemage()
    {
        Collider2D[] objToDamage = Physics2D.OverlapCircleAll(transform.position, destructionArea.radius);
        foreach (Collider2D obj in objToDamage)
        {
            var demagable = obj.GetComponent<IDamageable<float>>();
            if (demagable != null)
            {
                demagable.Damage(weapon.demage);
                Debug.Log(obj.name + weapon.demage);
            }
        }
        groundController.DestroyGround(destructionArea);
        GameMenager gameMenager = FindObjectOfType<GameMenager>();
        gameMenager.PlaySounds(audioExplosion);
        gameMenager.StartNewTurn();
        
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player1") || collision.collider.CompareTag("Player2") || collision.collider.CompareTag("Ground"))
        {
            GiveDemage();
        }
    }
}
