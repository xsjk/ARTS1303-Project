using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum KeyBinding
{
    PrimaryInteraction,
    SecondaryInteraction,
}

[RequireComponent(typeof(PlayerInput))]
public sealed class KeyTextureManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private string _currentControlScheme;
    private readonly Dictionary<string, Dictionary<KeyBinding, Texture>> _keyTextures = new();
    public static KeyTextureManager Instance { get; private set; }

    [SerializeField] private Texture keyboardE;
    [SerializeField] private Texture keyboardR;
    [SerializeField] private Texture gamepadNorth;
    [SerializeField] private Texture gamepadEast;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        _playerInput = GetComponent<PlayerInput>();
        _keyTextures.Add("Keyboard", new Dictionary<KeyBinding, Texture>
        {
            { KeyBinding.PrimaryInteraction, keyboardE },
            { KeyBinding.SecondaryInteraction, keyboardR }
        });
        _keyTextures.Add("Gamepad", new Dictionary<KeyBinding, Texture>
        {
            { KeyBinding.PrimaryInteraction, gamepadNorth },
            { KeyBinding.SecondaryInteraction, gamepadEast }
        });
        UpdateActiveTexture();
    }

    private void UpdateActiveTexture()
    {
        _currentControlScheme = _playerInput.currentControlScheme ?? "Keyboard";
        ActiveTexture = _keyTextures[_currentControlScheme];
        if (ActiveTexture == null)
            Debug.LogError("ActiveTexture is null");
    }

    private void Update()
    {
        UpdateActiveTexture();
    }


    public Dictionary<KeyBinding, Texture> ActiveTexture { get; private set; }
}