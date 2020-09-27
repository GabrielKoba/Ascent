using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour {

	[Header("Movement Settings")]
	public float m_moveSpeed = 35f; // How fast the player moves.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement
	public float m_dashSpeed = 30f; // How fast the player moves during a dash.
	public float m_dashDuration = 0.25f; // How long is the duration of the dash.
	public float m_dashCooldown = 1f; // How long is the duration of the dash.

	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private bool m_dashing = false;  // For determining if the player is currently dashing.

    [Header("Animation Settings")]
	[SerializeField] string m_movementBool; // The name of the float that dictates if the character is moving in the Animator.
	[SerializeField] string m_horizontalMovementFloat; // The name of the float that dictates horizontal directions in the Animator.
	[SerializeField] string m_verticalMovementFloat; // The name of the float that dictates vertical directions in the Animator.
    private Animator m_Animator;

	[Header("Events")]
	[Space] public UnityEvent OnDashStart;
	[Space] public UnityEvent OnDashStop;

	private void Awake() {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    
		if (OnDashStart == null)
			OnDashStart = new UnityEvent();

		if (OnDashStop == null)
			OnDashStop = new UnityEvent();
	}

	public void Move(float horizontalMove, float verticalMove, bool dash) {
		// dash button pressed
		if (dash && !m_dashing) {
			m_dashing = true;
			StartCoroutine(Dash(horizontalMove, verticalMove));
		}

		//only control the player if not dashing
		if (!dash){
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(horizontalMove, verticalMove);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity.normalized * m_moveSpeed, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (horizontalMove > 0 && !m_FacingRight) {
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (horizontalMove < 0 && m_FacingRight) {
				// ... flip the player.
				Flip();
			}

			// Feeds directional information to animator.
			if (horizontalMove != 0 || verticalMove != 0) {
				m_Animator.SetBool(m_movementBool, true);
				m_Animator.SetFloat(m_horizontalMovementFloat, horizontalMove);
				m_Animator.SetFloat(m_verticalMovementFloat, verticalMove);
			}
			else {
				m_Animator.SetBool(m_movementBool, false);
			}
		}
	}

	private void Flip() {
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	IEnumerator Dash(float horizontalMove, float verticalMove) {
		m_Rigidbody2D.velocity = new Vector2(0, 0);
		Vector2 dashDirection = new Vector2(horizontalMove, verticalMove);
		m_Rigidbody2D.velocity += dashDirection.normalized * m_dashSpeed;

		OnDashStart.Invoke();
		yield return new WaitForSeconds(m_dashDuration);

		m_Rigidbody2D.velocity = new Vector2(0, 0);

		OnDashStop.Invoke();
		StartCoroutine(DashCooldown());
	}

	IEnumerator DashCooldown() {
		yield return new WaitForSeconds(m_dashCooldown);
		m_dashing = false;
	}
}
