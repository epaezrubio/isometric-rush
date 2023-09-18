using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace IsoRush.State
{
    public class GameState : ITickable, IFixedTickable
    {
        public ReactiveProperty<float> GameTime = new ReactiveProperty<float>(0f);

        public ReactiveProperty<float> GameSpeed = new ReactiveProperty<float>(1f);

        public ReactiveProperty<GameDifficulty> GameDifficulty =
            new ReactiveProperty<GameDifficulty>(State.GameDifficulty.Normal);

        public ReactiveProperty<float> ScrollSpeed = new ReactiveProperty<float>(15f);

        public ReactiveProperty<float> CameraSize = new ReactiveProperty<float>(10f);

        public ReactiveProperty<float> CameraSizeTarget = new ReactiveProperty<float>(10f);

        public ReactiveProperty<bool> CameraCinematic = new ReactiveProperty<bool>(false);

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

        public void FixedTick()
        {
            GameTime.Value += GameSpeed.Value * Time.fixedDeltaTime;
        }

        public void Tick()
        {
            if (CameraCinematic.Value)
            {
                CameraSize.Value = Mathf.Lerp(CameraSize.Value, 20f, Time.deltaTime * 0.8f);

                CameraPosition.Value = CameraPosition.Value + new Vector3(0, 0, 9 * Time.deltaTime);
            }
            else
            {
                CameraSize.Value = Mathf.Lerp(
                    CameraSize.Value,
                    CameraSizeTarget.Value,
                    Time.deltaTime * 3f
                );

                CameraPosition.Value = Vector3.Lerp(
                    CameraPosition.Value,
                    CameraPositionTarget.Value,
                    Time.deltaTime * 3f
                );
            }
        }
    }
}
