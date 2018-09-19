using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helper;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class EffectsManager : MonoBehaviour
    {
        public GameObject PerfectMoveParticlePrefab;

        [SerializeField, Range(2, 25)]
        private int EffectsPoolCount = 4;

        private Queue<GameObject> _effects;

        private void Start()
        {
            InitEffects();
            EventManager.IncorrectMeshEvent += OnIncorrectMesh;
            EventManager.PerfectMoveEvent += OnPerfectMove;
        }

        private void OnDestroy()
        {
            EventManager.IncorrectMeshEvent -= OnIncorrectMesh;
            EventManager.PerfectMoveEvent -= OnPerfectMove;
        }

        private void InitEffects()
        {
            _effects = new Queue<GameObject>();
            for (int i = 0; i < EffectsPoolCount; i++)
            {
                var effect = Instantiate(PerfectMoveParticlePrefab, transform);
                effect.SetActive(false);
                _effects.Enqueue(effect);
            }
        }

        private void OnPerfectMove(Transform sender)
        {
            var effect = _effects.Dequeue();
            effect.transform.position = sender.position;
            effect.SetActive(false);
            effect.SetActive(true);
            _effects.Enqueue(effect);
        }

        private void OnIncorrectMesh(Transform sender)
        {
            var mesh = sender.GetChild(0).gameObject;
            var r = mesh.GetComponent<Renderer>();
            r.material = new Material(r.material);
            r.material.color = Color.red;

            StartCoroutine(DestroyMesh(2, sender));
        }

        private IEnumerator DestroyMesh(float time, Transform sender)
        {
            yield return new WaitForSeconds(time);

            sender.gameObject.SetActive(false);
            EventManager.DestroyMesh(sender);
        }
    }
}
