using UnityEngine;

public class Lvl : MonoBehaviour
{
    public static float nature;
    public static float water;
    public static float people;

    public static float time;

    private void Update()
    {
        
    }

    void Step()
    {

        time += Time.deltaTime;
    }
}
