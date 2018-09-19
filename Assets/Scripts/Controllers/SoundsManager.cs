using System.Collections;
using Assets.Scripts.Helper;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class SoundsManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource AudioSource;

        [SerializeField]
        private GameSettings Settings;

        [Space, SerializeField]
        private AudioClip BackgroundMusicClip;
        [SerializeField]
        private AudioClip NewMeshClip;
        [SerializeField]
        private AudioClip PerfectMoveClip;
        [SerializeField]
        private AudioClip IncorrectMeshClip;
        [SerializeField]
        private AudioClip DestroyMeshClip;

        private void OnEnable()
        {
            EventManager.BuildedNewMeshEvent += OnBuildedNewMesh;
            EventManager.IncorrectMeshEvent += OnIncorrectMesh;
            EventManager.PerfectMoveEvent += OnPerfectMove;
            EventManager.DestroyMeshEvent += OnDestroyMesh;

            InitBackgroundMusic();
        }

        private void OnDisable()
        {
            EventManager.BuildedNewMeshEvent -= OnBuildedNewMesh;
            EventManager.IncorrectMeshEvent -= OnIncorrectMesh;
            EventManager.PerfectMoveEvent -= OnPerfectMove;
            EventManager.DestroyMeshEvent -= OnDestroyMesh;
        }

        private void InitBackgroundMusic()
        {
            AudioSource.clip = BackgroundMusicClip;
            AudioSource.volume = Settings.MusicVolume;
            AudioSource.loop = true;

            AudioSource.Play();
        }

        private void OnBuildedNewMesh(Transform sender)
        {
            AudioSource.PlayClipAtPoint(NewMeshClip, sender.position, Settings.EffectsVolume * 0.8f);
        }

        private void OnPerfectMove(Transform sender)
        {
            AudioSource.PlayClipAtPoint(PerfectMoveClip, sender.position, Settings.EffectsVolume);
        }

        private void OnIncorrectMesh(Transform sender)
        {
            AudioSource.PlayClipAtPoint(IncorrectMeshClip, sender.position, Settings.EffectsVolume);
            StartCoroutine(DisablingMusic());
        }

        private void OnDestroyMesh(Transform sender)
        {
            AudioSource.PlayClipAtPoint(DestroyMeshClip, sender.position, Settings.EffectsVolume);
        }
    
        private IEnumerator DisablingMusic()
        {
            while (AudioSource.volume > 0)
            {
                AudioSource.volume -= Time.deltaTime * 0.3f;
                yield return new WaitForEndOfFrame();
            }
            AudioSource.Stop();
        }
    }
}
