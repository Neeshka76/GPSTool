using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GPSTool
{
    public class GPSToolData
    {
        public float PlayerPositionXGetSet { get; set; }
        public float PlayerPositionYGetSet { get; set; }
        public float PlayerPositionZGetSet { get; set; }
        // X of the Player Position the player will teleport to
        public float PlayerTeleportPositionXGetSet { get; set; }
        // Y of the Player Position the player will teleport to
        public float PlayerTeleportPositionYGetSet { get; set; }
        // Z of the Player Position the player will teleport to
        public float PlayerTeleportPositionZGetSet { get; set; }
        // Send the distance the player is from the pointer
        public float PlayerTeleportPositionDistanceGetSet { get; set; }
        // Send the distance max the player can teleport
        public float PlayerTeleportPositionDistanceMaxGetSet { get; set; }
        // Send the value typed with the Keyboard to the PlayerTeleportPositionX/Y/Z
        public float PlayerTeleportPositionValueToAssignedGetSet { get; set; }
        // Set if Player is using the player frame or the world frame
        public bool PlayerFrameGetSet { get; set; }
        // Set if Player has pressed the button Position X Teleport to
        public bool PlayerTeleportPositionXButtonPressedGetSet { get; set; }
        // Set if Player has pressed the button Position Y Teleport to
        public bool PlayerTeleportPositionYButtonPressedGetSet { get; set; }
        // Set if Player has pressed the button Position Z Teleport to
        public bool PlayerTeleportPositionZButtonPressedGetSet { get; set; }
        // Set if Player has pressed the button Distance Max
        public bool PlayerTeleportPositionDistanceMaxButtonPressedGetSet { get; set; }
        // Set if Player has pressed the button Teleport To Spawn
        public bool PlayerTeleportToSpawnButtonPressedGetSet { get; set; }
        // Set if Player has pressed the button Teleport To End Of Dungeon
        public bool PlayerTeleportToEndOfDungeonButtonPressedGetSet { get; set; }
        // Set if Player has pressed the button Teleport To Map
        public bool PlayerTeleportToMapButtonPressedGetSet { get; set; }
        // Set if Keyboard has pressed enter button finish
        public bool KeyboardFinishEnterButtonPressedGetSet { get; set; }
        // Info that the player has confirm the teleportation
        public bool PlayerTeleportPositionConfirmButtonPressedGetSet { get; set; }
        // Toggle the arrow indicator
        public bool ToggleArrowsIndicatorGetSet { get; set; }
        // Toggle the arrow to Exit
        public bool ToggleArrowToExitGetSet { get; set; }
        // Toggle the Pointer to teleport
        public bool PointToTeleportGetSet { get; set; }
        // Set if has to refresh the position
        public bool RefreshPositionGetSet { get; set; }
        // Set if Level is home
        public bool LevelIsHomeGetSet { get; set; }
        // Set if Level is dungeon
        public bool LevelIsDungeonGetSet { get; set; }
    }

    public class GPSToolController : MonoBehaviour
    {
        public GPSToolData data = new GPSToolData();
    }
}
