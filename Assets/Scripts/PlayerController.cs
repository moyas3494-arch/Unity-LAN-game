using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement")]
    public float Speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movementInput;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Inicializamos las acciones de input
        inputActions = new PlayerInputActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            enabled = false; // Desactiva este script si no es el propietario del objeto
            return;
        }

        inputActions.Player.Enable();

        inputActions.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Movement.canceled += ctx => movementInput = Vector2.zero;
    }

    private void OnDisable()
    {
        // Es una buena práctica deshabilitar el mapa de inputs al destruir o desactivar el objeto
        if (IsOwner && inputActions != null)
        {
            inputActions.Player.Disable();
        }
    }

    private void FixedUpdate()
    {
        // Doble seguridad: solo el dueño procesa las físicas de movimiento de su avatar
        if (!IsOwner) return;

        // Aplicamos el movimiento al Rigidbody2D
        rb.linearVelocity = movementInput * Speed;
    }
}