using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerEvents : MonoBehaviour {

    private Animator m_Animator;

    [Header("Animation Settings")]
    [SerializeField] string m_dashAnimBool; // The name of the bool to play the entity's jump animation in the animator.
    [SerializeField] string m_attackAnimTrigger; // The name of the trigger to play the entity's attack animation in the animator.
    [SerializeField] string m_blockAnimBool;    // The name of the bool to play the entity's block animation in the animator.

    [Header("Audio Settings")]
	[FMODUnity.EventRef][SerializeField] string stepSFX; //The sound it plays once the player takes a step.
    [FMODUnity.EventRef][SerializeField] string dashStartSFX; //The sound it plays once the player starts dashing.
    [FMODUnity.EventRef][SerializeField] string dashStopSFX; //The sound it plays once the player stops dashing.
    [FMODUnity.EventRef][SerializeField] string hurtSFX;    // The sound it plays once the player is hurt.
    [FMODUnity.EventRef][SerializeField] string deathSFX;    // The sound it plays once the player is hurt.
    [FMODUnity.EventRef][SerializeField] string attackSFX; // The sound it plays once the player attacks.
    [FMODUnity.EventRef][SerializeField] string blockSFX;    // The sound it plays once the player successfully blocks an attack.
    [FMODUnity.EventRef][SerializeField] string blockingSFX;    // The sound it plays once the player successfully blocks an attack.
    private FMOD.Studio.EventInstance blockingSFX_Instance;
    [FMODUnity.EventRef][SerializeField] string blockOnCooldownSFX;   // The sound it plays once the player blocks but is on cooldown.

    //Get needed components.
    private void Awake() {
        m_Animator = GetComponent<Animator>();
    }

	public void OnStep() {
		// Plays the sound for when the player takes a step.
		FMODUnity.RuntimeManager.PlayOneShot(stepSFX, transform.position);
	}

    public void OnDashStart() {
        m_Animator.SetBool(m_dashAnimBool, true); // Starts the character's dash animation

		// Plays the sound for when the player starts dashing.
		FMODUnity.RuntimeManager.PlayOneShot(dashStartSFX, transform.position);
	}
    public void OnDashStop() {
        m_Animator.SetBool(m_dashAnimBool, false); // Stops the character's dash animation

        // Plays the sound for when the player stops dashing.
		FMODUnity.RuntimeManager.PlayOneShot(dashStopSFX, transform.position);
	}

    public void OnDamageTaken() {
        // Plays the sound for when the player gets hurt.
        FMODUnity.RuntimeManager.PlayOneShot(hurtSFX, transform.position);
    }

    public void OnDeath() {
        // Plays the sound for when the player dies.
        FMODUnity.RuntimeManager.PlayOneShot(deathSFX, transform.position);
    }

    public void OnAttack() {
        m_Animator.SetTrigger(m_attackAnimTrigger); // Triggers the character's attack animation

		// Plays the sound for when the player attacks.
        FMODUnity.RuntimeManager.PlayOneShot(attackSFX, transform.position);
    }

    public void OnBlock() {
        m_Animator.SetBool(m_blockAnimBool, false); // Ends block animation.

		// Plays the sound for when the player successfully blocks an attack.
        FMODUnity.RuntimeManager.PlayOneShot(blockSFX, transform.position);
        blockingSFX_Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE); // Stops blocking sound
        blockingSFX_Instance.release();
    }

    public void OnStartBlocking() {
        Debug.Log("Blocking");

        m_Animator.SetBool(m_blockAnimBool, true);  // Starts blocking animation.

        // Plays the sound for when the player is blocking.
        blockingSFX_Instance = FMODUnity.RuntimeManager.CreateInstance(blockingSFX);
        blockingSFX_Instance.start();
    }

    public void OnStopBlocking() {
        Debug.Log("Not Blocking");

        m_Animator.SetBool(m_blockAnimBool, false); // Ends blocking animation.

        // Stops the sound for when the player stops blocking.
        blockingSFX_Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE); // Stops blocking sound
        blockingSFX_Instance.release();
    }

    public void BlockOnCooldown() {
        // Plays the sound for when the player is trying to block on cooldown.
        FMODUnity.RuntimeManager.PlayOneShot(blockOnCooldownSFX, transform.position);
    }
}
