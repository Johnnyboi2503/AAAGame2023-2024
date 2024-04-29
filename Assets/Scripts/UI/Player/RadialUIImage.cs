using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialUIImage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform image;
    [SerializeField] RectTransform pivot;

    float value;
    float minAngle;
    float maxAngle;
    float displacement;

    // Setting the value across the radial gague as a percentage
    public void SetValue(float _value) {
        value = Mathf.Clamp(_value, 0, 1); // Clamping value as percentage
        UpdateImage();
    }


    // Setting the starting and end points based on inputted angle
    public void SetMinMaxAngle(float min, float max) {
        minAngle = min % 360f; 
        maxAngle = max % 360f;
        UpdateImage();
    }

    // Setting the displacement distance from the pivot point
    public void SetDisplacement(float _displacement) {
        displacement = _displacement;
        UpdateImage();
    }

    // Updating the image basedd on the values set
    private void UpdateImage() {
        float angle = Mathf.Lerp(minAngle, maxAngle, value);

        Vector2 direction = MyMath.RotateAngle(Vector2.down, 360f-angle);

        Vector2 newPos = (direction * displacement) + new Vector2(pivot.localPosition.x, pivot.localPosition.y);

        image.localPosition = newPos;
        image.anchoredPosition = newPos;
    }
}
