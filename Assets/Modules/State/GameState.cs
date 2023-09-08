using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace IsoRush.State
{
    public class GameState
    {
        public ReactiveProperty<int> gameTime = new ReactiveProperty<int>(0);
    }
}
