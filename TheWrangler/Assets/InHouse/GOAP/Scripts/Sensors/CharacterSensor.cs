using JUTPS.CharacterBrain;
using UnityEngine;

namespace TheWrangler.GOAP.Sensors
{
    [RequireComponent(typeof(SphereCollider))]
    public class CharacterSensor : MonoBehaviour
    {
        [HideInInspector] public SphereCollider sphereCollider;
        public delegate void CharacterEnterEvent(Transform character);
        public delegate void CharacterExitEvent(Vector3 lastKnownPosition);

        public event CharacterEnterEvent OnCharacterEnter;
        public event CharacterExitEvent OnCharacterExit;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out JUCharacterBrain brain))
            {
                OnCharacterEnter?.Invoke(brain.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out JUCharacterBrain brain))
            {
                OnCharacterExit?.Invoke(brain.transform.position);
            }
        }
    }
}