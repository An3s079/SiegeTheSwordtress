using Gungeon;
using ItemAPI;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Dungeonator;
using System.Collections.ObjectModel;
using System.Reflection;
using Brave.BulletScript;
using tk2dRuntime.TileMap;

namespace Swordtress
{
    public static class VFXLibrary
    {
        public static Color emptyColor = new Color(0, 0, 0);
        /// <summary>
        /// Used to create new VFX. Place sprites inside of Mods/YourModFolder/sprites/VFX Collection, inside the EtG directory. Each parameter that is a list must have an entry for each sprite of the animation
        /// </summary>
        /// <param name="name">The name of the VFX</param> 
        /// <param name="spriteNames">The name of the sprites for the VFX, like VFX_001, VFX_002, etc.</param>
        /// <param name="fps">The frame rate the animation will play at</param>
        /// <param name="spriteSizes">The size of each sprite in the animation. Must have an IntVector2 entry for each sprite in the animation</param>
        /// <param name="anchors">I don't know what exactly this does</param>
        /// <param name="manualOffsets">I don't know what exactly this does</param>
        /// <param name="orphaned">I don't know what exactly this does</param>
        /// <param name="attached">I don't know what exactly this does</param>
        /// <param name="persistsOnDeath">Best left false</param>
        /// <param name="usesZHeight">Change if needed</param>
        /// <param name="zHeight">Change if needed, but usually left at 0</param>
        /// <param name="alignment">Leave at Fixed</param>
        /// <param name="destructible">Always set to true</param>
        /// <param name="emissivePowers">Doesn't seem to work, set all entries at 0. Might cause bugs if tinkered with</param>
        /// <param name="emissiveColors">Does't seem to work, set all entries to VFXLibrary.emptyColor. Might cause bugs if tinkered with</param>
        /// <returns></returns>
        public static VFXPool CreateMuzzleflash(string name, List<string> spriteNames, int fps, List<IntVector2> spriteSizes, List<tk2dBaseSprite.Anchor> anchors, List<Vector2> manualOffsets, bool orphaned, bool attached, bool persistsOnDeath,
            bool usesZHeight, float zHeight, VFXAlignment alignment, bool destructible, List<float> emissivePowers, List<Color> emissiveColors)
        {
            VFXPool pool = new VFXPool();
            pool.type = VFXPoolType.All;
            VFXComplex complex = new VFXComplex();
            VFXObject vfObj = new VFXObject();
            GameObject obj = new GameObject(name);
            obj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(obj);
            UnityEngine.Object.DontDestroyOnLoad(obj);
            tk2dSprite sprite = obj.AddComponent<tk2dSprite>();
            tk2dSpriteAnimator animator = obj.AddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip();
            clip.fps = fps;
            clip.frames = new tk2dSpriteAnimationFrame[0];
            for (int i = 0; i < spriteNames.Count; i++)
            {
                string spriteName = spriteNames[i];
                IntVector2 spriteSize = spriteSizes[i];
                tk2dBaseSprite.Anchor anchor = anchors[i];
                Vector2 manualOffset = manualOffsets[i];
                float emissivePower = emissivePowers[i];
                Color emissiveColor = emissiveColors[i];
                tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
                frame.spriteId = VFXCollection.GetSpriteIdByName(spriteName);
                tk2dSpriteDefinition def = SetupDefinitionForShellSprite(spriteName, frame.spriteId, spriteSize.x, spriteSize.y);
                def.ConstructOffsetsFromAnchor(anchor, def.position3);
                def.MakeOffset(manualOffset);
                def.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                def.material.SetFloat("_EmissiveColorPower", emissivePower);
                def.material.SetColor("_EmissiveColor", emissiveColor);
                def.materialInst.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                def.materialInst.SetFloat("_EmissiveColorPower", emissivePower);
                def.materialInst.SetColor("_EmissiveColor", emissiveColor);
                frame.spriteCollection = VFXCollection;
                clip.frames = clip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
            }
            sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
            sprite.renderer.material.SetFloat("_EmissiveColorPower", emissivePowers[0]);
            sprite.renderer.material.SetColor("_EmissiveColor", emissiveColors[0]);
            clip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            clip.name = "start";
            animator.spriteAnimator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
            animator.spriteAnimator.Library.clips = new tk2dSpriteAnimationClip[] { clip };
            animator.spriteAnimator.Library.enabled = true;
            SpriteAnimatorKiller kill = animator.gameObject.AddComponent<SpriteAnimatorKiller>();
            kill.fadeTime = -1f;
            kill.animator = animator;
            kill.delayDestructionTime = -1f;
            vfObj.orphaned = orphaned;
            vfObj.attached = attached;
            vfObj.persistsOnDeath = persistsOnDeath;
            vfObj.usesZHeight = usesZHeight;
            vfObj.zHeight = zHeight;
            vfObj.alignment = alignment;
            vfObj.destructible = destructible;
            vfObj.effect = obj;
            complex.effects = new VFXObject[] { vfObj };
            pool.effects = new VFXComplex[] { complex };
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("start");
            return pool;
        }
        public static tk2dSpriteDefinition SetupDefinitionForShellSprite(string name, int id, int pixelWidth, int pixelHeight, tk2dSpriteDefinition overrideToCopyFrom = null)
        {
            float thing = 14;
            float trueWidth = (float)pixelWidth / thing;
            float trueHeight = (float)pixelHeight / thing;
            tk2dSpriteDefinition def = overrideToCopyFrom ?? VFXCollection.inst.spriteDefinitions[(PickupObjectDatabase.GetById(202) as Gun).shellCasing.GetComponent<tk2dBaseSprite>().spriteId].CopyDefinitionFrom();
            def.boundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
            def.boundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
            def.untrimmedBoundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
            def.untrimmedBoundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
            def.position0 = new Vector3(0f, 0f, 0f);
            def.position1 = new Vector3(0f + trueWidth, 0f, 0f);
            def.position2 = new Vector3(0f, 0f + trueHeight, 0f);
            def.position3 = new Vector3(0f + trueWidth, 0f + trueHeight, 0f);
            def.name = name;
            VFXCollection.spriteDefinitions[id] = def;
            return def;
        }
        public static tk2dSpriteCollectionData VFXCollection
        {
            get
            {
                return (PickupObjectDatabase.GetById(95) as Gun).clipObject.GetComponent<tk2dBaseSprite>().Collection;
            }
        }
        public static void ConstructOffsetsFromAnchor(this tk2dSpriteDefinition def, tk2dBaseSprite.Anchor anchor, Vector2? scale = null, bool fixesScale = false, bool changesCollider = true)
        {
            if (!scale.HasValue)
            {
                scale = new Vector2?(def.position3);
            }
            if (fixesScale)
            {
                Vector2 fixedScale = scale.Value - def.position0.XY();
                scale = new Vector2?(fixedScale);
            }
            float xOffset = 0;
            if (anchor == tk2dBaseSprite.Anchor.LowerCenter || anchor == tk2dBaseSprite.Anchor.MiddleCenter || anchor == tk2dBaseSprite.Anchor.UpperCenter)
            {
                xOffset = -(scale.Value.x / 2f);
            }
            else if (anchor == tk2dBaseSprite.Anchor.LowerRight || anchor == tk2dBaseSprite.Anchor.MiddleRight || anchor == tk2dBaseSprite.Anchor.UpperRight)
            {
                xOffset = -scale.Value.x;
            }
            float yOffset = 0;
            if (anchor == tk2dBaseSprite.Anchor.MiddleLeft || anchor == tk2dBaseSprite.Anchor.MiddleCenter || anchor == tk2dBaseSprite.Anchor.MiddleLeft)
            {
                yOffset = -(scale.Value.y / 2f);
            }
            else if (anchor == tk2dBaseSprite.Anchor.UpperLeft || anchor == tk2dBaseSprite.Anchor.UpperCenter || anchor == tk2dBaseSprite.Anchor.UpperRight)
            {
                yOffset = -scale.Value.y;
            }
            def.MakeOffset(new Vector2(xOffset, yOffset), changesCollider);
        }
        public static void MakeOffset(this tk2dSpriteDefinition def, Vector2 offset, bool changesCollider = false)
        {
            float xOffset = offset.x;
            float yOffset = offset.y;
            def.position0 += new Vector3(xOffset, yOffset, 0);
            def.position1 += new Vector3(xOffset, yOffset, 0);
            def.position2 += new Vector3(xOffset, yOffset, 0);
            def.position3 += new Vector3(xOffset, yOffset, 0);
            def.boundsDataCenter += new Vector3(xOffset, yOffset, 0);
            def.boundsDataExtents += new Vector3(xOffset, yOffset, 0);
            def.untrimmedBoundsDataCenter += new Vector3(xOffset, yOffset, 0);
            def.untrimmedBoundsDataExtents += new Vector3(xOffset, yOffset, 0);
            if (def.colliderVertices != null && def.colliderVertices.Length > 0 && changesCollider)
            {
                def.colliderVertices[0] += new Vector3(xOffset, yOffset, 0);
            }
        }

    }
}
