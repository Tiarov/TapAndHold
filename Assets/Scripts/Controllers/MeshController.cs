using Assets.Scripts.Core.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class MeshController : MonoBehaviour
    {
        public BrickModel Model;
        public Transform Transform;

        [SerializeField]
        private float AnimateSpeed;

        private float _currentSize;
        private float _waveHight;
        private bool _isActive;
        private bool _isIncrease;

        public void ActivateAnimation(float waveHight = 0.3f)
        {
            if (_isActive)
                return;

            _isActive = true;
            _isIncrease = true;

            _currentSize = Transform.localScale.x;
            _waveHight = waveHight;
        }

        public bool TryUpdateScale()
        {
            if (_isActive && Model != null)
                Scaling();

            return _isActive;
        }

        private void Scaling()
        {
            var size = Transform.localScale.x;

            if (_isIncrease)
            {
                if (size >= _currentSize + _waveHight)
                {
                    _isIncrease = false;
                }
                else
                {
                    Transform.localScale += new Vector3(AnimateSpeed, 0, AnimateSpeed) * Time.deltaTime;
                }
            }
            else
            {
                if (size <= Model.Size)
                {
                    _isActive = false;
                    Transform.localScale = new Vector3(Model.Size, Transform.localScale.y, Model.Size);

                    return;
                }
                else
                {
                    Transform.localScale += new Vector3(-AnimateSpeed, 0, -AnimateSpeed) * Time.deltaTime;
                }
            }
        }
    }
}

