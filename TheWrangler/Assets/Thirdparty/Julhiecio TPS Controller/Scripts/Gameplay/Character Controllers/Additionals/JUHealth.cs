using CrashKonijn.Goap.Core;
using JUTPS.FX;
using JUTPSEditor.JUHeader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UI.GridLayoutGroup;

namespace JUTPS
{

    [RequireComponent(typeof(NeedsController))]
    [AddComponentMenu("JU TPS/Third Person System/Additionals/JU Health")]
    public class JUHealth : MonoBehaviour
    {
        [JUHeader("Settings")]
        [SerializeField] protected NeedsOwner owner;

        [JUHeader("Effects")]
        public bool HitEffect = false;
        public GameObject HitParticle;

        [JUHeader("On Destroyed Event")]
        public UnityEvent OnDestroyed;

        public float NormalizedValue { get; private set; }
        public NeedsBroadcastReason lastReason { get; private set; }

        protected NeedsType needsType = NeedsType.HEALTH;
        protected string guid;


        private void Start()
        {
            guid = GetComponent<NeedsController>().guid;
        }

        protected virtual void OnEnable()
        {
            GameEventsManager.Instance.NeedsEvents.onBroadcastNeedsUpdate += OnBroadcastNeedsUpdate;
        }

        protected virtual void OnDisable()
        {
            GameEventsManager.Instance.NeedsEvents.onBroadcastNeedsUpdate -= OnBroadcastNeedsUpdate;
        }

        protected virtual void OnBroadcastNeedsUpdate(NeedsBroadcastEvent e)
        {
            if (e.owner == owner && e.guid == guid && e.need.info.NeedsType == needsType)
            {
                if (e.reason == NeedsBroadcastReason.REACHED_MINIMUM)
                {
                    //Disable all damagers0
                    foreach (Damager dmg in GetComponentsInChildren<Damager>()) dmg.gameObject.SetActive(false);

                    OnDestroyed.Invoke();
                }

                lastReason = e.reason;
                NormalizedValue = e.need.NormalizedValue;
            }
        }

        public void DoDamage(NeedsOwner owner, float value, Vector3 hitPosition = default(Vector3))
        {
            if (owner == this.owner)
                return;

            GameEventsManager.Instance.NeedsEvents.UpdateNeeds(new NeedsUpdateEvent
            {
                owner = this.owner,
                guid = guid,
                type = needsType,
                reason = NeedsUpdateReason.DECREASE,
                value = value
            });

            if (HitEffect && value > 0) BloodScreen.PlayerTakingDamaged();
            if (hitPosition != Vector3.zero && HitParticle != null)
            {
                GameObject fxParticle = Instantiate(HitParticle, hitPosition, Quaternion.identity);
                fxParticle.hideFlags = HideFlags.HideInHierarchy;
                Destroy(fxParticle, 3);
            }
        }

        public void DoHeal(float value, Vector3 hitPosition = default(Vector3))
        {
            GameEventsManager.Instance.NeedsEvents.UpdateNeeds(new NeedsUpdateEvent
            {
                owner = owner,
                guid = guid,
                type = needsType,
                reason = NeedsUpdateReason.INCREASE,
                value = value
            });
        }

        public void ResetHealth()
        {
            GameEventsManager.Instance.NeedsEvents.UpdateNeeds(new NeedsUpdateEvent
            {
                owner = owner,
                guid = guid,
                type = needsType,
                reason = NeedsUpdateReason.RESET,
            });
        }

        public void SetMinimum()
        {
            GameEventsManager.Instance.NeedsEvents.UpdateNeeds(new NeedsUpdateEvent
            {
                owner = owner,
                guid = guid,
                type = needsType,
                reason = NeedsUpdateReason.SET_MINIMUM,
            });
        }
    }
}