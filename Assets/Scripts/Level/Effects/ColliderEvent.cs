using UnityEngine;
using UnityEngine.Events;

public class ColliderEvent : MonoBehaviour {
  
    [Header("Events")]
	[Space] public UnityEvent OnEventCalled;

	private void Awake() {
		if (OnEventCalled == null)
			OnEventCalled = new UnityEvent();
	}

    void OnCollisionEnter2D(Collision2D col) {
        OnEventCalled.Invoke();
    }
}
