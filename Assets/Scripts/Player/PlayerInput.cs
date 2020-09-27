using UnityEngine;

[RequireComponent(typeof(PlayerMovementController), typeof(PlayerCombatController))]
public class PlayerInput : MonoBehaviour {

    private PlayerMovementController m_movementController;
    private PlayerCombatController m_combatController;
    private float m_horizontalMove;
    private float m_verticalMove;
    private bool m_dash = false;
    private bool m_attack = false;
    private bool m_block = false;


    private void Awake() {
        // Gets the player PlayerMovementController & PlayerCombatController
        m_movementController = GetComponent<PlayerMovementController>();
        m_combatController = GetComponent<PlayerCombatController>();
    }

    private void Update() {
        // If the player is to move...
        m_horizontalMove = Input.GetAxisRaw("Horizontal");
        m_verticalMove = Input.GetAxisRaw("Vertical");

        // If the player is to dash...
        if (Input.GetButtonDown("Dash")) {
            m_dash = true;
        }

        // If the player is to attack... 
        if (Input.GetButtonDown("Attack")) {
            m_attack = true;
        }

        // If the player is to block...
        if (Input.GetButtonDown("Block")) {
            m_block = true;
        }
        else if (Input.GetButtonUp("Block")) {
            m_block = false;
        }    
    }

    private void FixedUpdate() {
        // Send information to the PlayerMovementController
        m_movementController.Move(m_horizontalMove * Time.fixedDeltaTime, m_verticalMove * Time.fixedDeltaTime, m_dash);
        m_dash = false;

        // Send information to the PlayerCombatController
        m_combatController.Attack(m_attack, m_block);
        m_attack = false;
    }
}
