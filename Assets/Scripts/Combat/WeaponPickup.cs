using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private WeaponSO weapon;
        [SerializeField] private float respawnTime = 5f;
        SphereCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            other.GetComponent<Fighter>().EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(respawnTime);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            _collider.enabled = shouldShow;
            
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}
