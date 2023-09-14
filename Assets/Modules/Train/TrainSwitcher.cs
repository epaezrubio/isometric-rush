using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsoRush.Train
{
    public class TrainSwitcher : MonoBehaviour
    {
        [SerializeField]
        private GameObject _left;

        [SerializeField]
        private GameObject _right;

        [SerializeField]
        private bool _isLeft;

        void Start()
        {
            UpdateTrains();
        }

        void UpdateTrains()
        {
            _left.SetActive(_isLeft);
            _right.SetActive(!_isLeft);
        }

        public void Toggle()
        {
            _isLeft = !_isLeft;

            UpdateTrains();
        }
    }
}
