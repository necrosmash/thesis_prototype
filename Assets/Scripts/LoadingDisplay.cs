using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingDisplay : MonoBehaviour
{
    [SerializeField]
    private int ROTATION_SPEED = 500;
    private const int STEP_DEGREES = 45;

    [SerializeField]
    private Image spinner, shadow;

    private float accAngle = 0;
    private int step = 0;
    private Vector3 zAxis = new Vector3(0, 0, 1);

    void Update()
    {
        ToggleEnabled();
        if (!spinner.gameObject.activeSelf) return;

        accAngle += Time.deltaTime * ROTATION_SPEED;

        if (step != (int) (accAngle / 45))
        {
            step++;
            spinner.transform.RotateAround(spinner.transform.position, zAxis, STEP_DEGREES);
        }
    }

    private void ToggleEnabled()
    {
        if (OpenAiApi.isPostInProgress == spinner.gameObject.activeSelf) return;

        spinner.gameObject.SetActive(OpenAiApi.isPostInProgress);
        shadow.gameObject.SetActive(OpenAiApi.isPostInProgress);

        if (!OpenAiApi.isPostInProgress)
        {
            step = 0;
            accAngle = 0;
        }
    }
}
