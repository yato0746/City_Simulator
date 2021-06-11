using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : Entity
{
    [Header("Movement")]
    [SerializeField] NavMeshAgent agent;
    Transform currentDestination;
    [SerializeField] List<Transform> destinations;
    [SerializeField] List<GameObject> graphicObjects;

    [Header("GameEffect")]
    [SerializeField] AudioSource audioSource;

    [Header("Animation")]
    [SerializeField] Animator animator;

    private void Update()
    {
        animator.SetBool("IsRaining", GameController.Instance.IsRaining);
    }

    #region IDLE
    [Header("Idle state")]
    [SerializeField] float idleTime = 3f;
    [SerializeField] float idleTimeRange = 1f;
    float idleCountdown;

    protected override void IdleEnter()
    {
        idleCountdown = idleTime + Random.Range(-idleTimeRange, idleTimeRange);

        foreach (GameObject _object in graphicObjects)
        {
            _object.SetActive(false);
        }

        //Debug.Log("Idle Enter at " + Time.frameCount);
    }

    protected override void IdleUpdate()
    {
        idleCountdown -= Time.deltaTime;
        if (idleCountdown <= 0f)
        {
            animator.SetBool("IsHome", false);
            //Debug.Log("Idle Update GetOut at " + Time.frameCount);
        }
    }

    protected override void IdleExit()
    {
        foreach (GameObject _object in graphicObjects)
        {
            _object.SetActive(true);
        }
    }
    #endregion

    #region WALK
    [Header("Walk state")]
    [SerializeField] float walkSpeed = 1f;

    protected override void WalkEnter()
    {
        int random = Random.Range(0, destinations.Count);
        currentDestination = destinations[random];
        agent.speed = walkSpeed;
        agent.SetDestination(currentDestination.position);

        //Debug.Log("Walk Enter at " + Time.frameCount);
    }

    protected override void WalkUpdate()
    {
        float _distance = (currentDestination.position - transform.position).magnitude;

        if (_distance < 0.15f)
        {
            animator.SetBool("IsHome", true);
            //Debug.Log("Walk Update GetOut at " + Time.frameCount);
        }
    }
    #endregion

    #region RUN
    [Header("Run state")]
    [SerializeField] float runSpeed = 2f;

    protected override void RunEnter()
    {
        agent.speed = runSpeed;
    }

    protected override void RunUpdate()
    {
        float _distance = (currentDestination.position - transform.position).magnitude;

        if (_distance < 0.15f)
        {
            animator.SetBool("IsHome", true);
        }
    }
    #endregion
}
