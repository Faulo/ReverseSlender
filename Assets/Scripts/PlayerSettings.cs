﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Settings", menuName = "Gameplay/Player Settings", order = 1)]
public class PlayerSettings : ScriptableObject {
    [Header("Movement")]
    [Range(0, 20), Tooltip("Movement speed while ghost.")]
    public float moveSpeed = 7;
    [Range(0, 1), Tooltip("Movement speed while monster.")]
    public float scareMoveSpeedModifier = .5f;
    [Range(0, 100)]
    public float maxDistanceAboveGround = 6f;
    [Range(0, 100)]
    public float minDistanceAboveGround = 3f;

    [Header("VFX")]
    [Range(.01f, .25f), Tooltip("How quickly something something ghost-form.")]
    public float ghostVFXLerpSpeed = 0.015f;
    [Range(.01f, .25f), Tooltip("How quickly something something monster-form.")]
    public float bigMonstaLerpSpeed = 0.05f;
}
