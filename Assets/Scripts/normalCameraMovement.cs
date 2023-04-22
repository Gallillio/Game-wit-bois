using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class normalCameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Flip Rotation")]
    [SerializeField] private float flipYRotationTime = 0.5f;
    private Coroutine turnCoroutine;

    private PlayerMovement1 player;
    private bool IsFacingRight;

    private void Start()
    {
        player = playerTransform.gameObject.GetComponent<PlayerMovement1>();
        IsFacingRight = player.IsFacingRight;
    }

    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }

        yield return null;
    }
    private float DetermineEndRotation()
    {
        IsFacingRight = !IsFacingRight;
        if (IsFacingRight)
        {
            return 0;
        }
        else
        {
            return 180f;
        }
    }
}

