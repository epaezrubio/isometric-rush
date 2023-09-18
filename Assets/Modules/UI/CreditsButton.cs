using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsoRush.UI
{
    public class CreditsButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject _credits;

        public void ToggleCredits()
        {
            _credits.SetActive(!_credits.activeSelf);
        }
    }
}
