using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using SnippetCode;
using System.Collections;

namespace SnippetCode
{
    public static class SnippetCode
    {
        /// <summary>
        /// Vector pointing away from the palm
        /// </summary>
        public static Vector3 PalmDir(this RagdollHand hand) => -hand.transform.forward;

        /// <summary>
        /// Vector pointing in the direction of the thumb
        /// </summary>
        public static Vector3 ThumbDir(this RagdollHand hand) => (hand.side == Side.Right) ? hand.transform.up : -hand.transform.up;

        /// <summary>
        /// Vector pointing away in the direction of the fingers
        /// </summary>
        public static Vector3 PointDir(this RagdollHand hand) => -hand.transform.right;

        /// <summary>
        /// Get a point above the player's hand
        /// </summary>
        public static Vector3 PosAboveBackOfHand(this RagdollHand hand, float factor) => hand.transform.position - hand.transform.right * 0.1f * factor + hand.transform.forward * 0.2f * factor;

        public static void SetVFXProperty<T>(this EffectInstance effect, string name, T data)
        {
            if (effect == null)
                return;
            if (data is Vector3 v)
            {
                foreach (EffectVfx effectVfx in effect.effects.Where<Effect>((Func<Effect, bool>)(fx => fx is EffectVfx effectVfx17 && effectVfx17.vfx.HasVector3(name))))
                    effectVfx.vfx.SetVector3(name, v);
            }
            else if (data is float f2)
            {
                foreach (EffectVfx effectVfx2 in effect.effects.Where<Effect>((Func<Effect, bool>)(fx => fx is EffectVfx effectVfx18 && effectVfx18.vfx.HasFloat(name))))
                    effectVfx2.vfx.SetFloat(name, f2);
            }
            else if (data is int i3)
            {
                foreach (EffectVfx effectVfx2 in effect.effects.Where<Effect>((Func<Effect, bool>)(fx => fx is EffectVfx effectVfx19 && effectVfx19.vfx.HasInt(name))))
                    effectVfx2.vfx.SetInt(name, i3);
            }
            else if (data is bool b4)
            {
                foreach (EffectVfx effectVfx2 in effect.effects.Where<Effect>((Func<Effect, bool>)(fx => fx is EffectVfx effectVfx20 && effectVfx20.vfx.HasBool(name))))
                    effectVfx2.vfx.SetBool(name, b4);
            }
            else
            {
                if (!(data is Texture t5))
                    return;
                foreach (EffectVfx effectVfx2 in effect.effects.Where<Effect>((Func<Effect, bool>)(fx => fx is EffectVfx effectVfx21 && effectVfx21.vfx.HasTexture(name))))
                    effectVfx2.vfx.SetTexture(name, t5);
            }
        }
        public static object GetVFXProperty(this EffectInstance effect, string name)
        {
            foreach (Effect effect1 in effect.effects)
            {
                if (effect1 is EffectVfx effectVfx1)
                {
                    if (effectVfx1.vfx.HasFloat(name))
                        return effectVfx1.vfx.GetFloat(name);
                    if (effectVfx1.vfx.HasVector3(name))
                        return effectVfx1.vfx.GetVector3(name);
                    if (effectVfx1.vfx.HasBool(name))
                        return effectVfx1.vfx.GetBool(name);
                    if (effectVfx1.vfx.HasInt(name))
                        return effectVfx1.vfx.GetInt(name);
                }
            }
            return null;
        }
        public static Vector3 zero = Vector3.zero;
        public static Vector3 one = Vector3.one;
        public static Vector3 forward = Vector3.forward;
        public static Vector3 right = Vector3.right;
        public static Vector3 up = Vector3.up;
        public static Vector3 back = Vector3.back;
        public static Vector3 left = Vector3.left;
        public static Vector3 down = Vector3.down;

        public static bool XBigger(this Vector3 vec) => Mathf.Abs(vec.x) > Mathf.Abs(vec.y) && Mathf.Abs(vec.x) > Mathf.Abs(vec.z);

        public static bool YBigger(this Vector3 vec) => Mathf.Abs(vec.y) > Mathf.Abs(vec.x) && Mathf.Abs(vec.y) > Mathf.Abs(vec.z);

        public static bool ZBigger(this Vector3 vec) => Mathf.Abs(vec.z) > Mathf.Abs(vec.x) && Mathf.Abs(vec.z) > Mathf.Abs(vec.y);
        public static Vector3 Velocity(this RagdollHand hand) => Player.local.transform.rotation * hand.playerHand.controlHand.GetHandVelocity();
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() ?? obj.AddComponent<T>();
        }
        public static bool IsPlayer(this RagdollPart part) => part?.ragdoll?.creature.isPlayer == true;
        public static bool IsImportant(this RagdollPart part)
        {
            var type = part.type;
            return type == RagdollPart.Type.Head
                   || type == RagdollPart.Type.Torso
                   || type == RagdollPart.Type.LeftHand
                   || type == RagdollPart.Type.RightHand
                   || type == RagdollPart.Type.LeftFoot
                   || type == RagdollPart.Type.RightFoot;
        }
        /// <summary>
        /// Get a creature's part from a PartType
        /// </summary>
        public static RagdollPart GetPart(this Creature creature, RagdollPart.Type partType)
            => creature.ragdoll.GetPart(partType);

        /// <summary>
        /// Get a creature's head
        /// </summary>
        public static RagdollPart GetHead(this Creature creature) => creature.ragdoll.headPart;

        /// <summary>
        /// Get a creature's torso
        /// </summary>
        public static RagdollPart GetTorso(this Creature creature) => creature.GetPart(RagdollPart.Type.Torso);

        public static Vector3 GetChest(this Creature creature) => Vector3.Lerp(creature.GetTorso().transform.position,
            creature.GetHead().transform.position, 0.5f);
        public static IEnumerable<Creature> CreaturesInRadius(this Vector3 position, float radius)
        {
            return Creature.allActive.Where(creature => (creature.GetChest() - position).sqrMagnitude < radius * radius);
        }

        public static IEnumerable<Creature> CreatureInRadiusMinusPlayer(this Vector3 position, float radius)
        {
            return Creature.allActive.Where(creature =>
                ((creature.GetChest() - position).sqrMagnitude < radius * radius) && !creature.isPlayer
            );
        }
        public static void Depenetrate(this Item item)
        {
            foreach (var handler in item.collisionHandlers)
            {
                foreach (var damager in handler.damagers)
                {
                    damager.UnPenetrateAll();
                }
            }
        }
        /// <summary>
        /// Get a creature's random part
        /// </summary>
        /*
        Head
        Neck
        Torso
        LeftArm
        RightArm
        LeftHand
        RightHand
        LeftLeg
        RightLeg
        LeftFoot
        RightFoot
        */
        public static RagdollPart GetRandomRagdollPart(this Creature creature)
        {
            Array values = Enum.GetValues(typeof(RagdollPart.Type));
            return creature.ragdoll.GetPart((RagdollPart.Type)values.GetValue(UnityEngine.Random.Range(0, values.Length)));
        }

        public static bool returnWaveStarted()
        {
            int nbWaveStarted = 0;
            foreach (WaveSpawner waveSpawner in WaveSpawner.instances)
            {
                if (waveSpawner.isRunning)
                {
                    nbWaveStarted++;
                }
            }
            return nbWaveStarted != 0 ? true : false;
        }

        public static Vector3 FromToDirection(this Vector3 from, Vector3 to)
        {
            return to - from;
        }
        /// <summary>
        /// Add a force that attracts when coef is positive and repulse when is negative
        /// </summary>
        public static void Attraction_Repulsion_Force(this Rigidbody rigidbody, Vector3 origin, Vector3 attractedRb, bool useDistance, float coef)
        {
            if (useDistance)
            {
                float distance = FromToDirection(attractedRb, origin).magnitude;
                Vector3 direction = FromToDirection(attractedRb, origin).normalized;
                rigidbody.AddForce(direction * (coef / distance) / (rigidbody.mass / 2), ForceMode.VelocityChange);
            }
            else
            {
                Vector3 direction = FromToDirection(attractedRb, origin).normalized;
                rigidbody.AddForce(direction * coef / (rigidbody.mass / 2), ForceMode.VelocityChange);
            }
        }
        /// <summary>
        /// Add a force that attracts when coef is positive and repulse when is negative
        /// </summary>
        public static void Attraction_Repulsion_ForceNoMass(this Rigidbody rigidbody, Vector3 origin, Vector3 attractedRb, bool useDistance, float coef)
        {
            if (useDistance)
            {
                float distance = FromToDirection(attractedRb, origin).magnitude;
                Vector3 direction = FromToDirection(attractedRb, origin).normalized;
                rigidbody.AddForce(direction * (coef / distance), ForceMode.VelocityChange);
            }
            else
            {
                Vector3 direction = FromToDirection(attractedRb, origin).normalized;
                rigidbody.AddForce(direction * coef, ForceMode.VelocityChange);
            }
        }

        public static Vector3[] CreateCircle(this Vector3 origin, Vector3 direction, float radius, int nbElementsAroundCircle)
        {
            Vector3[] positions = new Vector3[nbElementsAroundCircle];
            int angle = 360 / nbElementsAroundCircle;
            for (int i = 0; i < nbElementsAroundCircle; i++)
            {
                positions[i] = origin + direction * radius;
            }
            return positions;
        }
        public static void RotateCircle(this Vector3[] positions, Vector3 origin, Vector3 direction, float radius, int speed)
        {
            float rotationspeed = 0;
            rotationspeed += Time.deltaTime * speed;
        }

        public static ConfigurableJoint CreateJointToProjectileForCreatureAttraction(this Item projectile, RagdollPart attractedRagdollPart, ConfigurableJoint joint)
        {
            JointDrive jointDrive = new JointDrive();
            jointDrive.positionSpring = 1f;
            jointDrive.positionDamper = 0.2f;
            SoftJointLimit softJointLimit = new SoftJointLimit();
            softJointLimit.limit = 0.15f;
            SoftJointLimitSpring linearLimitSpring = new SoftJointLimitSpring();
            linearLimitSpring.spring = 1f;
            linearLimitSpring.damper = 0.2f;
            joint = attractedRagdollPart.gameObject.AddComponent<ConfigurableJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.targetRotation = Quaternion.identity;
            joint.anchor = Vector3.zero;
            joint.connectedBody = projectile.GetComponent<Rigidbody>();
            joint.connectedAnchor = Vector3.zero;
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;
            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Limited;
            joint.linearLimitSpring = linearLimitSpring;
            joint.linearLimit = softJointLimit;
            joint.angularXLimitSpring = linearLimitSpring;
            joint.xDrive = jointDrive;
            joint.yDrive = jointDrive;
            joint.zDrive = jointDrive;
            joint.massScale = 10000f;
            joint.connectedMassScale = 0.00001f;
            return joint;
        }

        public static void IgnoreCollider(this Ragdoll ragdoll, Collider collider, bool ignore = true)
        {
            foreach (var part in ragdoll.parts)
            {
                part.IgnoreCollider(collider, ignore);
            }
        }

        public static void IgnoreCollider(this RagdollPart part, Collider collider, bool ignore = true)
        {
            foreach (var itemCollider in part.colliderGroup.colliders)
            {
                Physics.IgnoreCollision(collider, itemCollider, ignore);
            }
        }

        public static void IgnoreCollider(this Item item, Collider collider, bool ignore)
        {
            foreach (var cg in item.colliderGroups)
            {
                foreach (var itemCollider in cg.colliders)
                {
                    Physics.IgnoreCollision(collider, itemCollider, ignore);
                }
            }
        }

        public static void addIgnoreRagdollAndItemHoldingCollision(Item item, Creature creature)
        {
            foreach (ColliderGroup colliderGroup in item.colliderGroups)
            {
                foreach (Collider collider in colliderGroup.colliders)
                    creature.ragdoll.IgnoreCollision(collider, true);
            }
            item.ignoredRagdoll = creature.ragdoll;

            if (creature.handLeft.grabbedHandle?.item != null)
            {
                foreach (ColliderGroup colliderGroup1 in item.colliderGroups)
                {
                    foreach (Collider collider1 in colliderGroup1.colliders)
                    {
                        foreach (ColliderGroup colliderGroup2 in creature.handLeft.grabbedHandle.item.colliderGroups)
                        {
                            foreach (Collider collider2 in colliderGroup2.colliders)
                                Physics.IgnoreCollision(collider1, collider2, true);
                        }
                    }
                }
                item.ignoredItem = creature.handLeft.grabbedHandle.item;
            }

            if (creature.handRight.grabbedHandle?.item != null)
            {
                foreach (ColliderGroup colliderGroup1 in item.colliderGroups)
                {
                    foreach (Collider collider1 in colliderGroup1.colliders)
                    {
                        foreach (ColliderGroup colliderGroup2 in creature.handRight.grabbedHandle.item.colliderGroups)
                        {
                            foreach (Collider collider2 in colliderGroup2.colliders)
                                Physics.IgnoreCollision(collider1, collider2, true);
                        }
                    }
                }
                item.ignoredItem = creature.handRight.grabbedHandle.item;
            }
        }

        /// <summary>
        /// return the head, torso, leftHand, rightHand, leftFoot and rightFoot of the creature
        /// </summary>
        public static List<RagdollPart> RagdollPartsImportantList(this Creature creature)
        {
            List<RagdollPart> ragdollPartsimportant = new List<RagdollPart> {
                creature.GetPart(RagdollPart.Type.Head),
                creature.GetPart(RagdollPart.Type.Torso),
                creature.GetPart(RagdollPart.Type.LeftHand),
                creature.GetPart(RagdollPart.Type.RightHand),
                creature.GetPart(RagdollPart.Type.LeftFoot),
                creature.GetPart(RagdollPart.Type.RightFoot)};
            return ragdollPartsimportant;
        }
        /// <summary>
        /// return the leftHand, rightHand, leftFoot and rightFoot of the creature
        /// </summary>
        public static List<RagdollPart> RagdollPartsExtremitiesBodyList(this Creature creature)
        {
            List<RagdollPart> ragdollPartsimportant = new List<RagdollPart> {
                creature.GetPart(RagdollPart.Type.LeftHand),
                creature.GetPart(RagdollPart.Type.RightHand),
                creature.GetPart(RagdollPart.Type.LeftFoot),
                creature.GetPart(RagdollPart.Type.RightFoot)};
            return ragdollPartsimportant;
        }

        public static Vector3 RandomPositionAroundCreatureInRadius(this Creature creature, float radius)
        {
            Vector3 position;
            return position = creature.transform.position + new Vector3(UnityEngine.Random.Range(-radius, radius), 0, UnityEngine.Random.Range(-radius, radius));
        }

        public static Vector3 CalculatePositionFromAngleWithDistance(this Vector3 position, Vector3 forward, float angle, Vector3 axis, float distance)
        {
            Vector3 positionReturned;
            positionReturned = position + Quaternion.AngleAxis(angle, axis) * forward * distance;
            return positionReturned;
        }

        public static void DebugPosition(this Vector3 position, string textToDisplay)
        {
            Debug.Log("SnippetCode : " + textToDisplay + " : " + "Position X : " + position.x.ToString() + "; Position Y : " + position.y.ToString() + "; Position Z : " + position.z.ToString());
        }

        private static IEnumerator LerpMovement(this Vector3 positionToReach, Quaternion rotationToReach, Item itemToMove, float durationOfMvt)
        {
            //Debug.Log("FireballHandling : Layer : " + itemToMove.gameObject.layer.ToString());
            foreach (ColliderGroup colliderGroup in itemToMove.colliderGroups)
            {
                foreach (Collider collider in colliderGroup.colliders)
                {
                    collider.enabled = false;
                }
            }
            float time = 0;
            Vector3 positionOrigin = itemToMove.transform.position;
            Quaternion orientationOrigin = itemToMove.transform.rotation;
            if (positionToReach != positionOrigin)
            {
                while (time < durationOfMvt)
                {
                    //itemToMove.isFlying = true;
                    //itemToMove.rb.position = Vector3.Lerp(positionOrigin, positionToReach, time / durationOfMvt);
                    //itemToMove.rb.rotation = Quaternion.Lerp(orientationOrigin, rotationToReach, time / durationOfMvt);
                    itemToMove.transform.position = Vector3.Lerp(positionOrigin, positionToReach, time / durationOfMvt);
                    itemToMove.transform.rotation = Quaternion.Lerp(orientationOrigin, rotationToReach, time / durationOfMvt);
                    time += Time.deltaTime;
                    yield return null;
                }
            }
            //itemToMove.rb.position = positionToReach;
            foreach (ColliderGroup colliderGroup in itemToMove.colliderGroups)
            {
                foreach (Collider collider in colliderGroup.colliders)
                {
                    collider.enabled = true;
                }
            }
        }

        public static List<Item> GetItemsOnCreature(this Creature creature, ItemData.Type? dataType)
        {
            List<Item> list = new List<Item>();
            foreach (Holder holder in creature.equipment.holders)
            {
                foreach (Item item in holder.items)
                {
                    if (dataType.HasValue)
                    {
                        if (item.data.type == dataType && dataType.HasValue)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            if (creature.handLeft.grabbedHandle?.item != null)
            {
                list.Add(creature.handLeft.grabbedHandle.item);
            }
            if (creature.handRight.grabbedHandle?.item != null)
            {
                list.Add(creature.handRight.grabbedHandle.item);
            }
            if (creature.mana.casterLeft.telekinesis.catchedHandle?.item != null)
            {
                list.Add(creature.mana.casterLeft.telekinesis.catchedHandle?.item);
            }
            if (creature.mana.casterRight.telekinesis.catchedHandle?.item != null)
            {
                list.Add(creature.mana.casterRight.telekinesis.catchedHandle?.item);
            }
            return list;
        }

        public static int ReturnNbFreeSlotOnCreature(this Creature creature)
        {
            int nbFreeSlots = 0;
            foreach (Holder holder in creature.equipment.holders)
            {
                if (holder.currentQuantity != 0)
                {
                    nbFreeSlots++;
                }
            }

            return nbFreeSlots;
        }

    }
}
