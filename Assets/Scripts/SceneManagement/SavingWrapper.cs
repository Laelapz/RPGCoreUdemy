using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] private float _fadeInTime = 0.4f;

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();

            fader.FadeOutimmediately();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(_fadeInTime);
        }

        private void Update()
        {
            if( Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);        
        }
    }
}
