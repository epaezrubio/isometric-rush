using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalController : MonoBehaviour
{
    [SerializeField]
    private bool _isLeft;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        UpdateAnimator();
    }

    public void Toggle()
    {
        _isLeft = !_isLeft;

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("isLeft", _isLeft);
    }
}
