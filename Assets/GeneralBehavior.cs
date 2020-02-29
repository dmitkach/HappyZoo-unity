using System;
using UnityEngine;
using UnityEngine.UI;

public class GeneralBehavior : MonoBehaviour
{
    public AudioSource backAudio, sound, lossSound, endSound;
    public GameObject round, rect, close, triangle;
    public GameObject playerRound, playerRect, playerTriangle, playerClose;
    public GameObject loseTextObj1, loseTextObj2, startTextObj, statTextObj;
    
    public Text resText;
    public Text loseText1, loseText2, statText;
    public double result1, high1, result2, high2, result3, high3, time = -1;
    public bool loseFlag, startFlag, isLose; 
    public Camera cam;
    private readonly int[] _status = new int[5];
    // private int[] _color = new int[5];
    private int _mode;

    private enum Shape
    {
        Triangle,
        Rect,
        Close,
        Round
    };

    private readonly Vector3 _defPos = new Vector3(0, -100, 0);
    private readonly Vector3 _playerPos = new Vector3(0, -1, 10);
    private readonly Vector3 _pos0 = new Vector3(8, -1, 10);
    private readonly Vector3 _pos1 = new Vector3(0, 2.8f, 10);
    private readonly Vector3 _pos2 = new Vector3(8, -1, 10);
    private readonly Vector3 _pos3 = new Vector3(0, -4.7f, 10);
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private int[] _positions = new int[4];

    private Vector3 IntToVec(int n)
    {
        switch (n)
        {
            case 0:
                return _pos0;
            case 1:
                return _pos1;
            case 2:
                return _pos2;
            default:
                return _pos3;
        }
    }

    private void SetRandomAnimalPos()
    {
        var now = DateTime.Now;

        var rnd = new System.Random(now.Millisecond - now.Second);
        _positions[0] = rnd.Next(0, 3);
        _positions[1] = rnd.Next(0, 3);
        if (_positions[0] == _positions[1])
            _positions[1] = (_positions[1] + 1) % 4;
        _positions[2] = rnd.Next(0, 3);
        while (_positions[2] == _positions[0] || _positions[2] == _positions[1])
            _positions[2] = (_positions[2] + 1) % 4;
        _positions[3] = rnd.Next(0, 3);
        while (_positions[3] == _positions[2] || _positions[3] == _positions[1] || _positions[3] == _positions[0])
            _positions[3] = (_positions[3] + 1) % 4;
        
        triangle.transform.position = IntToVec(_positions[0]);
        _status[_positions[0]] = (int) Shape.Triangle;
        round.transform.position = IntToVec(_positions[1]);
        _status[_positions[1]] = (int) Shape.Round;
        rect.transform.position = IntToVec(_positions[2]);
        _status[_positions[2]] = (int) Shape.Rect;
        close.transform.position = IntToVec(_positions[3]);
        _status[_positions[3]] = (int) Shape.Close;
    }

    private void AllSetDefault()
    {
        triangle.transform.position = _defPos;
        round.transform.position = _defPos;
        rect.transform.position = _defPos;
        close.transform.position = _defPos;
        playerTriangle.transform.position = _defPos;
        playerRound.transform.position = _defPos;
        playerRect.transform.position = _defPos;
        playerClose.transform.position = _defPos;       
    }

    private void ResetPlayerPos()
    {
        playerRect.transform.position = _defPos;
        playerTriangle.transform.position = _defPos;
        playerRound.transform.position = _defPos;
        playerClose.transform.position = _defPos;
        var now = DateTime.Now;

        var rnd = new System.Random(now.Millisecond - now.Second);
        var value = rnd.Next(0, 4);
        switch (value)
        {
            case 0:
                playerRect.transform.position = _playerPos;
                _status[4] = (int) Shape.Rect;
                break;
            case 1:
                playerTriangle.transform.position = _playerPos;
                _status[4] = (int) Shape.Triangle;
                break;
            case 2:
                playerRound.transform.position = _playerPos;
                _status[4] = (int) Shape.Round;
                break;
            case 3:
                playerClose.transform.position = _playerPos;
                _status[4] = (int) Shape.Close;
                break;
        }

    }
    
    private void LoseScreen(bool loseKind)
    { 
        if (loseKind == false)
        {
            if (_mode == 1)
            {
                if (time == -1)
                    time = Time.realtimeSinceStartup;
                resText.text = "Best: " + (int) high1 + "\nCurrent: " + (int) result1 + "\nTime:" + (int) (30 - Time.realtimeSinceStartup + time);
                if ((Input.GetKeyDown(KeyCode.LeftArrow) && _status[0] == _status[4])
                     || (Input.GetKeyDown(KeyCode.DownArrow) && _status[3] == _status[4])
                     || (Input.GetKeyDown(KeyCode.UpArrow) && _status[1] == _status[4])
                     || (Input.GetKeyDown(KeyCode.RightArrow) && _status[2] == _status[4])
                     && Time.realtimeSinceStartup - time < 30)
                {
                    result1++;
                    if (result1 > high1) 
                        high1 = result1;
                    resText.text = "Best: " + (int) high1 + "\nCurrent: " + (int) result1 + "\nTime:" + (int) (30 - Time.realtimeSinceStartup + time);
                    SetRandomAnimalPos();

                    sound.Play();
                    ResetPlayerPos();
                }
                else if (Time.realtimeSinceStartup - time >= 30)
                {
                    result1 = 0;
                    AllSetDefault();
                    startFlag = !startFlag;
                    startTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    endSound.Play();
                    time = -1;
                    _mode = 0;
                    resText.text = "";
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    result1 = 0;
                    sound.Play();
                    AllSetDefault();
                    startFlag = !startFlag;
                    statTextObj.transform.position = _defPos;
                    startTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    _mode = 0;
                    time = -1;
                    resText.text = "";
                }
            }

            if (_mode == 2)
            {
                if (time == -1)
                    time = Time.realtimeSinceStartup;
                resText.text = "Best: " + (int) high2 + "\nLast: " + (int) (45 - result2) + "\nTime:" +
                               (int) (Time.realtimeSinceStartup - time);

                if ((Input.GetKeyDown(KeyCode.LeftArrow) && _status[0] == _status[4])
                    || (Input.GetKeyDown(KeyCode.DownArrow) && _status[3] == _status[4])
                    || (Input.GetKeyDown(KeyCode.UpArrow) && _status[1] == _status[4])
                    || (Input.GetKeyDown(KeyCode.RightArrow) && _status[2] == _status[4])
                    && result2 < 45)
                {
                    result2++;
                    SetRandomAnimalPos();

                    resText.text = "Best: " + (int) high2 + "\nLast: " + (int) (45 - result2) + "\nTime:" +
                                   (int) (Time.realtimeSinceStartup - time);

                    sound.Play();
                    ResetPlayerPos();
                }
                else if (result2 >= 45)
                {
                    result2 = 0;
                    AllSetDefault();
                    startFlag = !startFlag;
                    startTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    if ((int) (Time.realtimeSinceStartup - time) < high2 || high2 == 0)
                        high2 = (int) (Time.realtimeSinceStartup - time);
                    endSound.Play();
                    time = -1;
                    _mode = 0;
                    resText.text = "";
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    result2 = 0;
                    sound.Play();
                    AllSetDefault();
                    startFlag = !startFlag;
                    statTextObj.transform.position = _defPos;
                    startTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    _mode = 0;
                    time = -1;
                    resText.text = "";
                }
            }

            if (_mode == 3)
            {
                resText.text = "Best: " + (int) high3 + "\nCurrent: " + result3;
                if ((Input.GetKeyDown(KeyCode.LeftArrow) && _status[0] == _status[4])
                    || (Input.GetKeyDown(KeyCode.DownArrow) && _status[3] == _status[4])
                    || (Input.GetKeyDown(KeyCode.UpArrow) && _status[1] == _status[4])
                    || (Input.GetKeyDown(KeyCode.RightArrow) && _status[2] == _status[4])
                    && !isLose)
                {
                    result3++;
                    if (result3 > high3)
                        high3 = result3;
                    resText.text = "Best: " + (int) high3 + "\nCurrent: " + (int) result3;
                    SetRandomAnimalPos();
                    sound.Play();
                    ResetPlayerPos();
                }
                else if ((Input.GetKeyDown(KeyCode.LeftArrow))
                         || (Input.GetKeyDown(KeyCode.DownArrow))
                         || (Input.GetKeyDown(KeyCode.UpArrow))
                         || (Input.GetKeyDown(KeyCode.RightArrow))
                         || isLose)
                {
                    result3 = 0;
                    AllSetDefault();
                    loseTextObj1.transform.position = new Vector3(Screen.width / 2, Screen.height / 2 + 5, 0);
                    loseTextObj2.transform.position = new Vector3(Screen.width / 2, Screen.height / 2 - 5, 0);
                    loseText1.text = "You lose!";
                    loseText2.text = "Click 'SPACE' to restart the game.";
                    startFlag = !startFlag;
                    startTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    _mode = 0;
                    lossSound.Play();
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    result3 = 0;
                    sound.Play();
                    AllSetDefault();
                    startFlag = !startFlag;
                    statTextObj.transform.position = _defPos;
                    startTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    _mode = 0;
                    time = -1;
                    resText.text = "";
                }
            }
        
            if (_mode == 4)
            {
                AllSetDefault();
                statTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                statText.text = "'1' - best: " + high1 + " points.\n'2' - best: " + high2 + " seconds.\n'3' - best: " +
                                high3 + " points.";
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    sound.Play(); 
                    startFlag = !startFlag;
                    statTextObj.transform.position = _defPos;
                    startTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    _mode = 0;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (backAudio.isPlaying)
                    backAudio.Pause();
                else
                    backAudio.Play();
            }

            loseText1.text = "";
            loseText2.text = "";
        }
        else
        {
            loseText1.text = "You lose!";
            loseText2.text = "Click 'SPACE' to restart the game.";
            AllSetDefault();
        }
    }

    
    // Start is called before the first frame update
    private void Start()
    {
        cam.depthTextureMode = DepthTextureMode.None;
        close = GameObject.Find("Close");
        rect = GameObject.Find("Rect");
        triangle = GameObject.Find("Triangle");
        round = GameObject.Find("Round");
        playerRound = GameObject.Find("PlRound");
        playerTriangle = GameObject.Find("PlTriangle");
        playerRect = GameObject.Find("PlRect");
        playerClose = GameObject.Find("PlClose");
        resText = GameObject.Find("Canvas/ResText").GetComponent<Text>();
        statText = GameObject.Find("Canvas/Stats").GetComponent<Text>();
        loseTextObj1 = GameObject.Find("Canvas/LoseText");
        loseTextObj2 = GameObject.Find("Canvas/AnyKey_Lose");
        startTextObj = GameObject.Find("Canvas/Start text");
        statTextObj = GameObject.Find("Canvas/Stats");
        loseText1 = loseTextObj1.GetComponent<Text>();
        loseText2 = loseTextObj2.GetComponent<Text>();
        loseText1.text = "";
        loseText2.text = "";
        SetRandomAnimalPos();

        AllSetDefault();
        backAudio.Play();
        
        statTextObj.transform.position = _defPos;
    }
    
    // Update is called every frame
    private void Update()
    {
        if (!startFlag)
            if (!backAudio.isPlaying)
                backAudio.Play();
        if (Input.GetKeyDown(KeyCode.Escape) && !startFlag)
            Application.Quit();
        if (!startFlag && Input.GetKeyDown(KeyCode.Alpha1))
        {
            startFlag = true;
            startTextObj.transform.position = new Vector3(-1000, 0, 0);
            SetRandomAnimalPos();
            _mode = 1;
            backAudio.Pause();
            sound.Play();
            ResetPlayerPos();
            
        }
        if (!startFlag && Input.GetKeyDown(KeyCode.Alpha2))
        {
            startFlag = true;
            startTextObj.transform.position = new Vector3(-1000, 0, 0);
            SetRandomAnimalPos();
            _mode = 2;
            backAudio.Pause();
            sound.Play();
            ResetPlayerPos();
            
        }
        if (!startFlag && Input.GetKeyDown(KeyCode.Alpha3))
        {
            startFlag = true;
            startTextObj.transform.position = new Vector3(-1000, 0, 0);
            SetRandomAnimalPos();
            _mode = 3;
            backAudio.Pause();
            sound.Play();
            ResetPlayerPos();
        }
        if (!startFlag && Input.GetKeyDown(KeyCode.Alpha4))
        {
            startFlag = true;
            startTextObj.transform.position = new Vector3(-1000, 0, 0);
            SetRandomAnimalPos();
            _mode = 4;
            sound.Play();
            ResetPlayerPos();
        }
        if (startFlag)
        {
            LoseScreen(loseFlag);
            if (Input.GetKeyDown(KeyCode.Space) && loseFlag)
            {
                loseFlag = !loseFlag;
                SetRandomAnimalPos();
                ResetPlayerPos();
                LoseScreen(loseFlag);
            }
        }
    }
}
