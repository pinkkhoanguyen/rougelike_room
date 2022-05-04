using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    Animator animator;
    public static ShakeCamera instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        animator = GetComponent<Animator>();
    }

    public void shake() {
        animator.Play("shakeCam");
    }
}
