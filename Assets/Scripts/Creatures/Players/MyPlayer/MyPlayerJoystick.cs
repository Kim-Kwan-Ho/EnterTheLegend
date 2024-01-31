using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyPlayerJoystick : BaseBehaviour, IPointerDownHandler ,IPointerUpHandler, IDragHandler
{
    private Vector3 _startPosition = Vector3.zero;
    [SerializeField] private float _movementRange = 20;
    private MyPlayer _player = null;
    private Vector2 _velocity = Vector2.zero;
    [SerializeField] private Canvas _canvas = null;

    private void Start()
    {
        _startPosition = transform.position;
        _movementRange *= _canvas.scaleFactor;
        _player = AdventureSceneManager.Instance.GetMyPlayer();
    }


    private void Update()
    {
        if (!CanContinueUsing())
            return;

        if (_velocity != Vector2.zero)
        {
            _player.EventState.CallStateChangedEvent(State.Move);
            _player.EventMovement.CallVelocityMovement(_velocity);
            _player.EventDirection.CallDirectionChanged(GetDirection());
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {}
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.position = _startPosition;
        _velocity = Vector2.zero;

        if (CanContinueUsing())
        {
            _player.EventState.CallStateChangedEvent(State.Idle);
            _player.EventMovement.CallVelocityMovement(Vector2.zero);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        Vector3 pos = (Vector3)eventData.position - _startPosition;
        pos = Vector3.ClampMagnitude(pos, _movementRange);
        transform.position = _startPosition + pos;
        _velocity = new Vector2(pos.x / _movementRange, pos.y / _movementRange);
    }

    private bool CanContinueUsing()
    {
        return AdventureSceneManager.Instance.SceneState != AdventureSceneState.MyPlayerDeath &&
               AdventureSceneManager.Instance.SceneState != AdventureSceneState.ClearFailed;
    }

    private Direction GetDirection()
    {
        float degrees = GetAngle(_velocity);

        if (degrees >= -22.5f && degrees < 22.5f)
            return Direction.Right;
        else if (degrees >= 22.5f && degrees < 67.5f)
            return Direction.UpRight;
        else if (degrees >= 67.5f && degrees < 112.5f)
            return Direction.Up;
        else if (degrees >= 112.5f && degrees < 157.5f)
            return Direction.UpLeft;
        else if ((degrees >= 157.5f && degrees <= 180) || (degrees >= -180 && degrees < -157.5f))
            return Direction.Left;
        else if (degrees >= -157.5f && degrees < -112.5f)
            return Direction.DownLeft;
        else if (degrees >= -112.5f && degrees < -67.5f)
            return Direction.Down;
        else // degrees >= -67.5f && degrees < -22.5f
            return Direction.DownRight;
    }

    private float GetAngle(Vector2 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }


#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _canvas = GetComponentInParent<Canvas>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _canvas);
    }

#endif 
}
