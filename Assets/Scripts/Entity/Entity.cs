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

    public abstract void HandleStateBehaviour(int _stateBehaviourId);

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

