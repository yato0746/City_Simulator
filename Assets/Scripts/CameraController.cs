using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject cameraParent;
    [SerializeField] float _speed = 0.066f;

    bool touched = false;
    Vector2 oldPosition;

    public void Event_PointerDown()
    {
        if (!touched)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                touched = true;

                if (Input.touchCount > 0)
                {
                    Touch _touch = Input.GetTouch(0);
                    oldPosition = _touch.position;
                    Debug.Log(oldPosition);
                }
            }
        }
    }

    public void Event_Drag()
    {
        if (touched)
        {
            if (Input.touchCount > 0)
            {
                Touch _touch = Input.GetTouch(0);

                Vector3 _touchOffset = _touch.position - oldPosition;

                float _x = _touchOffset.x;
                float _z = _touchOffset.y;

                Vector3 _move = cameraParent.transform.right * _x + cameraParent.transform.forward * _z;

                Vector3 _position = cameraParent.transform.position;
                _position += _move * _speed;
                cameraParent.transform.position = _position;

                oldPosition = _touch.position;
            }
        }
    }

    public void Event_PointerExit()
    {
        touched = false;
    }

    // Example code with

    //int _lastFingerIndex = -1;
    //Touch _lastTouch;

    //void TouchMovement()
    //{
    //    float _screenMid = Screen.width / 2;
    //    for (int i = 0; i < Input.touchCount; i++)
    //    {
    //        if (Input.touches[i].phase == TouchPhase.Began)
    //        {
    //            _lastFingerIndex = Input.touches[i].fingerId;
    //        }
    //        if (Input.touches[i].phase == TouchPhase.Ended)
    //        {
    //            _lastTouch = Input.touches[_lastFingerIndex - 1];
    //        }
    //        else
    //        {
    //            _lastTouch = Input.touches[_lastFingerIndex];
    //        }
    //    }

    //    if (_lastTouch.position.x < _screenMid)
    //    {
    //        // Moveleft
    //    }
    //    else
    //    {
    //        // Moveright
    //    }

    //    if (Input.touchCount == 0)
    //    {
    //        // Zero velocity
    //    }
    //}
}
