using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab = null;

        void Start()
        {
            float num = Random.Range(1, 100);
            Spawn(num);
        }

        public void Spawn(float damageTextValue)
        {
            DamageText damageTextGameObject = Instantiate<DamageText>(damageTextPrefab, transform);
            damageTextGameObject.SetText(damageTextValue.ToString());
        }
    }

}