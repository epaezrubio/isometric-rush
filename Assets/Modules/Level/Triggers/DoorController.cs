using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private bool _isOpen;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        UpdateAnimator();
    }

    public void Toggle()
    {
        _isOpen = !_isOpen;

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("isOpen", _isOpen);
    }
}
