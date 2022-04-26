using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimation : MonoBehaviour
{
    [SerializeField] private Animator animtor;
    private void Awake()
    {
        if(animtor == null)
        animtor = GetComponent<Animator>();
    }

    public void play(string name) {
        animtor.Play(name);
    }
}
