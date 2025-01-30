using UnityEngine;
using UnityEngine.UI; // Para UI
using TMPro; // Si usas TextMeshPro

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Física")]
    public float gravity = 9.81f;
    private Vector3 velocity;

    private CharacterController characterController;
    private Vector3 movementDirection;

    [Header("UI")]
    public GameObject letreroGanaste; // Referencia al letrero de "Ganaste"

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Asegurar que el letrero esté oculto al inicio
        if (letreroGanaste != null)
        {
            letreroGanaste.SetActive(false);
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movementDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (movementDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (characterController.isGrounded)
        {
            velocity.y = -0.1f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Vector3 finalMove = (movementDirection * moveSpeed + velocity) * Time.fixedDeltaTime;
        characterController.Move(finalMove);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inicio"))
        {
            Debug.Log("¡El juego ha comenzado!");
        }
        else if (other.CompareTag("Meta"))
        {
            Debug.Log("¡Ganaste!");

            // Mostrar el letrero de victoria
            if (letreroGanaste != null)
            {
                letreroGanaste.SetActive(true);
            }
        }
    }
}
