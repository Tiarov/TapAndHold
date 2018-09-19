using System;
using UnityEngine;

namespace Assets.Scripts.Helper
{
    public class EventManager
    {
        public static Action<Transform> BuildedNewMeshEvent;
        public static Action<Transform> PerfectMoveEvent;
        public static Action<Transform> IncorrectMeshEvent;
        public static Action<Transform> DestroyMeshEvent;
        public static Action GameOverEvent;

        public static void BuildedNewMesh(Transform sender)
        {
            if (BuildedNewMeshEvent != null)
                BuildedNewMeshEvent(sender);
        }

        public static void PerfectMove(Transform sender)
        {
            if (PerfectMoveEvent != null)
                PerfectMoveEvent(sender);
        }

        public static void IncorrectMesh(Transform sender)
        {
            if (IncorrectMeshEvent != null)
                IncorrectMeshEvent(sender);
        }

        public static void DestroyMesh(Transform sender)
        {
            if (DestroyMeshEvent != null)
                DestroyMeshEvent(sender);
        }

        public static void GameOver()
        {
            if (GameOverEvent != null)
                GameOverEvent();
        }
    }
}
