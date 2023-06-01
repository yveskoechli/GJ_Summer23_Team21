using System;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class BindingSprite : MonoBehaviour
{
    [SerializeField] private Sprite keyboardSprite;
    [SerializeField] private Sprite controllerSprite;

    private Image image;

    private InputObserver inputObserver;

    private void OnValidate()
    {
        image = GetComponent<Image>();
    }

    private void Awake()
    {
        OnValidate();
        inputObserver = FindObjectOfType<InputObserver>();
    }

    private void OnEnable()
    {
        InputUser.onChange += OnChange;
        ReadInput();
    }

    private void Start()
    {
        //ReadInput();
    }

    private void OnDisable()
    {
        InputUser.onChange -= OnChange;
    }
    
    private void OnChange(InputUser user, InputUserChange userChange, InputDevice device)
    {
        if (userChange != InputUserChange.ControlSchemeChanged) { return; }
        
        foreach (InputDevice userPairedDevice in user.pairedDevices)
        {
            Debug.Log(userPairedDevice.name);
        }

        switch (user.controlScheme.Value.name)
        {
            case "Gamepad":
                image.sprite = controllerSprite;
                break;
            case "Keyboard&Mouse":
                image.sprite = keyboardSprite;
                break;
            default:
                Debug.LogWarning($"Unknown control scheme: {user.controlScheme.Value.name}");
                return;
        }
    }
    
    private void ReadInput()
    {

        //switch (inputObserver.startControlSchemeName)
        string startControlScheme = PlayerPrefs.GetString("StartControlScheme");
        switch (startControlScheme)
        {
            case "Gamepad":
                image.sprite = controllerSprite;
                break;
            case "Keyboard&Mouse":
                image.sprite = keyboardSprite;
                break;
            default:
                Debug.LogWarning($"Unknown control scheme: {startControlScheme}");
                return;
        }
    }
    
}
