using UnityEngine.InputSystem;

    [System.Serializable]
    public class PlayerData
    {
        public int playerIndex = -1;
        public Gamepad gamepad;
        public bool isJoined;
        public PlayerInput playerInput;
    }