using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private TapHandlerJoystick _tapJoystick;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private Transform _bombContainer;

    public Vector2 InputVector { get; private set; }

    public float HorizontalDirection { get; private set; }
    public float VerticalDirection { get; private set; }

    private void Update()
    {
        HorizontalDirection = _joystick.Horizontal;
        VerticalDirection = _joystick.Vertical;

        InputVector = new Vector2(HorizontalDirection, VerticalDirection);

        if (_tapJoystick.Pressed)
        {
            DropBomb();
        }
    }

    private void DropBomb()
    {
        if (_bombPrefab != null)
        {
            Vector3 bombPosition = new Vector3(
                Mathf.RoundToInt(transform.position.x),
                _bombPrefab.transform.position.y,
                Mathf.RoundToInt(transform.position.z));

            foreach (Transform bomb in _bombContainer)
            {
                if (bomb.position == bombPosition) { return; }
            }

            Instantiate(_bombPrefab, bombPosition, _bombPrefab.transform.rotation, _bombContainer);
        }
    }
}
