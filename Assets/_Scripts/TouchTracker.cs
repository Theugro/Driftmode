using UnityEngine;
using System.Collections;

public class TouchTracker : MonoBehaviour
{

    public Controller playerController;
    Vector2[] startPos = new Vector2[10];
    GameObject[] joystick = new GameObject[10];
    Vector3[] joystickPos = new Vector3[10];
    public GameObject indicator;

    /*var touchPos = Input.GetTouch(i).position;
                var createPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10));
                Instantiate(indicator, createPos, Quaternion.identity);*/

    void Update()
    {

        GameObject[] joysticks = GameObject.FindGameObjectsWithTag("NotDestroy");
        for (int i = 0; i < joysticks.Length; i++)
        {
            joysticks[i].tag = "Destroy";
        }

        Touch[] myTouches = Input.touches;

        float speed = 0, turn = 0;

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (myTouches[i].phase == TouchPhase.Began) //First frame the touch is created
            {
                startPos[i] = myTouches[i].position;

                //instantiating a joystick
                joystickPos[i] = startPos[i];
                joystickPos[i].z = 5;
                Vector3 objPos = Camera.main.ScreenToWorldPoint(joystickPos[i]);
                joystick[i] = Instantiate(indicator, objPos, Quaternion.identity) as GameObject;
                joystick[i].tag = "NotDestroy";
            }

            else if (myTouches[i].phase == TouchPhase.Moved || myTouches[i].phase == TouchPhase.Stationary) //Frames between beginning and end
            {
                Vector3 objPos;
                if (startPos[i].x <= Screen.width / 2) // if touch is on left side of the screen
                {
                    float deltaPosY = myTouches[i].position.y - startPos[i].y;
                    speed = deltaPosY / 400;
                    Mathf.Clamp(speed, -1, 1);
                    objPos = Camera.main.ScreenToWorldPoint(new Vector3(startPos[i].x, startPos[i].y, 5) + new Vector3(0, speed * 100, 0));
                }

                else if (startPos[i].x > Screen.width / 2)
                {
                    float deltaPosX = myTouches[i].position.x - startPos[i].x;
                    turn = deltaPosX / 300;
                    Mathf.Clamp(turn, -1, 1);
                    objPos = Camera.main.ScreenToWorldPoint(new Vector3(startPos[i].x, startPos[i].y, 5) + new Vector3(turn * 40, 0, 0));
                }
                else { objPos = Vector3.zero; }

                joystick[i].transform.position = objPos;
                joystick[i].tag = "NotDestroy";

            }

            else if (myTouches[i].phase == TouchPhase.Ended || myTouches[i].phase == TouchPhase.Canceled) //Ending touches and reseting their start points
            {
                if (startPos[i].x <= Screen.width / 2)
                    speed = 0;

                if (startPos[i].x > Screen.width / 2)
                    turn = 0;

                joystick[i].tag = "Destroy";

                if (Input.touchCount > i + 1)
                {
                    // replace startPos values with all touches of lower i value
                    for (int j = 0; j < startPos.Length; j++)
                    {
                        if (j < startPos.Length)
                        {
                            startPos[j] = startPos[j + 1];
                            joystick[j] = joystick[j + 1];
                        }
                        else
                        {
                            startPos[j] = new Vector2(-1, 0);
                        }
                    }
                }
            }
        }

        joysticks = GameObject.FindGameObjectsWithTag("Destroy");
        for (int j = 0; j < joysticks.Length; j++)
        {
            Destroy(joysticks[j]);
        }

        playerController.go(speed);
        playerController.turn(turn);
    }
}