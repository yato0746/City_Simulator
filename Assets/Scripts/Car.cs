using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private const float animTime = 10f;

    [SerializeField] Animator animator;

    private void Start()
    {
        StartCoroutine(ChangeState(Random.Range(2f, 10f)));
    }

    private IEnumerator ChangeState(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);

        StartCoroutine(ChangeState(10f + Random.Range(2f, 10f)));

        if (Random.Range(0, 2) == 0)
        {
            animator.SetTrigger("Left_Right");
        }
        else
        {
            animator.SetTrigger("Right_Left");
        }
    }
}
