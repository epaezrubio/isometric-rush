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

        public ReactiveProperty<float> GameSpeed = new ReactiveProperty<float>(1f);

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

        public ReactiveCollection<float> Checkpoints = new ReactiveCollection<float>();

        public ReactiveDictionary<string, int> Triggers = new ReactiveDictionary<string, int>();

        public void Tick()
        {
            CameraSize.Value = Mathf.Lerp(CameraSize.Value, CameraSizeTarget.Value, Time.deltaTime);

            CameraPosition.Value = Vector3.Lerp(
                CameraPosition.Value,
                CameraPositionTarget.Value,
                Time.deltaTime
            );
        }
    }
}
