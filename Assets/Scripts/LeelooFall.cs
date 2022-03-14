using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    public class LeelooFall : MonoBehaviour
    {
        [Header("Object links")]
        [SerializeField]
        private Transform leeloo;
        [SerializeField]
        private GameObject hostileDummy;
        [SerializeField]
        private GameObject hitEffect;
        [SerializeField]
        private Rigidbody rbPlayer;

        [Header("Appear settings")]
        [SerializeField]
        private float minAppearDelay = 20f;
        [SerializeField]
        private float maxAppearDelay = 40f;

        [Header("Fall settings")]
        [SerializeField]
        private float fallTime = 3f;
        [SerializeField]
        private float fallDistance = 4f;

        [Header("Hit settings")]
        [SerializeField]
        private float hitForceValue = 3f;

        private Timer _timer = new Timer();

        private void OnEnable()
        {
            leeloo.gameObject.SetActive(false);
            hostileDummy.SetActive(false);
            hitEffect.SetActive(false);

            StartCoroutine(AppearCoroutine());
        }

        private void LateUpdate()
        {
            if (_timer.IsActive)
                UpdateLeelooPosition(_timer.Progress);
            else if (leeloo.gameObject.activeSelf)
                //we are in car - activate effect
                ActivateHostile();
        }

        IEnumerator AppearCoroutine()
        {
            yield return new WaitForSeconds(Random.Range(minAppearDelay, maxAppearDelay));

            _timer.Activate(fallTime);
            UpdateLeelooPosition(_timer.Progress);
            leeloo.gameObject.SetActive(true);
        }

        private void UpdateLeelooPosition(float progress)
        {
            leeloo.position = transform.position + Vector3.up * fallDistance * (1f - progress);
        }

        private void ActivateHostile()
        {
            //add hit force impulse
            rbPlayer.AddForce(Vector3.down * rbPlayer.mass * hitForceValue, ForceMode.Impulse);

            //hide leeloo and activate dummy hostile
            leeloo.gameObject.SetActive(false);
            hostileDummy.SetActive(true);
            hitEffect.SetActive(true);
        }
    }
}