using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helper;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class MeshesManager : MonoBehaviour
    {
        [SerializeField, Range(0.05f, 0.5f)]
        private float _waveTimeout = 0.2f;

        private List<MeshController> _meshes;
        private Transform _current;

        private bool _isGrowingNew;
        private float _increaseSpeed;
        private float _maxGrowableSize;

        private bool _isActiveWave;

        private void Update()
        {
            if (_isActiveWave)
            {
                WaveUpdate();
            }

            if (_isGrowingNew)
            {
                IncreaseScaleNewMesh();
            }
        }

        public void Init(GameSettings settings)
        {
            _meshes = new List<MeshController>();
            _increaseSpeed = settings.DifficultyLevel;
        }

        public void StartBuild(float maxGrowableSize, MeshController meshController)
        {
            _current = meshController.Transform;
            _current.localScale = new Vector3(0, _current.localScale.y, 0);
            _current.gameObject.SetActive(true);

            _maxGrowableSize = maxGrowableSize;
            _isGrowingNew = true;

            _meshes.Add(meshController);
        }

        public void StopIncrease()
        {
            _isGrowingNew = false;
        }

        public void Wave()
        {
            _isActiveWave = true;
            StartCoroutine(StartWave());
        }

        private void IncreaseScaleNewMesh()
        {
            _current.localScale += new Vector3(_increaseSpeed, 0, _increaseSpeed) * Time.deltaTime;
            if (_current.localScale.x >= _maxGrowableSize)
            {
                EventManager.IncorrectMesh(_current);
                _isGrowingNew = false;
            }
        }

        private void WaveUpdate()
        {
            var counter = 0;
            for (int i = _meshes.Count - 1; i >= 0; i--)
            {
                if (!_meshes[i].TryUpdateScale())
                    counter++;
            }

            if (counter == _meshes.Count)
            {
                _isActiveWave = false;
            }
        }

        private IEnumerator StartWave()
        {
            _meshes[_meshes.Count - 1].ActivateAnimation(0.4f);
            yield return new WaitForSeconds(_waveTimeout);

            for (int i = _meshes.Count - 2; i >= 0; i--)
            {
                _meshes[i].ActivateAnimation();
                yield return new WaitForSeconds(_waveTimeout);
            }
        }
    }
}
