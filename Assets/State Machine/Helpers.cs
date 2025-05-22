using UnityEngine;

public static class Helpers {

    //map a value from one range to another
    public static float Map(float value, float originalMin, float originalMax, float newMin, float newMax, bool clamp) {
        float newValue = (value - originalMin) / (originalMax - originalMin) * (newMax - newMin) + newMin;
        if (clamp) {
            newValue = Mathf.Clamp(newValue, newMin, newMax);
        }
        return newValue;
    }
}