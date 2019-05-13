using UnityEngine;
using System.Collections;
using Design;



public class FlyCamera : MonoBehaviour
{
    public float mainSpeed = 100.0f; //regular speed
    public float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    public float maxShift = 1000.0f; //Maximum speed when holdin gshift
    public float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;
    public Transform other;
    public Transform earth;
    public string timerFormatted = "0";
    public float secondsCount = 90F;
    public string missionLevel02Text1 = "Move to blue glow nearby any planet and make photo",
        missionLevel02Text2 = "Move to another planet blue glow and make one more photo",
        missionLevel02Text3 = "Move to Earth now";
    public int counterPlanetPhoto = 0;
    public float start = 0;
    public Texture2D button_blank = null,
    button_blue = null,
    button_blue_hover = null;
    public bool gameLoose = false;
    public bool gameWin = false;
    void OnGUI()
    {
     
        //Timer
        GUI.Label(new Rect((Screen.width / 2) - 20, 10, Screen.width / 2, 40), "MISSION DURATION", design.StyleText(design.Font_Futura, 18, TextAnchor.MiddleRight, Color.white));
        GUI.Label(new Rect((Screen.width / 2) - 20, 35, Screen.width / 2, 40), timerFormatted, design.StyleText(design.Font_Futura, 18, TextAnchor.MiddleRight, Color.white));
        // ----

        //Mission objective
        if (counterPlanetPhoto == 0)
        {
            design.DrawRectangle(new Rect(Screen.width / 2, 75, Screen.width / 2, 100), new Color(22 / 255.0f, 77 / 255.0f, 159 / 255.0f, 1f), 0.8f);
            GUI.Label(new Rect((Screen.width / 2) + 20, 90, Screen.width / 2, 40), "NEW OBJECTIVE", design.StyleText(design.Font_Futura, 30, TextAnchor.MiddleLeft, Color.white));
            GUI.Label(new Rect((Screen.width / 2) + 20, 125, Screen.width / 2, 30), missionLevel02Text1, design.StyleText(design.Font_Futura, 18, TextAnchor.MiddleLeft, Color.white));

        }
        if (counterPlanetPhoto == 1)
        {
            
            if (start - secondsCount <= 2)
            {
                design.DrawRectangle(new Rect(Screen.width / 2, 85, Screen.width / 2, 50), new Color(41 / 255.0f, 159 / 255.0f, 22 / 255.0f, 1f), 0.8f);
            GUI.Label(new Rect((Screen.width / 2) + 20, 90, Screen.width / 2, 40), "OBJECTIVE COMPLETED!", design.StyleText(design.Font_Futura, 30, TextAnchor.MiddleLeft, Color.white));
               
            }
            if (start - secondsCount > 2)
            {
                design.DrawRectangle(new Rect(Screen.width / 2, 75, Screen.width / 2, 100), new Color(22 / 255.0f, 77 / 255.0f, 159 / 255.0f, 1f), 0.8f);
            GUI.Label(new Rect((Screen.width / 2) + 20, 90, Screen.width / 2, 40), "NEW OBJECTIVE", design.StyleText(design.Font_Futura, 30, TextAnchor.MiddleLeft, Color.white));
            GUI.Label(new Rect((Screen.width / 2) + 20, 125, Screen.width / 2, 30), missionLevel02Text2, design.StyleText(design.Font_Futura, 18, TextAnchor.MiddleLeft, Color.white));

            }
                    
            
        }
        if (counterPlanetPhoto == 2)
        {

            if (start - secondsCount <= 2)
            {
                design.DrawRectangle(new Rect(Screen.width / 2, 85, Screen.width / 2, 50), new Color(41 / 255.0f, 159 / 255.0f, 22 / 255.0f, 1f), 0.8f);
                GUI.Label(new Rect((Screen.width / 2) + 20, 90, Screen.width / 2, 40), "OBJECTIVE COMPLETED!", design.StyleText(design.Font_Futura, 30, TextAnchor.MiddleLeft, Color.white));

            }
            if (start - secondsCount > 2)
            {
                design.DrawRectangle(new Rect(Screen.width / 2, 75, Screen.width / 2, 100), new Color(22 / 255.0f, 77 / 255.0f, 159 / 255.0f, 1f), 0.8f);
                GUI.Label(new Rect((Screen.width / 2) + 20, 90, Screen.width / 2, 40), "NEW OBJECTIVE", design.StyleText(design.Font_Futura, 30, TextAnchor.MiddleLeft, Color.white));
                GUI.Label(new Rect((Screen.width / 2) + 20, 125, Screen.width / 2, 30), missionLevel02Text3, design.StyleText(design.Font_Futura, 18, TextAnchor.MiddleLeft, Color.white));
            }
        }
        if (counterPlanetPhoto >= 3) design.MissionSuccess(design.Font_Futura, button_blank, button_blue, button_blue_hover);
        // ----

        //Mission abort
        if (gameLoose == true)
        {            
            design.MissionAbort(design.Font_Futura, button_blank, button_blue, button_blue_hover);
        }
        // ----

        //Mission success
        //if (gameWin == true)
        //{
        //    design.MissionAbort(design.Font_Futura, button_blank, button_blue, button_blue_hover);
        //}
        // ----

    }


    void Update()
    {
        if (secondsCount <= 0)
        {
            gameLoose = true;
            Screen.lockCursor = false;
        }
        else if (gameWin == true)
        {
            Screen.lockCursor = false;
        }
        else
        {
            Debug.Log("Screen locked");
            Screen.lockCursor = true;
        }
        
        //Mouse camera rotation

        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(-v, h, 0);


        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }
        //taking picture of planet
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.collider != null)
                {
                    other = hit.collider.transform.Find("guide_here");
                    Debug.Log(Vector3.Distance(other.position, transform.position));

                    if (Vector3.Distance(other.position, transform.position) < 4)
                    {
                        //isCameraFLASH = true;
                        hit.collider.enabled = false;
                        counterPlanetPhoto += 1;
                        start = secondsCount;
                        Debug.Log(hit.collider.transform.Find("guide_here"));
                    }
                    
                }
            }
        }
        if (counterPlanetPhoto == 2 && Vector3.Distance(earth.position, transform.position) < 4)
        {
            gameWin = true;
            counterPlanetPhoto += 1;
            Debug.Log("Game Win");
        }

        if (!gameWin && !gameLoose) secondsCount -= Time.deltaTime;
        timerFormatted = Mathf.RoundToInt(secondsCount).ToString();


    }
    //WASD input
    private Vector3 GetBaseInput()
    { 
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
        
    }
    
}