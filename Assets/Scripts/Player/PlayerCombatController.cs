using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EntityStats), typeof(PlayerMovementController))]
public class PlayerCombatController : MonoBehaviour { 

    private EntityStats m_Stats;

    [Header("Attack Settings")]
    [SerializeField] Transform m_attackPoint; // A transform determining the origin of the attack collider.
    [SerializeField] float m_attackRange = 0.5f; // A float determining the range the attack will encompass.
    [SerializeField] float m_attackCooldownTime = 0.25f; // How long is the cooldown between each attack.
    [SerializeField] float m_attackComboCooldownTime = 1f; // How long is the cooldown between each attack combo.
    private float m_attackCurrentCooldown = 0f; // How much cooldown to count before allowing an attack again.
    [SerializeField] int m_damage = 1;
    [SerializeField] LayerMask enemyLayers; // The layers the player's attack will check for enemies.

    [Header("Combo Settings")]
    [SerializeField] int m_comboCount = 0; // How many attacks the player has made.
    [SerializeField] int m_maxComboCount = 3; // How many attacks until the combo finishes.
    [SerializeField] float m_maxTimeBetweenAttacks = 1f; // How much time to count until combo is invalid.
    private float m_currentTimeBetweenAttacks = 0f; // How much time until combo is invalid.

	[Header("Events")]
	[Space] public UnityEvent OnAttackEvent;

    private void Awake() {
        m_Stats = GetComponent<EntityStats>();

        if (OnAttackEvent == null)
			OnAttackEvent = new UnityEvent();
    }

    private void Update() {
        //Combo Manager.
        if (m_comboCount >= m_maxComboCount) {
            m_comboCount = 0;
            m_attackCurrentCooldown = 0;
            m_attackCurrentCooldown = m_attackComboCooldownTime;
        }

        m_currentTimeBetweenAttacks += Time.deltaTime;
        if (m_currentTimeBetweenAttacks >= m_maxTimeBetweenAttacks) {
            m_comboCount = 0;
        }

        //Attack Countdown.
        if (m_attackCurrentCooldown > 0) {
            m_attackCurrentCooldown -= Time.deltaTime;
        }
    }

    public void Attack(bool m_attack, bool m_block) {
        // If the player is to attack...
        if (m_attack && m_attackCurrentCooldown <= 0 && !m_block) {
            
            //Combo manager.
            m_comboCount++;
            m_currentTimeBetweenAttacks = 0;

            //Attack cooldown manager.
            m_attackCurrentCooldown = m_attackCooldownTime;

			OnAttackEvent.Invoke();

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRange, enemyLayers); // Casts a circle collider to check for enemies in range.

            //Cycles through all the enemies that have been hit and damages them.
            foreach (Collider2D enemy in hitEnemies) {
                enemy.gameObject.GetComponent<EntityStats>().TakeDamage(m_damage);
            }
        }

        // Sends the player's entity stats the block input info...
        m_Stats.block = m_block;
    }
}
