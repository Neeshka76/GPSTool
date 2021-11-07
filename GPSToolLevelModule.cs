using System.Collections;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GPSTool
{
    class GPSToolLevelModule : LevelModule
    {
        private GPSToolController gPSToolController;
        private GameObject arrowsIndicator;
        AsyncOperationHandle<GameObject> handleArrowsIndicator = Addressables.LoadAssetAsync<GameObject>("Neeshka.GPSTool.ArrowsIndicator");
        private Transform transArrowsIndicator;
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
        private Quaternion orientationOfSpawn = new Quaternion();
        // When a level is loaded
        public override System.Collections.IEnumerator OnLoadCoroutine()
        {
            gPSToolController = GameManager.local.gameObject.GetComponent<GPSToolController>();
            // Create an event manager on creature spawn
            EventManager.onCreatureSpawn += EventManager_onCreatureSpawn;
            EventManager.onLevelUnload += EventManager_onLevelUnload;
            EventManager.onUnpossess += EventManager_onUnpossess;
            return base.OnLoadCoroutine();
        }
        private void EventManager_onCreatureSpawn(Creature creature)
        {
            // If creature is not hidden and isn't the player and the selector not on default
            if (creature.isPlayer)
            {
                GetPositionOfPlayer();
            }
        }
        private void EventManager_onUnpossess(Creature creature, EventTime eventTime)
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
            }
        }

        private void EventManager_onLevelUnload(LevelData levelData, EventTime eventTime)
        {
            //Debug.Log("GPSTool : LevelModule : LevelUnLoaded !");
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
                    lineRenderer.enabled = false;
                }
                if (gPSToolController.data.PointToTeleportGetSet == true)
                {
                    if (Player.currentCreature != null)
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
                        //handleArrowsIndicator = Addressables.LoadAssetAsync<GameObject>("Neeshka.GPSTool.ArrowsIndicator");
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
                }
                else
                {
                    gPSToolController.data.RefreshPositionGetSet = false;
                }
            }
        }

        public void RefreshPlayerPosition()
        {
            gPSToolController.data.PlayerPositionXGetSet = Player.local.creature.handRight.fingerIndex.tip.transform.position.x;
            gPSToolController.data.PlayerPositionYGetSet = Player.local.creature.handRight.fingerIndex.tip.transform.position.y;
            gPSToolController.data.PlayerPositionZGetSet = Player.local.creature.handRight.fingerIndex.tip.transform.position.z;
        }

        public void ChangeHandPose(bool pointTrue)
        {
            if (Player.currentCreature != null)
            {
                if (pointTrue)
                {
                    posActivated = true;
                    // Set the pointing hand pose and then extend the index !
                    Player.local.creature.handRight?.SetOpenPose(Catalog.GetData<HandPoseData>("Pointing"));
                    Player.local.creature.handRight?.SetClosePose(Catalog.GetData<HandPoseData>("Pointing"));
                    Player.local.creature.handRight?.UpdatePoseIndex(0.0f);
                }
                else
                {
                    posActivated = false;
                    Player.local.creature.handRight?.SetOpenPose();
                    Player.local.creature.handRight?.SetClosePose();
                }
            }
        }
        public void TeleportTo(bool usePointer)
        {
            Player.local.locomotion.rb.isKinematic = true;
            positionToTeleport.x = gPSToolController.data.PlayerTeleportPositionXGetSet;
            positionToTeleport.y = gPSToolController.data.PlayerTeleportPositionYGetSet;
            positionToTeleport.z = gPSToolController.data.PlayerTeleportPositionZGetSet;
            if (usePointer == false)
            {
                if (gPSToolController.data.PlayerFrameGetSet == true)
                {
                    Player.local.transform.position += positionToTeleport;
                }
                else
                {
                    Player.local.transform.position = positionToTeleport;
                }
            }
            else
            {
                Player.local.transform.position = positionToTeleport;
            }
            Player.local.locomotion.rb.isKinematic = false;
            gPSToolController.data.PlayerTeleportPositionConfirmButtonPressedGetSet = false;
        }

        public void TeleportToSpawn()
        {
            Player.local.locomotion.rb.isKinematic = true;
            positionToTeleport = positionOfSpawn;
            orientationToTeleport = orientationOfSpawn;
            Player.local.transform.position = positionToTeleport;
            Player.local.locomotion.rb.isKinematic = false;
            gPSToolController.data.PlayerTeleportToSpawnButtonPressedGetSet = false;
        }

        private void GetPositionOfPlayer()
        {
            positionOfSpawn = Player.local.creature.transform.position;
            orientationOfSpawn = Player.local.creature.transform.rotation;
        }
    }
}