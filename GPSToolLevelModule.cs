using System.Collections;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

namespace GPSTool
{
    class GPSToolLevelModule : LevelModule
    {
        private GPSToolController gPSToolController;
        private GameObject arrowsIndicator;
        private GameObject arrowToExit;
        AsyncOperationHandle<GameObject> handleArrowsIndicator = Addressables.LoadAssetAsync<GameObject>("Neeshka.GPSTool.ArrowsIndicator");
        AsyncOperationHandle<GameObject> handleArrowToExit = Addressables.LoadAssetAsync<GameObject>("Neeshka.GPSTool.ArrowToExit");
        private Transform transArrowsIndicator;
        private Transform transArrowToExit;
        private bool previousStateTeleportTo = false;
        private bool currentStateTeleportTo = false;
        private bool canTeleport = false;
        private LineRenderer lineRenderer;
        private static Color colorHitBad = new Color(0.8941177f, 0.282353f, 0.08627451f, 1f);
        private static Color colorHitNice = new Color(1f - colorHitBad.r, 1f - colorHitBad.g, 1f - colorHitBad.b, 1f);
        private Vector3 positionToTeleport = new Vector3(0f, 0f, 0f);
        private Quaternion orientationToTeleport = new Quaternion();
        private Ray ray;
        private bool posActivated = false;
        private Vector3 positionOfSpawn = new Vector3(0f, 0f, 0f);
        private Quaternion rotationOfSpawn = new Quaternion();
        private Vector3 positionOfEndOfDungeon = new Vector3(0f, 0f, 0f);
        private Quaternion rotationOfEndOfDungeon = new Quaternion();
        private Vector3 positionOfMap = new Vector3(0f, 0f, 0f);
        private Quaternion rotationOfMap = new Quaternion();
        private bool isPossessed = false;
        private Vector3 destinationToReach = new Vector3(0f, 0f, 0f);
        private int numberOfRoomsInLevel;
        private string levelName;
        // When a level is loaded
        public override IEnumerator OnLoadCoroutine()
        {
            gPSToolController = GameManager.local.gameObject.GetComponent<GPSToolController>();
            // Create an event manager on creature spawn
            EventManager.onUnpossess += EventManager_onUnpossess;
            EventManager.onPossess += EventManager_onPossess;
            return base.OnLoadCoroutine();
        }

        private void EventManager_onUnpossess(Creature creature, EventTime eventTime)
        {
            if (eventTime == EventTime.OnStart)
            {
                if (creature.isPlayer)
                {
                    //Debug.Log("GPSTool : LevelModule : Unpossessed Player !");
                    if (lineRenderer != null)
                    {
                        Object.Destroy(lineRenderer.gameObject);
                    }
                    if (arrowsIndicator != null)
                    {
                        Object.Destroy(arrowsIndicator.gameObject);
                    }
                    if (arrowToExit != null)
                    {
                        Object.Destroy(arrowToExit.gameObject);
                    }
                    //Player.local.creature.handRight?.SetOpenPose();
                    //Player.local.creature.handRight?.SetClosePose();
                    isPossessed = false;
                    if (Level.current.dungeon != null)
                    {
                        Level.current.dungeon.onPlayerChangeRoom -= Dungeon_onPlayerChangeRoom;
                    }
                }
            }
        }

        private void EventManager_onPossess(Creature creature, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
            {
                isPossessed = true;
                levelName = Level.current.data.id;
                GetPositionOfPlayer();
                GetPositionOfMap();
                if (Level.current.dungeon != null)
                {
                    Level.current.dungeon.onPlayerChangeRoom += Dungeon_onPlayerChangeRoom;
                    numberOfRoomsInLevel = Level.current.dungeon.rooms.Count;
                    destinationToReach = Level.current.dungeon.rooms[1].entryDoor.transform.position;
                    GetPositionOfEndOfDungeon();
                }
            }
        }

        private void Dungeon_onPlayerChangeRoom(Room oldRoom, Room newRoom)
        {
            if (oldRoom.index < newRoom.index && newRoom.exitDoor != null)
            {
                destinationToReach = newRoom.exitDoor.transform.position;
            }
            else
            {
                destinationToReach = oldRoom.entryDoor.transform.position;
            }
        }

        // Update the location of the player
        public override void Update()
        {
            if (gPSToolController == null)
            {
                gPSToolController = GameManager.local.gameObject.GetComponent<GPSToolController>();
                return;
            }
            else
            {
                // Initiate the line renderer
                if (lineRenderer == null)
                {
                    lineRenderer = new GameObject().AddComponent<LineRenderer>();
                    lineRenderer.widthMultiplier = 0.002f;
                    lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
                    lineRenderer.enabled = false;
                }
                if (gPSToolController.data.PointToTeleportGetSet == true)
                {
                    if (Player.currentCreature != null && isPossessed)
                    {
                        ChangeHandPose(true);
                        ray = new Ray(Player.local.creature.handRight.fingerIndex.tip.position, -Player.local.creature.handRight.transform.right);
                        if (lineRenderer?.enabled == false)
                        {
                            lineRenderer.enabled = true;
                        }
                        else
                        {
                            if (lineRenderer.enabled && Physics.Raycast(ray, out RaycastHit raycastHit, float.PositiveInfinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                            {
                                lineRenderer.SetPositions(new Vector3[] { Player.local.creature.handRight.fingerIndex.tip.position, raycastHit.point });
                                if (raycastHit.collider && raycastHit.distance <= gPSToolController.data.PlayerTeleportPositionDistanceMaxGetSet)
                                {
                                    lineRenderer.material.SetColor("_BaseColor", colorHitNice);
                                    gPSToolController.data.PlayerTeleportPositionXGetSet = raycastHit.point.x;
                                    gPSToolController.data.PlayerTeleportPositionYGetSet = raycastHit.point.y;
                                    gPSToolController.data.PlayerTeleportPositionZGetSet = raycastHit.point.z;
                                    gPSToolController.data.PlayerTeleportPositionDistanceGetSet = raycastHit.distance;
                                    canTeleport = true;
                                }
                                else
                                {
                                    lineRenderer.material.SetColor("_BaseColor", colorHitBad);
                                    gPSToolController.data.PlayerTeleportPositionXGetSet = raycastHit.point.x;
                                    gPSToolController.data.PlayerTeleportPositionYGetSet = raycastHit.point.y;
                                    gPSToolController.data.PlayerTeleportPositionZGetSet = raycastHit.point.z;
                                    gPSToolController.data.PlayerTeleportPositionDistanceGetSet = raycastHit.distance;
                                    canTeleport = false;
                                }
                            }
                            else
                            {
                                canTeleport = false;
                                gPSToolController.data.PlayerTeleportPositionDistanceGetSet = -999.0f;
                            }
                            currentStateTeleportTo = PlayerControl.GetHand(Side.Right).gripPressed;
                            if (currentStateTeleportTo != previousStateTeleportTo && previousStateTeleportTo == false && canTeleport == true)
                            {
                                TeleportTo(true);
                            }
                        }
                        previousStateTeleportTo = currentStateTeleportTo;
                    }
                }
                else
                {
                    if (Player.currentCreature != null)
                    {
                        if (posActivated == true)
                        {
                            ChangeHandPose(false);
                        }
                    }
                    if (lineRenderer?.enabled == true)
                    {
                        lineRenderer.enabled = false;
                    }
                }
                // If button enabled
                if (gPSToolController.data.ToggleArrowsIndicatorGetSet == true)
                {
                    // Creates arrows object if not created
                    if (arrowsIndicator == null && Player.local.creature != null)
                    {
                        arrowsIndicator = handleArrowsIndicator.WaitForCompletion();
                        arrowsIndicator = Object.Instantiate(arrowsIndicator);
                        arrowsIndicator.SetActive(true);
                        arrowsIndicator.transform.position = Player.local.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
                    }

                    // Activate arrows
                    if (arrowsIndicator != null)
                    {
                        if (!arrowsIndicator.activeSelf)
                            arrowsIndicator.SetActive(true);
                    }
                }
                // If button disabled, deactivate the arrows
                else
                {
                    if (arrowsIndicator != null)
                    {
                        if (arrowsIndicator.activeSelf)
                        {
                            arrowsIndicator.SetActive(false);
                        }
                    }
                }
                // If button enabled
                if (gPSToolController.data.ToggleArrowToExitGetSet == true)
                {
                    // Creates arrows object if not created
                    if (arrowToExit == null && Player.local.creature != null)
                    {
                        arrowToExit = handleArrowToExit.WaitForCompletion();
                        arrowToExit = Object.Instantiate(arrowToExit);
                        arrowToExit.SetActive(true);
                        arrowToExit.transform.localScale = Vector3.one * 0.4f;
                        arrowToExit.transform.SetParent(Player.currentCreature.handLeft.transform);
                        arrowToExit.transform.position = Snippet.PosAboveBackOfHand(Player.currentCreature.handLeft, 0.25f);
                        arrowToExit.transform.rotation = Quaternion.LookRotation(destinationToReach - Player.currentCreature.transform.position, Vector3.up);
                    }

                    // Activate arrows
                    if (arrowToExit != null)
                    {
                        if (!arrowToExit.activeSelf)
                            arrowToExit.SetActive(true);
                        else
                        {
                            transArrowToExit = arrowToExit.transform;
                            transArrowToExit.position = Player.local.creature.handRight.fingerIndex.tip.transform.position;
                            transArrowToExit.transform.position = Snippet.PosAboveBackOfHand(Player.currentCreature.handLeft, 0.5f);
                            transArrowToExit.transform.rotation = Quaternion.LookRotation(destinationToReach - Player.currentCreature.transform.position, Vector3.up);
                        }
                    }
                }
                // If button disabled, deactivate the arrows
                else
                {
                    if (arrowToExit != null)
                    {
                        if (arrowToExit.activeSelf)
                        {
                            arrowToExit.SetActive(false);
                        }
                    }
                }
                if (Player.currentCreature != null)
                {
                    // Refresh the position of the Player
                    gPSToolController.data.RefreshPositionGetSet = true;
                    // Spawn arrows for directions :
                    if (PlayerControl.GetHand(Side.Right).gripPressed && arrowsIndicator != null && !(PlayerControl.GetHand(Side.Right).castPressed || PlayerControl.GetHand(Side.Right).usePressed))
                    {
                        transArrowsIndicator = arrowsIndicator.transform;
                        transArrowsIndicator.position = Player.local.creature.handRight.fingerIndex.tip.transform.position;
                    }

                    // Refresh the position of the Player
                    if (gPSToolController.data.RefreshPositionGetSet == true)
                    {
                        RefreshPlayerPosition();
                    }

                    // If button confirm teleport is used
                    if (gPSToolController.data.PlayerTeleportPositionConfirmButtonPressedGetSet == true)
                    {
                        TeleportTo(false);
                    }
                    if (gPSToolController.data.PlayerTeleportToSpawnButtonPressedGetSet == true)
                    {
                        TeleportToSpawn();
                    }
                    if (gPSToolController.data.PlayerTeleportToEndOfDungeonButtonPressedGetSet == true)
                    {
                        TeleportToEndOfDungeon();
                    }
                    if (gPSToolController.data.PlayerTeleportToMapButtonPressedGetSet == true)
                    {
                        TeleportToMap();
                    }
                }
                else
                {
                    gPSToolController.data.RefreshPositionGetSet = false;
                }
            }
        }

        public void RefreshPlayerPosition()
        {
            if (Player.local?.creature?.handRight?.fingerIndex?.tip?.transform != null)
            {
                gPSToolController.data.PlayerPositionXGetSet = Player.local.creature.handRight.fingerIndex.tip.transform.position.x;
                gPSToolController.data.PlayerPositionYGetSet = Player.local.creature.handRight.fingerIndex.tip.transform.position.y;
                gPSToolController.data.PlayerPositionZGetSet = Player.local.creature.handRight.fingerIndex.tip.transform.position.z;
            }
        }

        public void ChangeHandPose(bool pointTrue)
        {
            if (Player.currentCreature != null)
            {
                if (pointTrue)
                {
                    posActivated = true;
                    // Set the pointing hand pose and then extend the index !
                }
                else
                {
                    posActivated = false;
                }
            }
        }
        public void TeleportTo(bool usePointer)
        {
            positionToTeleport.x = gPSToolController.data.PlayerTeleportPositionXGetSet;
            positionToTeleport.y = gPSToolController.data.PlayerTeleportPositionYGetSet;
            positionToTeleport.z = gPSToolController.data.PlayerTeleportPositionZGetSet;
            if (usePointer == false)
            {
                if (gPSToolController.data.PlayerFrameGetSet == true)
                {
                    Player.local.Teleport(Player.local.transform.position + positionToTeleport, Player.local.creature.transform.rotation);
                }
                else
                {
                    Player.local.Teleport(positionToTeleport, Player.local.creature.transform.rotation);
                }
            }
            else
            {
                Player.local.Teleport(positionToTeleport, Player.local.creature.transform.rotation);
            }
            gPSToolController.data.PlayerTeleportPositionConfirmButtonPressedGetSet = false;
        }

        public void TeleportToSpawn()
        {
            positionToTeleport = positionOfSpawn;
            orientationToTeleport = rotationOfSpawn;
            Player.local.Teleport(positionToTeleport, orientationToTeleport);
            gPSToolController.data.PlayerTeleportToSpawnButtonPressedGetSet = false;
        }
        public void TeleportToEndOfDungeon()
        {
            Player.local.Teleport(positionOfEndOfDungeon, rotationOfEndOfDungeon);
            gPSToolController.data.PlayerTeleportToEndOfDungeonButtonPressedGetSet = false;
        }
        public void TeleportToMap()
        {
            Player.local.Teleport(positionOfMap, rotationOfMap);
            gPSToolController.data.PlayerTeleportToMapButtonPressedGetSet = false;
        }
        private void GetPositionOfMap()
        {
            if (levelName == "Home")
            {
                gPSToolController.data.LevelIsHomeGetSet = true;
                positionOfMap = new Vector3(71.8f, -4f, -75f);
                rotationOfMap = Quaternion.Euler(0, -100f, 0);
            }
            else
            {
                gPSToolController.data.LevelIsHomeGetSet = false;
            }
        }
        private void GetPositionOfPlayer()
        {
            positionOfSpawn = Player.local.transform.position;
            rotationOfSpawn = Player.local.transform.rotation;
        }
        private void GetPositionOfEndOfDungeon()
        {
            if (Level.current.dungeon.rooms.FirstOrDefault(room => room.exitDoor == null).GetPlayerSpawner() is PlayerSpawner spawner)
            {
                positionOfEndOfDungeon = spawner.transform.position;
                rotationOfEndOfDungeon = spawner.transform.rotation;
                gPSToolController.data.LevelIsDungeonGetSet = true;
            }
            else
            {
                gPSToolController.data.LevelIsDungeonGetSet = false;
            }
        }
    }
}