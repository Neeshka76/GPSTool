using ThunderRoad;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GPSTool
{
    public class GPSToolMenuModule : MenuModule
    {
        private Text txtPlayerPositionX;
        private Text txtPlayerPositionY;
        private Text txtPlayerPositionZ;
        private Button btnArrowsIndicator;
        private Button btnArrowToExit;
        private Button btnTeleportPositionX;
        private Button btnTeleportPositionY;
        private Button btnTeleportPositionZ;
        private Text txtTeleportPositionX;
        private Text txtTeleportPositionY;
        private Text txtTeleportPositionZ;
        private Text txtTeleportPositionDistance;
        private Button btnConfirmTeleport;
        private Button btnPlayerFrame;
        private Button btnPointToTeleport;
        private Button btnTeleportPositionDistanceMax;
        private Button btnTeleportToSpawn;
        private Button btnTeleportToEndOfDungeon;
        public GPSToolController gPSToolController;
        public GPSToolHook gPSToolHook;
        public override void Init(MenuData menuData, Menu menu)
        {
            base.Init(menuData, menu);

            // Grab the value from Unity
            txtPlayerPositionX = menu.GetCustomReference("txt_PlayerPositionX").GetComponent<Text>();
            txtPlayerPositionY = menu.GetCustomReference("txt_PlayerPositionY").GetComponent<Text>();
            txtPlayerPositionZ = menu.GetCustomReference("txt_PlayerPositionZ").GetComponent<Text>();
            btnArrowsIndicator = menu.GetCustomReference("btn_ArrowsIndicator").GetComponent<Button>();
            btnArrowToExit = menu.GetCustomReference("btn_ArrowToExit").GetComponent<Button>();
            btnTeleportPositionX = menu.GetCustomReference("btn_TeleportPositionX").GetComponent<Button>();
            btnTeleportPositionY = menu.GetCustomReference("btn_TeleportPositionY").GetComponent<Button>();
            btnTeleportPositionZ = menu.GetCustomReference("btn_TeleportPositionZ").GetComponent<Button>();
            txtTeleportPositionX = menu.GetCustomReference("txt_TeleportPositionX").GetComponent<Text>();
            txtTeleportPositionY = menu.GetCustomReference("txt_TeleportPositionY").GetComponent<Text>();
            txtTeleportPositionZ = menu.GetCustomReference("txt_TeleportPositionZ").GetComponent<Text>();
            btnConfirmTeleport = menu.GetCustomReference("btn_ConfirmTeleport").GetComponent<Button>();
            btnPlayerFrame = menu.GetCustomReference("btn_PlayerFrame").GetComponent<Button>();
            btnPointToTeleport = menu.GetCustomReference("btn_PointToTeleport").GetComponent<Button>();
            txtTeleportPositionDistance = menu.GetCustomReference("txt_TeleportPositionDistance").GetComponent<Text>();
            btnTeleportPositionDistanceMax = menu.GetCustomReference("btn_TeleportPositionDistanceMax").GetComponent<Button>();
            btnTeleportToSpawn = menu.GetCustomReference("btn_TeleportToSpawn").GetComponent<Button>();
            btnTeleportToEndOfDungeon = menu.GetCustomReference("btn_TeleportToEndOfDungeon").GetComponent<Button>();

            // Add an event listener for buttons
            btnArrowsIndicator.onClick.AddListener(ClickArrowsIndicator);
            btnArrowToExit.onClick.AddListener(ClickArrowToExit);
            btnTeleportPositionX.onClick.AddListener(ClickTeleportPositionX);
            btnTeleportPositionY.onClick.AddListener(ClickTeleportPositionY);
            btnTeleportPositionZ.onClick.AddListener(ClickTeleportPositionZ);
            btnConfirmTeleport.onClick.AddListener(ClickConfirmTeleport);
            btnPlayerFrame.onClick.AddListener(ClickPlayerFrame);
            btnPointToTeleport.onClick.AddListener(ClickPointToTeleport);
            btnTeleportPositionDistanceMax.onClick.AddListener(ClickTeleportPositionDistanceMax);
            btnTeleportToSpawn.onClick.AddListener(ClickTeleportToSpawn);
            btnTeleportToEndOfDungeon.onClick.AddListener(ClickTeleportToEndOfDungeon);

            // Initialization of datas

            gPSToolController = GameManager.local.gameObject.AddComponent<GPSToolController>();
            gPSToolController.data.PlayerPositionXGetSet = -999.0f;
            gPSToolController.data.PlayerPositionYGetSet = -999.0f;
            gPSToolController.data.PlayerPositionZGetSet = -999.0f;
            gPSToolController.data.RefreshPositionGetSet = false;
            gPSToolController.data.ToggleArrowsIndicatorGetSet = false;
            gPSToolController.data.ToggleArrowToExitGetSet = false;
            gPSToolController.data.PlayerTeleportPositionConfirmButtonPressedGetSet = false;
            gPSToolController.data.PlayerFrameGetSet = false;
            gPSToolController.data.PlayerTeleportPositionDistanceGetSet = -999.0f;
            gPSToolController.data.PlayerTeleportPositionDistanceMaxGetSet = 50.0f;
            gPSToolController.data.PlayerTeleportToSpawnButtonPressedGetSet = false;
            gPSToolController.data.PlayerTeleportToEndOfDungeonButtonPressedGetSet = false;


            gPSToolHook = menu.gameObject.AddComponent<GPSToolHook>();
            gPSToolHook.menu = this;

            // Update all the Data for left page (text, visibility of buttons etc...)
            UpdateDataPageLeft1();
            // Update all the Data for right page (text, visibility of buttons etc...)
            UpdateDataPageRight1();
        }

        public void ClickArrowsIndicator()
        {
            gPSToolController.data.ToggleArrowsIndicatorGetSet ^= true;
            UpdateDataPageLeft1();
        }
        public void ClickArrowToExit()
        {
            gPSToolController.data.ToggleArrowToExitGetSet ^= true;
            UpdateDataPageLeft1();
        }
        public void ClickTeleportPositionX()
        {
            gPSToolController.data.PlayerTeleportPositionXButtonPressedGetSet = true;
            UpdateDataPageLeft1();
        }
        public void ClickTeleportPositionY()
        {
            gPSToolController.data.PlayerTeleportPositionYButtonPressedGetSet = true;
            UpdateDataPageLeft1();
        }
        public void ClickTeleportPositionZ()
        {
            gPSToolController.data.PlayerTeleportPositionZButtonPressedGetSet = true;
            UpdateDataPageLeft1();
        }
        public void ClickConfirmTeleport()
        {
            gPSToolController.data.PlayerTeleportPositionConfirmButtonPressedGetSet = true;
            UpdateDataPageRight1();
        }
        public void ClickPlayerFrame()
        {
            gPSToolController.data.PlayerFrameGetSet ^= true;
            UpdateDataPageLeft1();
        }
        public void ClickPointToTeleport()
        {
            gPSToolController.data.PointToTeleportGetSet ^= true;
            UpdateDataPageRight1();
        }
        public void ClickTeleportPositionDistanceMax()
        {
            gPSToolController.data.PlayerTeleportPositionDistanceMaxButtonPressedGetSet = true;
            UpdateDataPageRight1();
        }
        public void ClickTeleportToSpawn()
        {
            gPSToolController.data.PlayerTeleportToSpawnButtonPressedGetSet = true;
            UpdateDataPageRight1();
        }
        public void ClickTeleportToEndOfDungeon()
        {
            gPSToolController.data.PlayerTeleportToEndOfDungeonButtonPressedGetSet = true;
            UpdateDataPageRight1();
        }
        public void UpdateDataPageLeft1()
        {
            // Display null if not in map or not in creature
            if (gPSToolController.data.PlayerPositionXGetSet == -999.0f && gPSToolController.data.PlayerPositionYGetSet == -999.0f && gPSToolController.data.PlayerPositionZGetSet == -999.0f || gPSToolController.data.RefreshPositionGetSet == false)
            {
                txtPlayerPositionX.text = "null";
                txtPlayerPositionY.text = "null";
                txtPlayerPositionZ.text = "null";
            }
            // Display the location of the player in map with 5 digits in decimals
            else
            {
                txtPlayerPositionX.text = gPSToolController.data.PlayerPositionXGetSet.ToString("0.000");
                txtPlayerPositionY.text = gPSToolController.data.PlayerPositionYGetSet.ToString("0.000");
                txtPlayerPositionZ.text = gPSToolController.data.PlayerPositionZGetSet.ToString("0.000");
            }
            btnArrowsIndicator.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = gPSToolController.data.ToggleArrowsIndicatorGetSet ? "Enabled" : "Disabled";
            btnArrowToExit.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = gPSToolController.data.ToggleArrowToExitGetSet ? "Enabled" : "Disabled";
            btnPlayerFrame.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = gPSToolController.data.PlayerFrameGetSet ? "Enabled" : "Disabled";
        }

        public void UpdateDataPageRight1()
        {
            btnConfirmTeleport.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = gPSToolController.data.PlayerTeleportPositionConfirmButtonPressedGetSet ? "Confirmed" : "Confirm ?";
            // Assign the position to Teleport To X, Y, or Z when the enter button is pressed
            if (gPSToolController.data.KeyboardFinishEnterButtonPressedGetSet == true)
            {
                // Assign the position to Teleport To X
                if (gPSToolController.data.PlayerTeleportPositionXButtonPressedGetSet == true)
                {
                    gPSToolController.data.PlayerTeleportPositionXGetSet = gPSToolController.data.PlayerTeleportPositionValueToAssignedGetSet;
                    gPSToolController.data.PlayerTeleportPositionXButtonPressedGetSet = false;
                    gPSToolController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the position to Teleport To Y
                if (gPSToolController.data.PlayerTeleportPositionYButtonPressedGetSet == true)
                {
                    gPSToolController.data.PlayerTeleportPositionYGetSet = gPSToolController.data.PlayerTeleportPositionValueToAssignedGetSet;
                    gPSToolController.data.PlayerTeleportPositionYButtonPressedGetSet = false;
                    gPSToolController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the position to Teleport To Z
                if (gPSToolController.data.PlayerTeleportPositionZButtonPressedGetSet == true)
                {
                    gPSToolController.data.PlayerTeleportPositionZGetSet = gPSToolController.data.PlayerTeleportPositionValueToAssignedGetSet;
                    gPSToolController.data.PlayerTeleportPositionZButtonPressedGetSet = false;
                    gPSToolController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the value to Teleport Distance Max
                if (gPSToolController.data.PlayerTeleportPositionDistanceMaxButtonPressedGetSet == true)
                {
                    gPSToolController.data.PlayerTeleportPositionDistanceMaxGetSet = gPSToolController.data.PlayerTeleportPositionValueToAssignedGetSet;
                    gPSToolController.data.PlayerTeleportPositionDistanceMaxButtonPressedGetSet = false;
                    gPSToolController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
            }
            btnPointToTeleport.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = gPSToolController.data.PointToTeleportGetSet ? "Enabled" : "Disabled";
            // Refresh the position teleport to when activate the function point to Teleport
            txtTeleportPositionX.text = gPSToolController.data.PlayerTeleportPositionXGetSet.ToString("0.000");
            txtTeleportPositionY.text = gPSToolController.data.PlayerTeleportPositionYGetSet.ToString("0.000");
            txtTeleportPositionZ.text = gPSToolController.data.PlayerTeleportPositionZGetSet.ToString("0.000");
            // Display the Distance from the player send by the RayCast
            if (gPSToolController.data.PlayerTeleportPositionDistanceGetSet == -999.0f)
            {
                txtTeleportPositionDistance.text = "null";
            }
            else
            {
                txtTeleportPositionDistance.text = gPSToolController.data.PlayerTeleportPositionDistanceGetSet.ToString("0.000");
            }
            // Display the max distance set for the raycast to work (and enable the teleportation)
            btnTeleportPositionDistanceMax.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = gPSToolController.data.PlayerTeleportPositionDistanceMaxGetSet.ToString("0.000");
        }
        // Refresh the menu each frame (need optimization)
        public class GPSToolHook : MonoBehaviour
        {
            public GPSToolMenuModule menu;

            void Update()
            {
                menu.UpdateDataPageLeft1();
                menu.UpdateDataPageRight1();
            }
        }

    }
}
