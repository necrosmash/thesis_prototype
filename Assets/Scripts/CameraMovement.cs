using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    float speed = 0.03f;
    private TMPro.TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GameObject.Find("Canvas/Log/Chat Input Field").GetComponent<TMPro.TMP_InputField>();
    }

    void LateUpdate(){
        
        if (MenuCanvas.IsRendered || inputField.isFocused) return;

        Vector3 velocity = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            velocity.z += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            velocity.z -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x += 1;
        }
        
        // disabled for now because toggling camera angle breaks click raycast detection of grid cells
        /*if (Input.GetKeyDown(KeyCode.C))
        {
            if (gameObject.transform.rotation.eulerAngles.x == 90)
            {
                gameObject.transform.rotation = Quaternion.Euler(45, 0, 0);
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 8);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 8);
            }
        }*/

        gameObject.transform.position += (velocity * speed);
    }
}
