using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EntityState
{
    Nothing,
    Idle = 1,
    Walk,
    Run,
    Shoot
}

public abstract class EntityModel : MonoBehaviour
{
    public Entity Entity;
}

public abstract class Entity : MonoBehaviour
{
    [Header("Entity")]
    public Rigidbody Rb;
    public EntityState State;
    public Vector3 MoveDirection;
    public float SpeedLerp = 30f;
    public float RotateLerp = 30f;

    // STATE MACHINE BEHAVIOUR
    //public abstract void HandleStateBehaviour(int _stateBehaviourId);
    public void HandleStateBehaviour(int _stateBehaviourId)
    {
        if (onStateBehaviours.ContainsKey(_stateBehaviourId))
        {
            onStateBehaviours[_stateBehaviourId](this);
        }
    }

    // Enter = EntityState * 10
    // Update = EntityState * 10 + 1
    // Exit = EntityState * 10 + 2
    private delegate void OnStateBehaviour(Entity _character);
    private static Dictionary<int, OnStateBehaviour> onStateBehaviours =
        new Dictionary<int, OnStateBehaviour>()
        {
            { (int) EntityState.Idle * 10, IdleEnter },
            { (int) EntityState.Idle * 10 + 1, IdleUpdate },
            { (int) EntityState.Idle * 10 + 2, IdleExit },
            { (int) EntityState.Walk * 10, WalkEnter },
            { (int) EntityState.Walk * 10 + 1, WalkUpdate },
            { (int) EntityState.Walk * 10 + 2, WalkExit },
            { (int) EntityState.Run * 10, RunEnter },
            { (int) EntityState.Run * 10 + 1, RunUpdate },
            { (int) EntityState.Run * 10 + 2, RunExit}
        };

    #region IDLE
    private static void IdleEnter(Entity _entity)
    {
        _entity.IdleEnter();
    }
    protected virtual void IdleEnter() { }

    private static void IdleUpdate(Entity _entity)
    {
        _entity.IdleUpdate();
    }
    protected virtual void IdleUpdate() { }

    private static void IdleExit(Entity _entity)
    {
        _entity.IdleExit();
    }
    protected virtual void IdleExit() { }
    #endregion

    #region WALK
    private static void WalkEnter(Entity _entity)
    {
        _entity.WalkEnter();
    }
    protected virtual void WalkEnter() { }

    private static void WalkUpdate(Entity _entity)
    {
        _entity.WalkUpdate();
    }
    protected virtual void WalkUpdate() { }
   
    private static void WalkExit(Entity _entity)
    {
        _entity.WalkExit();
    }
    protected virtual void WalkExit() { }
    #endregion

    #region RUN
    private static void RunEnter(Entity _entity)
    {
        _entity.RunEnter();
    }
    protected virtual void RunEnter() { }

    private static void RunUpdate(Entity _entity)
    {
        _entity.RunUpdate();
    }
    protected virtual void RunUpdate() { }

    private static void RunExit(Entity _entity)
    {
        _entity.RunExit();
    }
    protected virtual void RunExit() { }
    #endregion

    public void VelocityLerp(float _speed)
    {
        Vector3 _velocity = new Vector3(MoveDirection.x * _speed, Rb.velocity.y, MoveDirection.z * _speed);

        Rb.velocity = Vector3.Lerp(Rb.velocity, _velocity, SpeedLerp * Time.deltaTime);
    }
    
    public void RotationLerp(Vector3 _direction, float _rotateLerp)
    {
        _direction.y = 0f;

        if (_direction.sqrMagnitude > 0.01f)
        {
            Quaternion _rotation = Quaternion.LookRotation(_direction);

            transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, _rotateLerp * Time.deltaTime);
        }
    }

    public void RotationLerp(Vector3 _direction)
    {
        RotationLerp (_direction, RotateLerp);
    }

    /// <summary>Random position inside of circle depends on navMesh.<summary
    /// <param name="_radius">Radius of the circle.</param>
    public Vector3 RandomPositionNearMe(float _radius)
    {
        Vector2 _v2Direction = Random.insideUnitCircle;
        Vector3 _direction = new Vector3(_v2Direction.x, 0f, _v2Direction.y);

        Vector3 _randomPosition = transform.position + _direction * _radius;

        NavMeshHit hit;
        NavMesh.SamplePosition(_randomPosition, out hit, _radius, 1);

        return hit.position;
    }
}

