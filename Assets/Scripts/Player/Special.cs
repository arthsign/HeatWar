using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Special : MonoBehaviour
{
    public GameObject specialAttackPrefab; // Prefab da bola de ataque especial
    public Transform firePoint; // Ponto de disparo da bola
    public float maxChargeTime = 1f; // Tempo máximo de carga
    public float maxSize = 2f; // Tamanho máximo da bola ao ser carregada

    private float currentChargeTime;
    private bool isCharging;
    private GameObject currentAttack;

    private void Update()
    {
        if (isCharging)
        {
            currentChargeTime += Time.deltaTime;
            float size = Mathf.Lerp(1f, maxSize, currentChargeTime / maxChargeTime);
            specialAttackPrefab.transform.localScale = new Vector3(size, size, size);

            if (currentChargeTime >= maxChargeTime)
            {
                isCharging = false;
                Fire();
            }
        }
    }

    public void OnChargeAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCharging();
        }
        else if (context.canceled)
        {
            Fire();
        }
    }

    private void StartCharging()
    {
        specialAttackPrefab.SetActive(true);
        specialAttackPrefab = specialAttackPrefab;
        currentChargeTime = 0f;
        isCharging = true;
    }

    private void Fire()
    {
        if (specialAttackPrefab == null)
        {
            return;
        }

        isCharging = false;
        Rigidbody2D rb = specialAttackPrefab.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * 10f;
        }

        // Inicia a corrotina para desativar o prefab após 3 segundos
        StartCoroutine(ResetSpecialAttack());
    }

    private IEnumerator ResetSpecialAttack()
    {
        yield return new WaitForSeconds(3f);

        // Reseta a posição e desativa o prefab
        specialAttackPrefab.transform.position = firePoint.position;
        specialAttackPrefab.SetActive(false);
        
    }
}
