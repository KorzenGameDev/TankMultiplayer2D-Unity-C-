using UnityEngine;

public class Bullet : MonoBehaviour
{
    Weapons weapon=null;
    [SerializeField] CircleCollider2D destructionArea = null;
    [SerializeField] AudioClip audioExplosion = null;
    public static GroundController groundController;

    public void SetWeapons(Weapons wep) { weapon = wep; }
    public void GiveDemage()
    {
        Collider2D[] objToDamage = Physics2D.OverlapCircleAll(transform.position, destructionArea.radius);
        foreach (Collider2D obj in objToDamage)
        {
            var demagable = obj.GetComponent<IDamageable<float>>();
            if (demagable != null)
            {
                demagable.Damage(weapon.damage);
                Debug.Log(obj.name + weapon.damage);
            }
        }
        groundController.DestroyGround(destructionArea);
        Destroy(gameObject);
    }
    private void ParticleEffect()
    {
        GameObject effect = Instantiate(weapon.effect, transform.position, Quaternion.identity);
        ParticleSystem particle = effect.GetComponent<ParticleSystem>();
        Destroy(effect, particle.main.duration);
    }
    private void NexTurn()
    {
        GameMenager gameMenager = FindObjectOfType<GameMenager>();
        gameMenager.PlaySounds(audioExplosion);
        gameMenager.StartNewTurn();
    }
    bool ColliderDestructionObject(Collision2D collision) { return (collision.collider.CompareTag("Player1") || collision.collider.CompareTag("Player2") || collision.collider.CompareTag("Ground")); }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (weapon.timer==0 && ColliderDestructionObject(collision))
        {
            ParticleEffect();
            NexTurn();
            GiveDemage();
        }
    }
}
