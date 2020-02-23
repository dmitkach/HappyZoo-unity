using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerakBehavior : MonoBehaviour
{
    public AudioSource backaudio, sound, losesound, endsound;
    public GameObject round, rect, close, triangle;
    public GameObject plround, plrect, pltriangle, plclose;
    public GameObject LoseTextObj1, LoseTextObj2, StartTextObj, StatTextObj;
    
    public Text ResText;
    public Text LoseText1, LoseText2, StartText, StatText;
    public double Result1 = 0, High1 = 0, Result2 = 0, High2 = 0, Result3 = 0, High3 = 0, time = -1;
    public bool LoseFlag = false, StartFlag = false, IsLose = false; 
    public Camera cam;
    int[] Status = new int[5];
    int[] Color = new int[5];
    private int mode;
    
    enum Shape
    {
        triangle,
        rect,
        close,
        round
    };

    Vector3 DefPos = new Vector3(0, -100, 0);
    Vector3 PlayerPos = new Vector3(0, -1, 10);
    Vector3 Pos0 = new Vector3(-8, -1, 10);
    Vector3 Pos1 = new Vector3(0, 2.8f, 10);
    Vector3 Pos2 = new Vector3(8, -1, 10);
    Vector3 Pos3 = new Vector3(0, -4.7f, 10);
    private int[] positions = new int[4];

    // Start is called before the first frame update
    public int GetRandom()
    {
        DateTime now = DateTime.Now;

        //Создание объекта для генерации чисел (с указанием начального значения)
        System.Random rnd = new System.Random(now.Second * 10 + now.Millisecond);
 
        //Получить случайное число 
        int value = rnd.Next();
 
        //Вернуть полученное значение
        return value;
    }

    Vector3 IntToVec(int n)
    {
        if (n == 0)
            return Pos0;
        if (n == 1)
            return Pos1;
        if (n == 2)
            return Pos2;
        else
            return Pos3;
    }
    
    void RandomArray()
    {
        DateTime now = DateTime.Now;

        System.Random rnd = new System.Random(now.Millisecond - now.Second);
        positions[0] = rnd.Next(0, 3);
        positions[1] = rnd.Next(0, 3);
        if (positions[0] == positions[1])
            positions[1] = (positions[1] + 1) % 4;
        positions[2] = rnd.Next(0, 3);
        while (positions[2] == positions[0] || positions[2] == positions[1])
            positions[2] = (positions[2] + 1) % 4;
        positions[3] = rnd.Next(0, 3);
        while (positions[3] == positions[2] || positions[3] == positions[1] || positions[3] == positions[0])
            positions[3] = (positions[3] + 1) % 4;
        // while (positions[0] == positions[1])
        //     positions[1] = rnd.Next(0, 3);
        // positions[2] = rnd.Next(0, 3);
        // while (positions[2] == positions[1] || posfitions[2] == positions[0])
        //     positions[2] = rnd.Next(0, 3);
        // positions[3] = rnd.Next(0, 3);
        // while (positions[3] == positions[2] || positions[3] == positions[1] || positions[3] == positions[0])
        //     positions[3] = rnd.Next(0, 3);
    }
    
    void RandomArrayColor()
    {
        DateTime now = DateTime.Now;

        System.Random rnd = new System.Random(now.Millisecond - now.Second);
        Color[0] = rnd.Next(0, 3);
        Color[1] = rnd.Next(0, 3);
        if (Color[0] == Color[1])
            Color[1] = (Color[1] + 1) % 4;
        Color[2] = rnd.Next(0, 3);
        while (Color[2] == Color[0] || Color[2] == Color[1])
            Color[2] = (Color[2] + 1) % 4;
        Color[3] = rnd.Next(0, 3);
        while (Color[3] == Color[2] || Color[3] == Color[1] || Color[3] == Color[0])
            Color[3] = (Color[3] + 1) % 4;
    }

    void ResetPlayerPos()
    {
        plrect.transform.position = DefPos;
        pltriangle.transform.position = DefPos;
        plround.transform.position = DefPos;
        plclose.transform.position = DefPos;
        DateTime now = DateTime.Now;

        System.Random rnd = new System.Random(now.Millisecond - now.Second);
        int value = rnd.Next(0, 4);
        if (value == 0)
        {
            plrect.transform.position = PlayerPos;
            Status[4] = (int) Shape.rect;
        }
        else if (value == 1)
        {
            pltriangle.transform.position = PlayerPos;
            Status[4] = (int) Shape.triangle;
        }
        else if (value == 2)
        {
            plround.transform.position = PlayerPos;
            Status[4] = (int) Shape.round;
        }
        else if (value == 3)
        {
            plclose.transform.position = PlayerPos;
            Status[4] = (int) Shape.close;
        }

    }

    Vector4 IntToVec4(int n)
    {
        if (n == 0)
            return UnityEngine.Color.magenta;
        else if (n == 1)
            return UnityEngine.Color.cyan;
        else if (n == 2)
            return UnityEngine.Color.red;
        else
            return UnityEngine.Color.green;
    }

    void Start()
    {
        cam.depthTextureMode = DepthTextureMode.None;
        close = GameObject.Find("Close");
        rect = GameObject.Find("Rect");
        triangle = GameObject.Find("Triangle");
        round = GameObject.Find("Round");
        plround = GameObject.Find("PlRound");
        pltriangle = GameObject.Find("PlTriangle");
        plrect = GameObject.Find("PlRect");
        plclose = GameObject.Find("PlClose");
        ResText = GameObject.Find("Canvas/ResText").GetComponent<Text>();
        StatText = GameObject.Find("Canvas/Stats").GetComponent<Text>();
        LoseTextObj1 = GameObject.Find("Canvas/LoseText");
        LoseTextObj2 = GameObject.Find("Canvas/AnyKey_Lose");
        StartTextObj = GameObject.Find("Canvas/Start text");
        StatTextObj = GameObject.Find("Canvas/Stats");
        LoseText1 = LoseTextObj1.GetComponent<Text>();
        LoseText2 = LoseTextObj2.GetComponent<Text>();
        LoseText1.text = "";
        LoseText2.text = "";
        RandomArray();
        RandomArrayColor();

        rect.transform.position = DefPos;
        triangle.transform.position = DefPos;
        close.transform.position = DefPos;
        round.transform.position = DefPos;
        plrect.transform.position = DefPos;
        plclose.transform.position = DefPos;
        pltriangle.transform.position = DefPos;
        plround.transform.position = DefPos;
        backaudio.Play();
        
        StatTextObj.transform.position = DefPos;
    }

    void LoseScreen(bool LoseKind)
    { 
        if (LoseKind == false)
        {
            if (mode == 1)
            {
                if (time == -1)
                    time = Time.realtimeSinceStartup;
                ResText.text = "Best: " + (int) High1 + "\nCurrent: " + (int) Result1 + "\nTime:" + (int) (30 - Time.realtimeSinceStartup + time);
                if ((Input.GetKeyDown(KeyCode.LeftArrow) && Status[0] == Status[4])
                     || (Input.GetKeyDown(KeyCode.DownArrow) && Status[3] == Status[4])
                     || (Input.GetKeyDown(KeyCode.UpArrow) && Status[1] == Status[4])
                     || (Input.GetKeyDown(KeyCode.RightArrow) && Status[2] == Status[4])
                     && Time.realtimeSinceStartup - time < 30)
                {
                    Result1++;
                    if (Result1 > High1) 
                        High1 = Result1;
                    ResText.text = "Best: " + (int) High1 + "\nCurrent: " + (int) Result1 + "\nTime:" + (int) (30 - Time.realtimeSinceStartup + time);
                    RandomArray();

                    triangle.transform.position = IntToVec(positions[0]);
                    Status[positions[0]] = (int) Shape.triangle;
                    round.transform.position = IntToVec(positions[1]);
                    Status[positions[1]] = (int) Shape.round;
                    rect.transform.position = IntToVec(positions[2]);
                    Status[positions[2]] = (int) Shape.rect;
                    close.transform.position = IntToVec(positions[3]);
                    Status[positions[3]] = (int) Shape.close;
                    
                    sound.Play();
                    ResetPlayerPos();
                }
                else if (Time.realtimeSinceStartup - time >= 30)
                {
                    Result1 = 0;
                    triangle.transform.position = DefPos;
                    round.transform.position = DefPos;
                    rect.transform.position = DefPos;
                    close.transform.position = DefPos;
                    pltriangle.transform.position = DefPos;
                    plround.transform.position = DefPos;
                    plrect.transform.position = DefPos;
                    plclose.transform.position = DefPos;
                    StartFlag = !StartFlag;
                    StartTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    endsound.Play();
                    time = -1;
                    mode = 0;
                    ResText.text = "";
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Result1 = 0;
                    sound.Play();
                    triangle.transform.position = DefPos;
                    round.transform.position = DefPos;
                    rect.transform.position = DefPos;
                    close.transform.position = DefPos;
                    pltriangle.transform.position = DefPos;
                    plround.transform.position = DefPos;
                    plrect.transform.position = DefPos;
                    plclose.transform.position = DefPos;
                    StartFlag = !StartFlag;
                    StatTextObj.transform.position = DefPos;
                    StartTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    mode = 0;
                    time = -1;
                    ResText.text = "";
                }
            }

            if (mode == 2)
            {
                if (time == -1)
                    time = Time.realtimeSinceStartup;
                ResText.text = "Best: " + (int) High2 + "\nLast: " + (int) (45 - Result2) + "\nTime:" +
                               (int) (Time.realtimeSinceStartup - time);

                if ((Input.GetKeyDown(KeyCode.LeftArrow) && Status[0] == Status[4])
                    || (Input.GetKeyDown(KeyCode.DownArrow) && Status[3] == Status[4])
                    || (Input.GetKeyDown(KeyCode.UpArrow) && Status[1] == Status[4])
                    || (Input.GetKeyDown(KeyCode.RightArrow) && Status[2] == Status[4])
                    && Result2 < 45)
                {
                    Result2++;
                    RandomArray();

                    ResText.text = "Best: " + (int) High2 + "\nLast: " + (int) (45 - Result2) + "\nTime:" +
                                   (int) (Time.realtimeSinceStartup - time);

                    triangle.transform.position = IntToVec(positions[0]);
                    Status[positions[0]] = (int) Shape.triangle;
                    round.transform.position = IntToVec(positions[1]);
                    Status[positions[1]] = (int) Shape.round;
                    rect.transform.position = IntToVec(positions[2]);
                    Status[positions[2]] = (int) Shape.rect;
                    close.transform.position = IntToVec(positions[3]);
                    Status[positions[3]] = (int) Shape.close;

                    sound.Play();
                    ResetPlayerPos();
                }
                else if (Result2 >= 45)
                {
                    Result2 = 0;
                    triangle.transform.position = DefPos;
                    round.transform.position = DefPos;
                    rect.transform.position = DefPos;
                    close.transform.position = DefPos;
                    pltriangle.transform.position = DefPos;
                    plround.transform.position = DefPos;
                    plrect.transform.position = DefPos;
                    plclose.transform.position = DefPos;
                    StartFlag = !StartFlag;
                    StartTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    if ((int) (Time.realtimeSinceStartup - time) < High2 || High2 == 0)
                        High2 = (int) (Time.realtimeSinceStartup - time);
                    endsound.Play();
                    time = -1;
                    mode = 0;
                    ResText.text = "";
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Result2 = 0;
                    sound.Play();
                    triangle.transform.position = DefPos;
                    round.transform.position = DefPos;
                    rect.transform.position = DefPos;
                    close.transform.position = DefPos;
                    pltriangle.transform.position = DefPos;
                    plround.transform.position = DefPos;
                    plrect.transform.position = DefPos;
                    plclose.transform.position = DefPos;
                    StartFlag = !StartFlag;
                    StatTextObj.transform.position = DefPos;
                    StartTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    mode = 0;
                    time = -1;
                    ResText.text = "";
                }
            }

            if (mode == 3)
            {
                ResText.text = "Best: " + (int) High3 + "\nCurrent: " + Result3;
                if ((Input.GetKeyDown(KeyCode.LeftArrow) && Status[0] == Status[4])
                    || (Input.GetKeyDown(KeyCode.DownArrow) && Status[3] == Status[4])
                    || (Input.GetKeyDown(KeyCode.UpArrow) && Status[1] == Status[4])
                    || (Input.GetKeyDown(KeyCode.RightArrow) && Status[2] == Status[4])
                    && !IsLose)
                {
                    Result3++;
                    if (Result3 > High3)
                        High3 = Result3;
                    ResText.text = "Best: " + (int) High3 + "\nCurrent: " + (int) Result3;
                    RandomArray();

                    triangle.transform.position = IntToVec(positions[0]);
                    Status[positions[0]] = (int) Shape.triangle;
                    round.transform.position = IntToVec(positions[1]);
                    Status[positions[1]] = (int) Shape.round;
                    rect.transform.position = IntToVec(positions[2]);
                    Status[positions[2]] = (int) Shape.rect;
                    close.transform.position = IntToVec(positions[3]);
                    Status[positions[3]] = (int) Shape.close;

                    sound.Play();
                    ResetPlayerPos();
                }
                else if ((Input.GetKeyDown(KeyCode.LeftArrow))
                         || (Input.GetKeyDown(KeyCode.DownArrow))
                         || (Input.GetKeyDown(KeyCode.UpArrow))
                         || (Input.GetKeyDown(KeyCode.RightArrow))
                         || IsLose)
                {
                    Result3 = 0;
                    triangle.transform.position = DefPos;
                    round.transform.position = DefPos;
                    rect.transform.position = DefPos;
                    close.transform.position = DefPos;
                    pltriangle.transform.position = DefPos;
                    plround.transform.position = DefPos;
                    plrect.transform.position = DefPos;
                    plclose.transform.position = DefPos;
                    LoseTextObj1.transform.position = new Vector3(Screen.width / 2, Screen.height / 2 + 5, 0);
                    LoseTextObj2.transform.position = new Vector3(Screen.width / 2, Screen.height / 2 - 5, 0);
                    LoseText1.text = "You lose!";
                    LoseText2.text = "Click 'SPACE' to restart the game.";
                    StartFlag = !StartFlag;
                    StartTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    mode = 0;
                    losesound.Play();
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Result3 = 0;
                    sound.Play();
                    triangle.transform.position = DefPos;
                    round.transform.position = DefPos;
                    rect.transform.position = DefPos;
                    close.transform.position = DefPos;
                    pltriangle.transform.position = DefPos;
                    plround.transform.position = DefPos;
                    plrect.transform.position = DefPos;
                    plclose.transform.position = DefPos;
                    StartFlag = !StartFlag;
                    StatTextObj.transform.position = DefPos;
                    StartTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    mode = 0;
                    time = -1;
                    ResText.text = "";
                }
            }
        
            if (mode == 4)
            {
                //backaudio.Play();
                triangle.transform.position = DefPos;
                round.transform.position = DefPos;
                rect.transform.position = DefPos;
                close.transform.position = DefPos;
                pltriangle.transform.position = DefPos;
                plround.transform.position = DefPos;
                plrect.transform.position = DefPos;
                plclose.transform.position = DefPos;
                StatTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                StatText.text = "'1' - best: " + High1 + " points.\n'2' - best: " + High2 + " seconds.\n'3' - best: " +
                                High3 + " points.";
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    sound.Play(); 
                    StartFlag = !StartFlag;
                    StatTextObj.transform.position = DefPos;
                    StartTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    mode = 0;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (backaudio.isPlaying)
                    backaudio.Pause();
                else
                    backaudio.Play();
            }

            LoseText1.text = "";
            LoseText2.text = "";
        }
        else
        {
            LoseText1.text = "You lose!";
            LoseText2.text = "Click 'SPACE' to restart the game.";
            triangle.transform.position = DefPos;
            round.transform.position = DefPos;
            rect.transform.position = DefPos;
            close.transform.position = DefPos;
            pltriangle.transform.position = DefPos;
            plround.transform.position = DefPos;
            plrect.transform.position = DefPos;
            plclose.transform.position = DefPos;
        }
    }

    // Update is called every frame
    void Update()
    {
        if (!StartFlag)
            if (!backaudio.isPlaying)
                backaudio.Play();
        if (Input.GetKeyDown(KeyCode.Escape) && !StartFlag)
            Application.Quit();
        if (!StartFlag && Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartFlag = true;
            StartTextObj.transform.position = new Vector3(-1000, 0, 0);
            RandomArray();
            mode = 1;        
            triangle.transform.position = IntToVec(positions[0]);
            Status[positions[0]] = (int) Shape.triangle;
            round.transform.position = IntToVec(positions[1]);
            Status[positions[1]] = (int) Shape.round;
            rect.transform.position = IntToVec(positions[2]);
            Status[positions[2]] = (int) Shape.rect;
            close.transform.position = IntToVec(positions[3]);
            Status[positions[3]] = (int) Shape.close;
            
            backaudio.Pause();
            sound.Play();
            ResetPlayerPos();
            
        }
        if (!StartFlag && Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartFlag = true;
            StartTextObj.transform.position = new Vector3(-1000, 0, 0);
            RandomArray();
            mode = 2;        
            triangle.transform.position = IntToVec(positions[0]);
            Status[positions[0]] = (int) Shape.triangle;
            round.transform.position = IntToVec(positions[1]);
            Status[positions[1]] = (int) Shape.round;
            rect.transform.position = IntToVec(positions[2]);
            Status[positions[2]] = (int) Shape.rect;
            close.transform.position = IntToVec(positions[3]);
            Status[positions[3]] = (int) Shape.close;
            
            backaudio.Pause();
            sound.Play();
            ResetPlayerPos();
            
        }
        if (!StartFlag && Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartFlag = true;
            StartTextObj.transform.position = new Vector3(-1000, 0, 0);
            RandomArray();
            mode = 3;        
            triangle.transform.position = IntToVec(positions[0]);
            Status[positions[0]] = (int) Shape.triangle;
            round.transform.position = IntToVec(positions[1]);
            Status[positions[1]] = (int) Shape.round;
            rect.transform.position = IntToVec(positions[2]);
            Status[positions[2]] = (int) Shape.rect;
            close.transform.position = IntToVec(positions[3]);
            Status[positions[3]] = (int) Shape.close;
            
            backaudio.Pause();
            sound.Play();
            ResetPlayerPos();
        }
        if (!StartFlag && Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartFlag = true;
            StartTextObj.transform.position = new Vector3(-1000, 0, 0);
            RandomArray();
            mode = 4;        
            triangle.transform.position = IntToVec(positions[0]);
            Status[positions[0]] = (int) Shape.triangle;
            round.transform.position = IntToVec(positions[1]);
            Status[positions[1]] = (int) Shape.round;
            rect.transform.position = IntToVec(positions[2]);
            Status[positions[2]] = (int) Shape.rect;
            close.transform.position = IntToVec(positions[3]);
            Status[positions[3]] = (int) Shape.close;
            
            sound.Play();
            ResetPlayerPos();
        }
        if (StartFlag)
        {
            LoseScreen(LoseFlag);
            if (Input.GetKeyDown(KeyCode.Space) && LoseFlag == true)
            {
                LoseFlag = !LoseFlag;
                RandomArray();

                triangle.transform.position = IntToVec(positions[0]);
                Status[positions[0]] = (int) Shape.triangle;
                round.transform.position = IntToVec(positions[1]);
                Status[positions[1]] = (int) Shape.round;
                rect.transform.position = IntToVec(positions[2]);
                Status[positions[2]] = (int) Shape.rect;
                close.transform.position = IntToVec(positions[3]);
                Status[positions[3]] = (int) Shape.close;

                ResetPlayerPos();
                LoseScreen(LoseFlag);
            }
        }
    }
}
