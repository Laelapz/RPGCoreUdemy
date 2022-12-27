using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private int _sceneToLoadIndex;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float _fadeOutTime;
        [SerializeField] private float _fadeInTime;
        [SerializeField] private float _fadeWaitTime;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != Mathf.Log(_playerLayer.value, 2)) return;

            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if(_sceneToLoadIndex < 0)
            {
                Debug.LogError("Scene to load not set");
            }

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(_fadeOutTime);

            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();

            DontDestroyOnLoad(this);
            yield return SceneManager.LoadSceneAsync(_sceneToLoadIndex);

            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(_fadeWaitTime);

            yield return fader.FadeIn(_fadeInTime);
            Destroy(this);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player =  GameObject.FindGameObjectWithTag("Player");
            NavMeshAgent playerNavMeshAgent = player.GetComponent<NavMeshAgent>();
            playerNavMeshAgent.enabled = false;
            playerNavMeshAgent.Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            playerNavMeshAgent.enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach( Portal portal in FindObjectsOfType<Portal>() )
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
        }
    }
}
