using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


public class InputManager : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool sprint;
	public bool crouch;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;
	private PlayerControls inputActions;

#if ENABLE_INPUT_SYSTEM
    private void OnEnable()
    {
		if (inputActions == null)
		{
			inputActions = new PlayerControls();
			inputActions.Player.Move.performed += OnMove;
            inputActions.Player.Jump.performed += OnJump;
            inputActions.Player.Look.performed += OnLook;
			inputActions.Player.Move.canceled += OnMoveStopped;
            inputActions.Player.Look.canceled += OnLookStopped;
            inputActions.Player.Sprint.performed += OnSprint;
			inputActions.Player.Crouch.performed += OnCrouch;
			inputActions.Player.Sprint.canceled += OnSprintStopped;
			inputActions.Player.Crouch.canceled += OnCrouchStopped;

        }
        inputActions.Enable();

    }

    private void OnDisable()
	{
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Move.canceled -= OnMoveStopped;
        inputActions.Player.Look.canceled -= OnLookStopped;
        inputActions.Player.Crouch.performed -= OnCrouch;
        inputActions.Player.Sprint.canceled -= OnSprintStopped;
        inputActions.Player.Crouch.canceled -= OnCrouchStopped;
        inputActions.Disable();

    }


    public void OnMove(InputAction.CallbackContext context)
	{
		MoveInput(context.ReadValue<Vector2>());
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		if(cursorInputForLook)
		{
			LookInput(context.ReadValue<Vector2>());
		}
	}
	
	public void OnMoveStopped(InputAction.CallbackContext context)
	{
		MoveInput(Vector2.zero);
	}

	public void OnLookStopped(InputAction.CallbackContext context)
	{
		if(cursorInputForLook)
		{
			LookInput(Vector2.zero);
		}
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		JumpInput(context.performed);
	}
		
	public void OnSprint(InputAction.CallbackContext context)
	{
		SprintInput(context.performed);
	}
    public void OnCrouch(InputAction.CallbackContext context)
    {
        CrouchInput(context.performed);
    }
    public void OnSprintStopped(InputAction.CallbackContext context)
    {
        SprintInput(false);
    }
    public void OnCrouchStopped(InputAction.CallbackContext context)
    {
        CrouchInput(false);
    }
#endif


    public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	} 

	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}

	public void JumpInput(bool newJumpState)
	{
		jump = newJumpState;
	}
		
	public void SprintInput(bool newSprintState)
	{
		sprint = newSprintState;
	}

    public void CrouchInput(bool newCrouchState)
    {
		crouch = newCrouchState;
    }

    private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
	
