using Assets.Scripts.Core;
using Assets.Scripts.Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        public UserInput Ui;
        public GameSettings Settings;
        public MeshesManager Manager;
        public GameObject Prefab;

        [SerializeField, Range(0.25f, 0.5f)]
        private float _yOffset = 0.25f;

        private float _count = -1;
        private BricksTower _tower;
        private MeshController _currentMesh;

        public void RestartScene()
        {
            SceneManager.LoadScene(0);
        }

        private void Start()
        {
            _tower = new BricksTower(1);
            InitManager();
            AddEventListeners();

            InitFirstMesh();
        }

        private void OnDestroy()
        {
            RemoveEventListeners();
        }

        private void InitManager()
        {
            Manager.Init(Settings);
        }

        private void InitFirstMesh()
        {
            InitMesh();
            _currentMesh.Transform.localScale = new Vector3(_tower.LastBrickSize, _currentMesh.Transform.localScale.y, _tower.LastBrickSize);
            ConsolidateMesh();
        }

        private MeshController InstantiateMesh()
        {
            var go = Instantiate(Prefab, new Vector3(0, ++_count * _yOffset, 0), Quaternion.identity, Manager.transform);
            var meshController = go.GetComponent<MeshController>();

            meshController.Transform = go.transform;

            return meshController;
        }

        private void InitMesh()
        {
            _currentMesh = InstantiateMesh();

            Manager.StartBuild(_tower.LastBrickSize * 1.1f, _currentMesh);
        }

        private void ConsolidateMesh()
        {
            Manager.StopIncrease();
            var size = _currentMesh.Transform.localScale.x;

            var model = _tower.BuildNewBrick(size);
            if (model == null)
            {
                EventManager.IncorrectMesh(_currentMesh.Transform);
                return;
            }

            _currentMesh.Model = model;

            if (_tower.IsLastBrickPerfetcMove())
            {
                EventManager.PerfectMove(_currentMesh.Transform);
                Manager.Wave();
            }

            EventManager.BuildedNewMeshEvent(_currentMesh.Transform);

            _currentMesh = null;
        }

        #region Handlers

        private void AddEventListeners()
        {
            Ui.TapUiEvent += OnTapUi;
            Ui.UpTouchUiEvent += UpTouchUi;

            EventManager.IncorrectMeshEvent += OnIncorrectMesh;
        }

        private void RemoveEventListeners()
        {
            Ui.TapUiEvent -= OnTapUi;
            Ui.UpTouchUiEvent -= UpTouchUi;

            EventManager.IncorrectMeshEvent -= OnIncorrectMesh;
        }

        private void OnTapUi()
        {
            if (!_currentMesh)
                InitMesh();
            else
                RestartScene();
        }

        private void UpTouchUi()
        {
            if (_currentMesh)
                ConsolidateMesh();
        }

        private void OnIncorrectMesh(Transform sender)
        {
            Ui.UpTouchUiEvent -= UpTouchUi;
        }

        #endregion Handlers
    }
}
