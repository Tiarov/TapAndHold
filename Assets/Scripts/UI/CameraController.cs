using Assets.Scripts.Helper;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField, Range(1, 5)]
        private float Speed = 2;

        [SerializeField]
        private Vector3 Offset;

        private Transform _currentTarget;
        private Transform _transform;
        private bool _isCinematicFinal;
        private Vector3 _finalPosition;

        private void Start()
        {
            _transform = transform;
            EventManager.BuildedNewMeshEvent += OnBuildNewMesh;
            EventManager.DestroyMeshEvent += OnDestroyMesh;
        }

        private void OnDestroy()
        {
            EventManager.BuildedNewMeshEvent -= OnBuildNewMesh;
            EventManager.DestroyMeshEvent -= OnDestroyMesh;
        }

        private void Update()
        {
            if (_isCinematicFinal)
            {
                MoveToPosition();
                Rotate();
            }
            else
            {
                MoveToTarget();
            }
        }

        private void MoveToTarget()
        {
            if (_currentTarget)
                _transform.position = Vector3.Slerp(_transform.position, _currentTarget.position + Offset, Speed * Time.deltaTime);
        }

        private void MoveToPosition()
        {
            _transform.position = Vector3.Slerp(_transform.position, _finalPosition, Speed * Time.deltaTime);
        }

        private void Rotate()
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.identity, Speed * Time.deltaTime);
        }

        private void OnBuildNewMesh(Transform sender)
        {
            _currentTarget = sender;
        }

        private void OnDestroyMesh(Transform sender)
        {
            var value = sender.position.y / 2;
            _finalPosition = new Vector3(0, value + 2, (-value * 2) - 4);
            _isCinematicFinal = true;
        }
    }
}
