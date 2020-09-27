using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(EntityStats))]
public class PlayerCombatController : MonoBehaviour { 

    private EntityStats m_Stats;

    [Header("Attack Settings")]
    [SerializeField] Transform attackPoint; // A transform determining the origin of the attack collider.
    [SerializeField] float attackRange = 0.5f; // A float determining the range the attack will encompass.
    [SerializeField] int damage = 1;
    [SerializeField] LayerMask enemyLayers; // The layers the player's attack will check for enemies.

	[Header("Events")]
	[Space] public UnityEvent OnAttackEvent;

    private void Awake() {
        m_Stats = GetComponent<EntityStats>();

        if (OnAttackEvent == null)
			OnAttackEvent = new UnityEvent();
    }

    public void Attack(bool m_attack, bool m_block) {
        // If the player is to attack...
        if (m_attack) {
			OnAttackEvent.Invoke();

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers); // Casts a circle collider to check for enemies in range.

            //Cycles through all the enemies that have been hit and damages them.
            foreach (Collider2D enemy in hitEnemies) {
                enemy.gameObject.GetComponent<EntityStats>().TakeDamage(damage);
            }
        }

        // Sends the player's entity stats the block input info...
        m_Stats.block = m_block;
    }
}
