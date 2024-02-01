using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementController : BaseBehaviour, IPointerDownHandler ,IPointerUpHandler, IDragHandler
{
    



    [Header("Size Ratio")]
    [SerializeField] private Canvas _canvas = null;

    [Header("UI SETTINGS TEST")]
    [SerializeField] private float _movementRange = 25;

    private MyPlayer _player = null;

    private Vector3 _startPosition = Vector3.zero;
    private Vector2 _velocity = Vector2.zero;

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
            _player.EventMovement.CallVelocityMovement(_velocity, Utilities.GetDirectionFromVector(_velocity));

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
            _player.EventMovement.CallStopMovement();
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
