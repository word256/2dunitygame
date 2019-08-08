using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControllerStats : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI groundedText;

    private void Update()
    {
        if (playerController.isGrounded())
            groundedText.text = "Grounded: True";
        else
            groundedText.text = "Grounded: False";
    }
}
