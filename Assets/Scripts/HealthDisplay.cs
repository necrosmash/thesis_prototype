using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class HealthDisplay : MonoBehaviour
{
    private Image healthBarDisplay;
    private Camera mainCamera;

    [SerializeField]
    private GamePiece gp;

    private float enemyXOffset, enemyYOffsetBelow, enemyYOffsetAbove;

    // Start is called before the first frame update
    void Start()
    {
        healthBarDisplay = this.transform.Find("HealthBarInner").GetComponent<Image>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        Rect healthBarOuterRect = this.transform.Find("HealthBarOuter").GetComponent<RectTransform>().rect;
        float healthBarWidth = healthBarOuterRect.width;
        float healthBarHeight = healthBarOuterRect.height;
        enemyXOffset = healthBarWidth * 0.5f;
        enemyYOffsetBelow = healthBarHeight * 2f;
        enemyYOffsetAbove = (healthBarHeight * 0.5f) * -1;

        this.transform.SetParent(GameObject.Find("Canvas").transform, false);
        this.transform.SetAsFirstSibling();

        if (gp is PlayerController)
        {
            this.transform.position = mainCamera.WorldToScreenPoint(new Vector3(11.39f, 0, 9));
        }
    }

    private void Update()
    {
        if (gp.maxHealth == -1) return;

        if (gp is EnemyController)
        {
            this.transform.position = mainCamera.WorldToScreenPoint(gp.transform.position);
            this.transform.position -= new Vector3(
                enemyXOffset,
                gp.currentTile.y == 0 ? enemyYOffsetAbove : enemyYOffsetBelow,
                0
            );
        }

        healthBarDisplay.fillAmount = Mathf.Clamp((float) gp.health / (float) gp.maxHealth, 0, 1);
    }
}