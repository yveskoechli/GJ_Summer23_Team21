using System;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

using Debug = UnityEngine.Debug;

public class InputObserver : MonoBehaviour
{
    public static event Action<InputControlScheme?> ControlSchemeChanged;
    
    [SerializeField] private InputActionAsset inputAsset;
    
    public InputUser user;
    
    // Testing:
    public string startControlSchemeName = "Keyboard&Mouse";
    
    private void Awake()
    {
        if (inputAsset == null)
        {
            Debug.LogError($"Missing {nameof(inputAsset)}.", this);
            enabled = false;
        }

        // Create a user
        user = InputUser.CreateUserWithoutPairedDevices();
        user.AssociateActionsWithUser(inputAsset);
        
        // Enable listening on unpaired devices.
        ++InputUser.listenForUnpairedDeviceActivity;
        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
        InputUser.onPrefilterUnpairedDeviceActivity += OnPreFilterUnpairedDeviceUsed;
    }

    private bool OnPreFilterUnpairedDeviceUsed(InputDevice device, InputEventPtr _)
    {
        // Early out if the device isn't usable with any of our control schemes.
        return inputAsset.IsUsableWithDevice(device);
    }

    private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr _)
    {
        if (SwitchControlScheme(control.device))
        {
            ControlSchemeChanged?.Invoke(user.controlScheme);
        }
    }

    private bool SwitchControlScheme(InputDevice withDevice)
    {
        using var availableDevices = InputUser.GetUnpairedInputDevices();
        
        if (availableDevices.Count > 1)
        {
            int index = availableDevices.IndexOf(withDevice);
            // Put device that was just actuated first to be preferred for a match
            availableDevices.SwapElements(0, index);
        }
        
        // Add all devices currently paired to allow switching to control schemes that contain those devices.
        availableDevices.AddRange(user.pairedDevices);
        
        if (InputControlScheme.FindControlSchemeForDevices(availableDevices, inputAsset.controlSchemes, out var controlScheme, out var matchResult, withDevice))
        {
            try
            {
                var oldDevices = user.pairedDevices.ToArray(); // Force enumeration
                var newDevices = matchResult.devices; // Is disposed with the matchResult

                // Remove all currently paired devices that are not part of the new controlScheme
                foreach (var oldDevice in oldDevices.Except(newDevices))
                {
                    user.UnpairDevice(oldDevice);
                }

                // Pair all new devices part of the new controlScheme
                foreach (var newDevice in newDevices.Except(oldDevices))
                {
                    user = InputUser.PerformPairingWithDevice(newDevice, user);
                }

                user.ActivateControlScheme(controlScheme);

                Debug.Log($"ControlScheme switched to: {controlScheme.name}\nPairedDevices: {string.Join(',', user.pairedDevices.Select(device => device.displayName))}");
                startControlSchemeName = controlScheme.name;
                PlayerPrefs.SetString("StartControlScheme", startControlSchemeName);
                return true;
            }
            finally
            {
                matchResult.Dispose();
            }
        }
        else
        {
            return false; 
        }
    }

    public void SetStartControlScheme()
    {
        PlayerPrefs.SetString("StartControlScheme", startControlSchemeName);
        Debug.Log("Start Control Scheme ist set to: " + startControlSchemeName);
    }

    public string GetStartControlScheme()
    {
        return PlayerPrefs.GetString("StartControlScheme");
    }
}
