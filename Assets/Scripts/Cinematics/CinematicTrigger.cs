using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {


        private bool _hasPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                var playableDirector = GetComponent<PlayableDirector>();

                if (_hasPlayed)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    _hasPlayed = true;
                    playableDirector.Play();
                }
            }

        }
    }
}
