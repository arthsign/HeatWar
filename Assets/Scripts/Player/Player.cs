using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float moveSpeed = 5f;
    private Vector2 movimento;
    private Camera mainCamera;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private bool isShooting;
    private float laserChargeTime = 0.3f; // Tempo em segundos para carregar o laser
    private float currentChargeTime;

    [SerializeField] private GameObject laser;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Slider laserChargeSlider; // Referência para a barra de progresso

    public float rotationSpeed = 15f;
    public static Player _player;
    public Vector2 moveDirection;
    private void Start()
    {
        _player = this;
        rb = GetComponent<Rigidbody2D>();
        currentChargeTime = laserChargeTime;
        UpdateChargeSlider();
    }
    private void Awake()
    {
        
        mainCamera = Camera.main;
    }


    public void SetMoviment(InputAction.CallbackContext value)
    {
        movimento = value.ReadValue<Vector2>();
    }
   
    public void SetShoot(InputAction.CallbackContext value)
    {
        if (value.started && !isShooting)
        {
            isShooting = true;
            StartCoroutine(Laser());
        }

    }



    private void Update()
    {
        moveDirection = movimento.normalized;
        // Calcula o vetor de movimento
        Vector2 move = movimento * moveSpeed * Time.deltaTime;
        rb.AddForce(moveDirection * moveSpeed);
        // Move o jogador
        transform.Translate(move, Space.World);

        // Rotacionar a sprite
        if (moveDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
        if (move == Vector2.zero)
        {
            // Aplicar a velocidade constante
            transform.position += (Vector3)rb.velocity * Time.deltaTime;
        }
        
        // Atualiza o tempo de carga do laser e a barra de progresso
        if (isShooting)
        {
            currentChargeTime -= Time.deltaTime;
            UpdateChargeSlider();

            if (currentChargeTime <= 0f)
            {

                Shoot(false); // Desativa o laser quando o tempo de carga termina
            }
        }
        else
        {
            currentChargeTime += Time.deltaTime * 0.5f;
            UpdateChargeSlider();
        }
    }
    private void Shoot(bool estado)
    {
        if(estado)
            laser.SetActive(true);
        else
            laser.SetActive(false);
        
    }
    private IEnumerator Laser()
    {
        if(currentChargeTime >= laserChargeTime)
            currentChargeTime = laserChargeTime;

        UpdateChargeSlider();

        laser.SetActive(true);

        while (currentChargeTime > 0f)
        {
            yield return null;
        }

        isShooting = false;
        laser.SetActive(false);

    }
    private void UpdateChargeSlider()
    {
        if (laserChargeSlider != null)
        {
            laserChargeSlider.value = currentChargeTime / laserChargeTime;
        }
    }
}
