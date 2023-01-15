using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab = null;

        private Vector3 GetOffset()
        {
            Vector3 pos = transform.position;
            pos.x += Random.Range(-0.3f, 0.4f);
            pos.y += Random.Range(-0.1f, 0.2f);
            return pos;
        }

        public void Spawn(float[] data)
        {
            DamageText damageTextGameObject = Instantiate<DamageText>(damageTextPrefab, GetOffset(), Quaternion.identity);
            damageTextGameObject.SetText(data[0].ToString());
        }
    }

}