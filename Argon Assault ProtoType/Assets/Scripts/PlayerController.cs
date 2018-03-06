using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


//TODO: Merge Y Range into singular Y Range instead of positive and negative 
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [Tooltip("In ms^-1")]
    [SerializeField]
    float xSpeed = 4f;
    [Tooltip("In ms^-1")] [SerializeField] float ySpeed = 4f;
    [Tooltip("Sets the X Range in m from center")] [SerializeField] float xClampRange = 10f;
    [Tooltip("Sets the -Y Range in m from center")] [SerializeField] float yMinClampRange = 10f;
    [Tooltip("Sets the Y Range in m from center")] [SerializeField] float yMaxClampRange = 10f;

    [Header("Screen-poition based")]
    [Tooltip("Changes how much the ship pitches based on its Y position")]
    [SerializeField]
    float positionPitchFactor = -5f;
    [Tooltip("Changes how much the ship yaws based on its X position")] [SerializeField] float positionYawFactor = 5f;

    [Header("Control-throw based")]
    [Tooltip("Changes how much the ship pitches while moving in Y")]
    [SerializeField]
    float controlPitchFactor = -5f;

    [Tooltip("Changes how much the ship rolls while moving in X")] [SerializeField] float controlRollFactor = 5f;

    float xThrow, yThrow;
    bool isControlEnabled = true;

    // Update is called once per frame
    void Update()
    {
        FindIsControlEnabled();
    }

    void FindIsControlEnabled()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
        }
    }

    void OnPlayerDeath() // called by string refference
    {
        isControlEnabled = false;
    }

    private void ProcessRotation()
    {
        float pitch = PitchRotation();
        float yaw = YawRotation();
        float roll = RollRotation();
        ShipRotation(pitch, yaw, roll);
    }

    private void ShipRotation(float pitch, float yaw, float roll)
    {
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private float RollRotation()
    {
        return xThrow * controlRollFactor;
    }

    private float YawRotation()
    {
        return transform.localPosition.x * positionYawFactor;
    }

    private float PitchRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        return pitch;
    }

    private void ProcessTranslation()
    {
        float clampedXPos = XPosMovement();
        float clampedYPos = YPosMovement();
        XYShipTransform(clampedXPos, clampedYPos);
    }

    private float XPosMovement()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float xOffset = xThrow * xSpeed * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xClampRange, xClampRange);
        return clampedXPos;
    }

    private float YPosMovement()
    {
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");
        float yOffset = yThrow * ySpeed * Time.deltaTime;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, yMinClampRange, yMaxClampRange);
        return clampedYPos;
    }

    private void XYShipTransform(float clampedXPos, float clampedYPos)
    {
        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }
}
