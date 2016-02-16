﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public static class SpriteAnimationClipCreatorExample
{
    [MenuItem("Assets/CharacterAnimatorCreator/Example/Create AnimationClip")]
    public static void CreateAnimationClip()
    {
        IEnumerable<Sprite> sprites = Selection.objects.OfType<Sprite>();
        if (!sprites.Any())
        {
            Debug.LogWarning("Please selecting sprites.");
            return;
        }

        List<SpriteKeyframeDefinition> spriteKeyframes = sprites
            .OrderBy(it => it.name)
            .Select((Sprite sprite, int index) => new SpriteKeyframeDefinition
            {
                Value = sprite,
                Time = 0.1F * index,
            })
            .ToList();

        SpriteAnimationClipDefinition definition = new SpriteAnimationClipDefinition
        {
            SpriteKeyframes = spriteKeyframes,
            WrapMode = WrapMode.Loop,
            IsLoop = true,
            FrameRate = 60.0F,
            ResulutPath = "Assets/Sample.anim",
        };

        SpriteAnimationClipCreator.CreateAnimationClip(definition);
    }
}
