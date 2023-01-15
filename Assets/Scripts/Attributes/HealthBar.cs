using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image _lifeBar = null;
        public void SetLifeBarSize(float[] data)
        {
            float size = data[1];
            Vector3 scale = _lifeBar.rectTransform.localScale;
            scale.x = size;
            _lifeBar.rectTransform.localScale = scale;
        }
    }
}
