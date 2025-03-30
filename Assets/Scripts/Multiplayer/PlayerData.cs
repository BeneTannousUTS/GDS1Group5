using UnityEngine.InputSystem;

    [System.Serializable]
    public class PlayerData
    {
        public int playerIndex;
        public Gamepad gamepad;
        public bool isJoined;
        public PlayerInput playerInput;
    }