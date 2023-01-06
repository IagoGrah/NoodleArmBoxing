using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] float minVelocityToDamage = 20f;
    [SerializeField] float damageMultiplier = 1f;

    Player parentPlayer;

    void Start()
    {
        parentPlayer = GetComponentInParent<Player>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Head") && collision.gameObject.transform.parent != parentPlayer.transform)
        {
            var hitVelocity = collision.relativeVelocity.sqrMagnitude;
            var damage = VelocityToDamage(hitVelocity);

            if (damage > 0)
            {
                var player = collision.gameObject.GetComponentInParent<Player>();
                player.Damage(damage, collision.relativeVelocity);
            }
        }
    }

    private int VelocityToDamage(float velocity)
    {
        if (velocity < minVelocityToDamage)
        {
            return 0;
        }

        return Mathf.FloorToInt(velocity / 10 * damageMultiplier);
    }
}
