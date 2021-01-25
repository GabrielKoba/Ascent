using UnityEngine;
using UnityEngine.Events;

public class EntityStats : MonoBehaviour {

    [Header("Health Settings")]
    [SerializeField] public int maxHealth = 5;
    [SerializeField] public int currentHealth = 5;

    [Header("Block Settings")]
    [SerializeField] bool entityCanBlock;   // Is entity able to block?
    [SerializeField] private bool myBlock;  // Bool to see if the blocking process is currently happening.
    public bool block {  // Is entity is blocking?
        get { return myBlock; }
        set {
        if (value == myBlock)
            return;
            myBlock = value ;

            if (myBlock && blockOnCooldown) {
                BlockOnCooldownEvent.Invoke();
            }    
        }
    }    

    [SerializeField] float blockCooldownTime = 1f;  // How much cooldown time, in seconds, to block after a failed block.
    public bool blockOnCooldown;  // Check if the entity's block is on cool down.
    private float blockCooldownElapsed = 0f;    // Float to check how much time left on cooldown.
    [SerializeField] float blockDuration = 0.25f;   // How much time, in seconds, the block stays active.
    [SerializeField] private bool myBlocking;  // Bool to see if the blocking process is currently happening.
    public bool blocking {
        get { return myBlocking; }
        set {
        if (value == myBlocking)
            return;
            myBlocking = value ;

            if (myBlocking) {
                OnStartBlockingEvent.Invoke();
            }
            else if (!myBlocking) {
                OnStopBlockingEvent.Invoke();   
            }
        }    
    }
    
    private float blockDurationElapsed = 0f;    // Float to check how much time left on block.

    [Header("Event Settings")]
    [Space] public UnityEvent OnBlockEvent;
    [Space] public UnityEvent OnStartBlockingEvent;
    [Space] public UnityEvent OnStopBlockingEvent;
    [Space] public UnityEvent BlockOnCooldownEvent;
    [Space] public UnityEvent OnDamagedEvent;
	[Space] public UnityEvent OnDeathEvent;

    private void Awake() {
        currentHealth = maxHealth;
        blocking = false;

		if (OnBlockEvent == null)
			OnBlockEvent = new UnityEvent();
        
		if (OnStartBlockingEvent == null)
			OnStartBlockingEvent = new UnityEvent();

        if (OnStopBlockingEvent == null)
			OnStopBlockingEvent = new UnityEvent();

		if (BlockOnCooldownEvent == null)
			BlockOnCooldownEvent = new UnityEvent();

        if (OnDamagedEvent == null)
			OnDamagedEvent = new UnityEvent();

		if (OnDeathEvent == null)
			OnDeathEvent = new UnityEvent();
    }

    void Update() {
        if (entityCanBlock) {
            BlockHandler();
        }
    }

    public void TakeDamage(int damage) {
        if (currentHealth > 0 && !blocking) { 
            currentHealth -= damage; // Damages the entity based on how much damage it receives.

            OnDamagedEvent.Invoke();

            if (currentHealth <= 0) {
                OnDeathEvent.Invoke();
            }
        }
        else if (blocking) {
            OnBlockEvent.Invoke();

            blocking = false; // Confirms the block process
            blockOnCooldown = true;     // Puts the block on cooldown.
            blockDurationElapsed = 0;   // Resets the timer on the duration of the block.
        }
    }

    private void BlockHandler() {
        // Puts the block on cooldown timer after a block...
        while (blockOnCooldown && blockCooldownElapsed < blockCooldownTime) {
            blockCooldownElapsed += Time.deltaTime;     //Counts the block cooldown duration.

            return;
        }

        // If block cooldown timer is done...
        if (blockCooldownElapsed > blockCooldownTime) {
            blockOnCooldown = false;    // Stops the cooldown on the block
            blockCooldownElapsed = 0;   // Resets the timer on the cooldown of the block.
        }

        // If is to block & not on cooldown...
        while (block && !blockOnCooldown && blockDurationElapsed < blockDuration) {
            blocking = true;    // Confirms the block process
            blockDurationElapsed += Time.deltaTime;     // Counts the block duration.

            // If the block duration ends...
            if (!blockOnCooldown && blockDurationElapsed > blockDuration || !block && blocking) {
                blocking = false; // Confirms the block process
                blockOnCooldown = true;     // Puts the block on cooldown.
                blockDurationElapsed = 0;   // Resets the timer on the duration of the block.
            }

            return;
        }

        if (!myBlock) {
            blocking = false; // Confirms the block process
            blockOnCooldown = true; // Puts the block on cooldown.
            blockDurationElapsed = 0; // Resets the timer on the duration of the block.
        }
    }
}
