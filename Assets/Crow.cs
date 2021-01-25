using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Crow : MonoBehaviour {

    [Header("Behaviour Settings")]
    [SerializeField] LayerMask reactToMask; // Which layers the crow will react to.

    [Header("Animation Settings")]
    [SerializeField] string m_flightBool; // The name of the bool to start the crow's flight.
    private Animator m_Animator;

    [Header("Sound Settings")]
    [SerializeField] float minSoundDelay = 1f;
    [SerializeField] float manSoundDelay = 10f;
    [FMODUnity.EventRef][SerializeField] string idleSFX; // The sound it plays when the crow is idling.
    [FMODUnity.EventRef][SerializeField] string cawSFX; // The sound it plays once the crow flies away.
    private float m_SoundDelay;
    private FMOD.Studio.EventInstance idleSFX_Instance;

    private void Awake() {
        m_Animator = GetComponent<Animator>();
    }

    private void Start() {
        // PLays crow idle sound and stores it.
        idleSFX_Instance = FMODUnity.RuntimeManager.CreateInstance(idleSFX);
        idleSFX_Instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

        m_SoundDelay = (Random.Range(minSoundDelay, manSoundDelay));

        StartCoroutine(SoundDelay());
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (reactToMask == (reactToMask | (1 << col.gameObject.layer))) {
            // Triggers crow flight animation
            m_Animator.SetBool(m_flightBool, true);

            // Stops idling sound
            idleSFX_Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            idleSFX_Instance.release();

            // Plays crow flie away SFX
            FMODUnity.RuntimeManager.PlayOneShot(cawSFX, transform.position);
        }
    }

    private IEnumerator SoundDelay() {
        yield return new WaitForSeconds(m_SoundDelay);   

        idleSFX_Instance.start();
    }
}