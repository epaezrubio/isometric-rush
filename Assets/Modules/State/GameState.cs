using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace IsoRush.State
{
    public class GameState
    {
        public ReactiveProperty<float> GameTime = new ReactiveProperty<float>(0f);

        public ReactiveProperty<float> CameraSize = new ReactiveProperty<float>(5f);

        public ReactiveProperty<int> ScrollSpeed = new ReactiveProperty<int>(10);

        public ReactiveProperty<int> InventoryCheckpoints = new ReactiveProperty<int>(5);

        public ReactiveCollection<int> Checkpoints = new ReactiveCollection<int>();

        public ReactiveDictionary<string, int> Triggers = new ReactiveDictionary<string, int>();
    }
}
