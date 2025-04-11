using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

    [Serializable]
    public class PlayerData
    {
        public int playerIndex = -1;
        [Serialize]
        public Gamepad gamepad;
        public bool isJoined;
        public PlayerInput playerInput;
    }