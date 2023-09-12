using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace IsoRush.State
{
    public class GameState : ITickable
    {
        public ReactiveProperty<float> GameTime = new ReactiveProperty<float>(0f);

        public ReactiveProperty<float> GameSpeed = new ReactiveProperty<float>(1.2f);

        public ReactiveProperty<GameDifficulty> GameDifficulty = new ReactiveProperty<GameDifficulty>(State.GameDifficulty.Normal);

        public ReactiveProperty<float> ScrollSpeed = new ReactiveProperty<float>(10f);

        public ReactiveProperty<float> CameraSize = new ReactiveProperty<float>(10f);

        public ReactiveProperty<float> CameraSizeTarget = new ReactiveProperty<float>(10f);

        public ReactiveProperty<Vector3> CameraPosition = new ReactiveProperty<Vector3>(
            Vector3.zero
        );

        public ReactiveProperty<Vector3> CameraPositionTarget = new ReactiveProperty<Vector3>(
            Vector3.zero
        );

        public ReactiveProperty<int> InventoryCheckpoints = new ReactiveProperty<int>(5);

        public ReactiveProperty<Vector3> CheckpointPosition = new ReactiveProperty<Vector3>();

        public ReactiveProperty<float> CheckpointGameTime = new ReactiveProperty<float>();

        public ReactiveProperty<int> CheckpointsCount = new ReactiveProperty<int>();

        public ReactiveDictionary<string, int> Triggers = new ReactiveDictionary<string, int>();

        public void Tick()
        {
            CameraSize.Value = Mathf.Lerp(CameraSize.Value, CameraSizeTarget.Value, Time.deltaTime * 3f);

            CameraPosition.Value = Vector3.Lerp(
                CameraPosition.Value,
                CameraPositionTarget.Value,
                Time.deltaTime * 3f
            );
        }
    }
}
