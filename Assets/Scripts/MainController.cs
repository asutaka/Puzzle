using SimpleFileBrowser;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    #region Các biến public
    /// <summary>
    /// Đối tượng chính để add các RawImage vào
    /// </summary>
    public GameObject objMain;
    /// <summary>
    /// Đối tượng RawImage chứa toàn bộ bức ảnh
    /// </summary>
    public RawImage imgMain;
    /// <summary>
    /// Button
    /// </summary>
    public Button btnPlay;
    public Button btnBack;
    public Button btnNext;
    public Button btnLoadImage;
    public Button btnSetting;
    public Button btnHighScore;
    public Button btnReLoad;
    public GameObject btnCompass;
    /// <summary>
    /// Biến check game đang ở mode EASY hay mode NORMAL
    /// </summary>
    public Toggle chkMode;
    #endregion
    #region Các biến private
    //Biến chứa thông tin mode: EASY/NORMAL
    private Type _type;
    //
    private int ROW = -1, COL = -1;
    //
    private int[,] arrMatrix;
    //
    private float stepW = 0, stepH = 0;
    //
    private float startPosX = 0, startPosY = 0;
    //
    private List<GameObject> lstGameObject = new List<GameObject>();
    //
    private List<GameObject> lstGameObject_BackGround = new List<GameObject>();
    //Biến lưu index của ảnh hiện tại(dùng lưu thông tin index của ảnh trong Asset)
    private int curImage = 1;
    //Hằng số giá trị index của ảnh đầu tiên và ảnh cuối cùng
    private const int FIRST_IMAGE = 1, LAST_IMAGE = 12;
    //Biến kiểm tra keyCode nào được bấm
    private bool keyLeft, keyRight, keyUp, keyDown;
    //Biến lưu index của cell trống hiện tại(Cell không chứa miếng ghép)
    private int currentEmptyRow = 1, currentEmptyCol = 1;
    // Biến chứa đường dẫn được lựa chọn 
    private string path;
    //Hằng số giá trị số lần đảo ảnh
    private const int MAX_SWAP = 4;
    //Biến lưu index của lần swap 
    private int curSwap = 0;
    #endregion
    #region enum
    private enum Type : int
    {
        EASY = 0,
        NORMAL = 1,
    }
    #endregion
    #region Start/Update
    void Start()
    {
        initButtonStatus();
        //////////////////////////////////////////
        checkType();
        switch (_type)
        {
            case Type.EASY:
                ROW = 5;
                COL = 7;
                break;
            case Type.NORMAL:
                ROW = 7;
                COL = 10;
                break;
            default:
                break;
        }
        arrMatrix = new int[ROW, COL];
        //////////////////////////////////////////
        ThietLapThongSo();
        generateArray();
        generatePiece_BackGround();
        generatePiece();
        LoadDefaultImage();
    }
    // Update is called once per frame
    void Update()
    {
        getInput();
        handle();
    }
    #endregion
    #region Public function
    /// <summary>
    /// Hàm show Dialog chọn ảnh 
    /// </summary>
    public void OpenExplore()
    {
        FileBrowser.ShowLoadDialog(
                                    (_path) =>
                                    {
                                        this.path = _path;
                                        getImage();
                                        loadToTexture();
                                    },
                                    () => { this.path = ""; });
    }
    /// <summary>
    /// Chuyển sang ảnh kế tiếp
    /// </summary>
    public void NextImage()
    {
        LoadDefaultImage(++curImage);
        if (curImage >= LAST_IMAGE)
        {
            btnNext.gameObject.SetActive(false);
        }
        btnBack.gameObject.SetActive(true);

        curSwap = 0;
        Swap_Normal();
        reArrange();
    }
    /// <summary>
    /// Quay về ảnh trước đó
    /// </summary>
    public void PrevImage()
    {
        LoadDefaultImage(--curImage);
        if (curImage <= FIRST_IMAGE)
        {
            btnBack.gameObject.SetActive(false);
        }
        btnNext.gameObject.SetActive(true);

        curSwap = 0;
        Swap_Normal();
        reArrange();
    }
    /// <summary>
    /// Button Play Game
    /// </summary>
    public void ButtonPlay()
    {
        btnPlay.gameObject.SetActive(false);
        btnBack.gameObject.SetActive(false);
        btnNext.gameObject.SetActive(false);
        btnSetting.gameObject.SetActive(false);
        btnHighScore.gameObject.SetActive(false);
        btnReLoad.gameObject.SetActive(false);

        btnCompass.gameObject.SetActive(true);
    }
    /// <summary>
    /// Button Compass Down, hiển thị ảnh full
    /// </summary>
    public void ButtonCompassDown()
    {
        foreach (var item in lstGameObject)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in lstGameObject_BackGround)
        {
            item.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Button Compass Up, hiển thị các mảnh ghép hiện tại
    /// </summary>
    public void ButtonCompassUp()
    {
        foreach (var item in lstGameObject)
        {
            item.gameObject.SetActive(true);
        }
        foreach (var item in lstGameObject_BackGround)
        {
            item.gameObject.SetActive(true);
        }
    }
    public void ButtonSwap()
    {
        //Tên Packet : 1_Disney ==> 1
        var x = 1;
        var index = 0;

        curSwap++;
        if (curSwap > MAX_SWAP)
        {
            curSwap = 1;
        }
        switch (curSwap)
        {
            case 1:
                index = 0 + curImage;
                break;
            case 2:
                index = 13 + (((x * x) + curImage) % 15);
                break;
            case 3:
                index = 28 + (((x * x) + curImage) % 16);
                break;
            case 4:
                index = 0;
                break;
        }

        int indexSwap = 0 + index;
        switch (indexSwap)
        {
            case 0: Swap_Normal();break;
            case 1: Swap_UpDownA();break;
            case 2: Swap_UpDownA_L();break;
            case 3: Swap_UpDownA_R();break;
            case 4: Swap_UpDownA_U();break;
            case 5: Swap_UpDownA_D();break;
            case 6: Swap_UpDownA_LR();break;
            case 7: Swap_UpDownA_RL();break;
            case 8: Swap_UpDownB();break;
            case 9: Swap_UpDownB_L();break;
            case 10: Swap_UpDownB_R();break;
            case 11: Swap_UpDownB_U();break;
            case 12: Swap_UpDownB_D();break;
            case 13: Swap_Sole();break;
            case 14: Swap_Block3A();break;
            case 15: Swap_Sole_Left();break;
            case 16: Swap_Block3B();break;
            case 17: Swap_Sole_Right();break;
            case 18: Swap_Block3C();break;
            case 19: Swap_Sole_Up();break;
            case 20: Swap_Block3D();break;
            case 21: Swap_Sole_Down();break;
            case 22: Swap_Block3E();break;
            case 23: Swap_Sole_LR();break;
            case 24: Swap_Block3F();break;
            case 25: Swap_Sole_RL();break;
            case 26: Swap_Block3G();break;
            case 27: Swap_Block3H();break;
            case 28: Swap_UpDownB_LR(); break;
            case 29: Swap_UpDownB_RL(); break;
            case 30: Swap_Sin();break;
            case 31: SwapHoanVi();break;
            case 32: Swap_Sin_L();break;
            case 33: SwapHoanVi_L();break;
            case 34: Swap_Sin_R();break;
            case 35: SwapHoanVi_R();break;
            case 36: Swap_Sin_U();break;
            case 37: SwapHoanVi_U();break;
            case 38: Swap_Sin_D();break;
            case 39: SwapHoanVi_D();break;
            case 40: Swap_Sin_LR();break;
            case 41: SwapHoanVi_LR();break;
            case 42: Swap_Sin_RL();break;
            case 43: SwapHoanVi_RL();break;
            default:Swap_Normal();break;
        }
        reArrange();
        //showArray();
    }
    //fake
    private void showArray()
    {
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                Debug.Log(string.Format("index[{0},{1}]: {2} ", i, j, arrMatrix[i, j]));
            }
        }
    }
    #endregion
    #region Private function
    /// <summary>
    /// get thông tin về key nào được bấm từ bàn phím
    /// </summary>
    private void getInput()
    {
        keyLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        keyRight = Input.GetKeyDown(KeyCode.RightArrow);
        keyUp = Input.GetKeyDown(KeyCode.UpArrow);
        keyDown = Input.GetKeyDown(KeyCode.DownArrow);
    }
    /// <summary>
    /// Thiết lập trạng thái ban đầu Active/DeActive của các nút(chỉ gọi 1 lần vào lúc khởi tạo)
    /// </summary>
    private void initButtonStatus()
    {
        btnPlay.gameObject.SetActive(true);
        btnBack.gameObject.SetActive(true);
        btnNext.gameObject.SetActive(true);
        btnSetting.gameObject.SetActive(true);
        btnHighScore.gameObject.SetActive(true);
        btnReLoad.gameObject.SetActive(true);

        btnCompass.gameObject.SetActive(false);

        #region Tạm thời ẩn nút này đi
        btnLoadImage.gameObject.SetActive(false);
        chkMode.gameObject.SetActive(false);
        #endregion
    }
    /// <summary>
    /// check Loại độ khó được chọn,set dữ liệu vào biến _type
    /// </summary>
    private void checkType()
    {
        if (chkMode.isOn)
        {
            _type = Type.EASY;
        }
        else
        {
            _type = Type.NORMAL;
        }
    }
    /// <summary>
    /// Thiết lập thông số: độ rộng miếng ghép, độ dài miếng ghép, tọa độ ban đầu của X, tọa độ ban đầu của Y
    /// </summary>
    /// 
    private void ThietLapThongSo()
    {
        stepW = (float)Math.Round((float)Screen.width / (COL - 2), 2);
        stepH = (float)Math.Round((float)Screen.height / (ROW - 2), 2);
        startPosX = -(Screen.width - stepW) / 2;
        startPosY = (Screen.height - stepH) / 2;
    }
    /// <summary>
    /// Khởi tạo giá trị cho mảng
    /// </summary>
    /// 
    private void generateArray()
    {
        int count = 1;
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                if ((i < 1) || (j < 1) || (i == 1 && j == 1) || (i == ROW - 1) || (j == COL - 1))
                {
                    arrMatrix[i, j] = -1;
                    continue;
                }
                arrMatrix[i, j] = count++;
            }
        }
    }
    /// <summary>
    /// Add các RawImage cho background
    /// </summary>
    private void generatePiece_BackGround()
    {
        //clear List
        lstGameObject_BackGround.Clear();
        RectTransform rt = objMain.GetComponent<RectTransform>();
        for (int i = 0; i < ROW - 2; i++)
        {
            for (int j = 0; j < COL - 2; j++)
            {
                GameObject go = new GameObject("gameobject");
                var rectTransform = go.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.localScale = new Vector2(1.0f, 1.0f);
                rectTransform.sizeDelta = new Vector2(stepW, stepH);

                var image = go.gameObject.AddComponent<RawImage>();
                image.color = new Color(255, 255, 0);

                go.transform.SetParent(rt, false);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosX + stepW * j, startPosY - stepH * i);
                //add to List
                lstGameObject_BackGround.Add(go);
            }
        }
    }
    /// <summary>
    /// Add các RawImage cho các mảnh ghép
    /// </summary>
    private void generatePiece()
    {
        //clear List
        lstGameObject.Clear();
        RectTransform rt = objMain.GetComponent<RectTransform>();
        for (int i = 0; i < ROW - 2; i++)
        {
            for (int j = 0; j < COL - 2; j++)
            {
                GameObject go = new GameObject("gameobject");
                var rectTransform = go.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.localScale = new Vector2(1.0f, 1.0f);
                rectTransform.sizeDelta = new Vector2(stepW - 3, stepH - 3);

                if (!(i == 0 && j == 0))
                {
                    var image = go.gameObject.AddComponent<RawImage>();

                    var info = go.AddComponent<Info>();
                    initInfo(ref info, i + 1, j + 1, true);
                }

                go.transform.SetParent(rt, false);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosX + stepW * j, startPosY - stepH * i);
                //add to List
                lstGameObject.Add(go);
            }
        }
    }
    /// <summary>
    /// Phương thức chi tiết: Thiết lập giá trị cho đối tượng Info
    /// </summary>
    /// <param name="info"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="isOrigin"></param>
    private void initInfo(ref Info info, int row, int col, bool isOrigin = false)
    {
        info.Current = arrMatrix[row, col];
        if (isOrigin)
        {
            info.Origin = arrMatrix[row, col];
        }
    }
    /// <summary>
    /// Load ảnh mặc định ban đầu khi vào game
    /// </summary>
    /// <param name="index"></param>
    private void LoadDefaultImage(int index = 1)
    {
        var strInput = "";
        switch (index)
        {
            case 1:
                strInput = "1_Disney/1_Dumbo";
                break;
            case 2:
                strInput = "1_Disney/2_Frozen";
                break;
            case 3:
                strInput = "1_Disney/3_MickeyMouse";
                break;
            case 4:
                strInput = "1_Disney/4_Parrot";
                break;
            case 5:
                strInput = "1_Disney/5_PeterPan";
                break;
            case 6:
                strInput = "1_Disney/6_SnowWhiteAndTheSevenDwarfs";
                break;
            case 7:
                strInput = "1_Disney/7_Tangled";
                break;
            case 8:
                strInput = "1_Disney/8_TheLittleMermaid";
                break;
            case 9:
                strInput = "1_Disney/9_TheBeautyAndTheBeast";
                break;
            case 10:
                strInput = "1_Disney/10_TheLionKing";
                break;
            case 11:
                strInput = "1_Disney/11_Up";
                break;
            case 12:
                strInput = "1_Disney/12_WinnieThePooh";
                break;

        }
        //Load a Texture (Assets/Resources/Disney/1_Dumbo.png)
        var texture = Resources.Load<Texture2D>(strInput);
        imgMain.texture = texture;
        loadToTexture();
    }
    /// <summary>
    /// Phương thức xử lý chi tiết khi load ảnh
    /// </summary>
    private void loadToTexture()
    {
        Texture2D _texture = (Texture2D)imgMain.texture;
        RenderTexture _renderTexture = RenderTexture.GetTemporary(
                           _texture.width,
                           _texture.height,
                           0,
                           RenderTextureFormat.Default,
                           RenderTextureReadWrite.Linear);
        Graphics.Blit(_texture, _renderTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = _renderTexture;
        Texture2D myTexture2D = new Texture2D(_texture.width, _texture.height);
        myTexture2D.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        myTexture2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(_renderTexture);
        float _stepW = 0, _stepH = 0;

        for (int i = 0; i < lstGameObject.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }
            var item = lstGameObject[i];
            float startX = -1, startY = -1;
            _stepW = myTexture2D.width / (COL - 2);
            _stepH = myTexture2D.height / (ROW - 2);

            startX = (float)Math.Round(_stepW * (i % (COL - 2)), 2);
            startY = (float)Math.Round(_stepH * (ROW - (3 + i / (COL - 2))), 2);

            var _sprite1 = Sprite.Create(myTexture2D, new Rect(startX, startY, _stepW, _stepH), new Vector2(0.5f, 0.5f));

            Texture2D newText = new Texture2D((int)_sprite1.rect.width, (int)_sprite1.rect.height);
            Color[] newColors = _sprite1.texture.GetPixels((int)_sprite1.textureRect.x,
                                                             (int)_sprite1.textureRect.y,
                                                             (int)_sprite1.textureRect.width,
                                                             (int)_sprite1.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();

            var raw = item.GetComponent<RawImage>();
            raw.texture = newText;
        }
    }
    /// <summary>
    /// Hàm xử lý được gọi bên trong phương thức update
    /// </summary>
    private void handle()
    {
        if (keyLeft)
        {
            if (currentEmptyCol <= COL - 2)
            {
                int rowNext = -1, colNext = -1;
                rowNext = currentEmptyRow;
                colNext = currentEmptyCol + 1;
                var val = arrMatrix[rowNext, colNext];
                GameObject gameObjectBkgr = getGameObjectBackground(currentEmptyRow, currentEmptyCol);
                foreach (var item in lstGameObject)
                {
                    var inf = item.GetComponent<Info>();
                    if (inf == null)
                    {
                        continue;
                    }
                    if (inf.Current == val)
                    {
                        arrMatrix[currentEmptyRow, currentEmptyCol] = arrMatrix[rowNext, colNext];
                        arrMatrix[rowNext, colNext] = -1;
                        currentEmptyCol = colNext;
                        item.GetComponent<RectTransform>().anchoredPosition = gameObjectBkgr.GetComponent<RectTransform>().anchoredPosition;
                        break;
                    }
                }
            }
            else
            {
                return;
            }
        }
        else if (keyRight)
        {
            if (currentEmptyCol >= 2)
            {
                int rowNext = -1, colNext = -1;
                rowNext = currentEmptyRow;
                colNext = currentEmptyCol - 1;
                var val = arrMatrix[rowNext, colNext];
                GameObject gameObjectBkgr = getGameObjectBackground(currentEmptyRow, currentEmptyCol);
                foreach (var item in lstGameObject)
                {
                    var inf = item.GetComponent<Info>();
                    if (inf == null)
                    {
                        continue;
                    }
                    if (inf.Current == val)
                    {
                        arrMatrix[currentEmptyRow, currentEmptyCol] = arrMatrix[rowNext, colNext];
                        arrMatrix[rowNext, colNext] = -1;
                        currentEmptyCol = colNext;
                        item.GetComponent<RectTransform>().anchoredPosition = gameObjectBkgr.GetComponent<RectTransform>().anchoredPosition;
                        break;
                    }
                }
            }
            else
            {
                return;
            }
        }
        else if (keyUp)
        {
            if (currentEmptyRow <= ROW - 2)
            {
                int rowNext = -1, colNext = -1;
                rowNext = currentEmptyRow + 1;
                colNext = currentEmptyCol;
                var val = arrMatrix[rowNext, colNext];
                GameObject gameObjectBkgr = getGameObjectBackground(currentEmptyRow, currentEmptyCol);
                foreach (var item in lstGameObject)
                {
                    var inf = item.GetComponent<Info>();
                    if (inf == null)
                    {
                        continue;
                    }
                    if (inf.Current == val)
                    {
                        arrMatrix[currentEmptyRow, currentEmptyCol] = arrMatrix[rowNext, colNext];
                        arrMatrix[rowNext, colNext] = -1;
                        currentEmptyRow = rowNext;
                        item.GetComponent<RectTransform>().anchoredPosition = gameObjectBkgr.GetComponent<RectTransform>().anchoredPosition;
                        break;
                    }
                }
            }
            else
            {
                return;
            }
        }
        else if (keyDown)
        {
            if (currentEmptyRow >= 2)
            {
                int rowNext = -1, colNext = -1;
                rowNext = currentEmptyRow - 1;
                colNext = currentEmptyCol;
                var val = arrMatrix[rowNext, colNext];
                GameObject gameObjectBkgr = getGameObjectBackground(currentEmptyRow, currentEmptyCol);
                foreach (var item in lstGameObject)
                {
                    var inf = item.GetComponent<Info>();
                    if (inf == null)
                    {
                        continue;
                    }
                    if (inf.Current == val)
                    {
                        arrMatrix[currentEmptyRow, currentEmptyCol] = arrMatrix[rowNext, colNext];
                        arrMatrix[rowNext, colNext] = -1;
                        currentEmptyRow = rowNext;
                        item.GetComponent<RectTransform>().anchoredPosition = gameObjectBkgr.GetComponent<RectTransform>().anchoredPosition;
                        break;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
    /// <summary>
    /// Hàm set lại location các ảnh ứng với index của mảng
    /// </summary>
    private void reArrange()
    {
        for (int i = 1; i <= ROW - 2; i++)
        {
            for (int j = 1; j <= COL - 2; j++)
            {
                var gameObjectBkgr = getGameObjectBackground(i,j);
                foreach (var item in lstGameObject)
                {
                    var inf = item.GetComponent<Info>();
                    if (inf == null)
                    {
                        continue;
                    }
                    if (inf.Current == arrMatrix[i,j])
                    {
                        item.GetComponent<RectTransform>().anchoredPosition = gameObjectBkgr.GetComponent<RectTransform>().anchoredPosition;
                        break;
                    }
                }
            }
        }
        
    }
    /// <summary>
    /// Hàm xử lý chi tiết: get đối được Nền
    /// </summary>
    /// <returns></returns>
    private GameObject getGameObjectBackground(int _row,int _col)
    {
        var i = (_row - 1) * (COL - 2) + (_col - 1);
        return lstGameObject_BackGround[i];
    }
    /// <summary>
    /// Hàm chi tiết: Lấy ảnh từ đường dẫn,được gọi bởi phương thức OpenExplore
    /// </summary>
    private void getImage()
    {
        if (path != null)
        {
            WWW www = new WWW(string.Format("file:///{0}", path));
            imgMain.texture = www.texture;
        }
    }

    #region swap
    /// <summary>
    /// Trở về trạng thái chưa swap
    /// </summary>
    private void Swap_Normal()
    {
        generateArray();
    }
    #region Swap Sole
    #region Sole
    private void Swap_Sole()
    {
        generateArray();
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                if (arrMatrix[i, j] > -1 && (j + 2) < COL)
                {
                    if (i % 2 != 0 && j == 1)
                        continue;
                    var tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                    arrMatrix[i, j + 1] = tmp;
                    j++;
                }
            }
        }
    }
    #endregion

    #region Sole + Dịch phải
    private void Swap_Sole_Right()
    {
        Swap_Sole();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = COL - 2; j >= 1; j--)
            {
                if (j == COL - 2)
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j - 1];
                }
                else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    arrMatrix[i, j] = tmp;
                }
                else if (arrMatrix[i, j] > -1)
                {
                    arrMatrix[i, j] = arrMatrix[i, j - 1];
                }
            }
        }
    }
    #endregion

    #region Sole + Dịch trái
    private void Swap_Sole_Left()
    {
        Swap_Sole();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = 1; j <= COL - 2; j++)
            {
                if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                }
                else if (j == COL - 2)
                {
                    arrMatrix[i, j] = tmp;
                }
                else if (arrMatrix[i, j] > -1)
                {
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                }
            }
        }
    }
    #endregion

    #region Sole + Dịch lên
    private void Swap_Sole_Up()
    {
        Swap_Sole();

        for (int j = 1; j <= COL - 2; j++)
        {
            var tmp = -1;
            for (int i = 1; i <= ROW - 2; i++)
            {
                if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i + 1, j];
                }
                else if (i == ROW - 2)
                {
                    arrMatrix[i, j] = tmp;
                }
                else if (arrMatrix[i, j] > -1)
                {
                    arrMatrix[i, j] = arrMatrix[i + 1, j];
                }
            }
        }
    }
    #endregion

    #region Sole + Dịch xuống
    private void Swap_Sole_Down()
    {
        Swap_Sole();

        for (int j = 1; j <= COL - 2; j++)
        {
            var tmp = -1;
            for (int i = ROW - 2; i >= 1; i--)
            {
                if (i == ROW - 2)
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i - 1, j];
                }
                else if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    arrMatrix[i, j] = tmp;
                }
                else if (arrMatrix[i, j] > -1)
                {
                    arrMatrix[i, j] = arrMatrix[i - 1, j];
                }
            }
        }
    }
    #endregion

    #region Sole + Two Ways R-L
    private void Swap_Sole_RL()
    {
        Swap_Sole();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = 1; j <= COL - 2; j++)
                {
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                    else if (j == COL - 2)
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                }
            }
            else
            {
                for (int j = COL - 2; j >= 1; j--)
                {
                    if (j == COL - 2)
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                }
            }
        }
    }
    #endregion

    #region Sole + Two Ways L-R
    private void Swap_Sole_LR()
    {
        Swap_Sole();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = COL - 2; j >= 1; j--)
                {
                    if (j == COL - 2)
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                }
            }
            else
            {
                for (int j = 1; j <= COL - 2; j++)
                {
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                    else if (j == COL - 2)
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                }
            }
        }
    }
    #endregion
    #endregion
    #region Swap Block3
    private void Swap_Block3_R(int j)
    {
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int k = j; k <= j + 2; k++)
                {
                    if ((i == 1 && k == Math.Max(2, j)) || (i != 1 && k == Math.Max(1, j)))
                    {
                        tmp = arrMatrix[i, k];
                        arrMatrix[i, k] = arrMatrix[i, k + 1];
                    }
                    else if (k == j + 2)
                    {
                        arrMatrix[i, k] = tmp;
                    }
                    else if (arrMatrix[i, k] > -1)
                    {
                        arrMatrix[i, k] = arrMatrix[i, k + 1];
                    }
                }
            }
            else
            {
                for (int k = j + 2; k >= j; k--)
                {
                    if (k == j + 2)
                    {
                        tmp = arrMatrix[i, k];
                        arrMatrix[i, k] = arrMatrix[i, k - 1];
                    }
                    else if ((i == 1 && k == Math.Max(2, j)) || (i != 1 && k == Math.Max(1, j)))
                    {
                        arrMatrix[i, k] = tmp;
                    }
                    else if (arrMatrix[i, k] > -1)
                    {
                        arrMatrix[i, k] = arrMatrix[i, k - 1];
                    }
                }
            }
        }
    }
    private void Swap_Block3_L(int j)
    {
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int k = j + 2; k >= j; k--)
                {
                    if (k == j + 2)
                    {
                        tmp = arrMatrix[i, k];
                        arrMatrix[i, k] = arrMatrix[i, k - 1];
                    }
                    else if ((i == 1 && k == Math.Max(2, j)) || (i != 1 && k == Math.Max(1, j)))
                    {
                        arrMatrix[i, k] = tmp;
                    }
                    else if (arrMatrix[i, k] > -1)
                    {
                        arrMatrix[i, k] = arrMatrix[i, k - 1];
                    }
                }
            }
            else
            {
                for (int k = j; k <= j + 2; k++)
                {
                    if ((i == 1 && k == Math.Max(2, j)) || (i != 1 && k == Math.Max(1, j)))
                    {
                        tmp = arrMatrix[i, k];
                        arrMatrix[i, k] = arrMatrix[i, k + 1];
                    }
                    else if (k == j + 2)
                    {
                        arrMatrix[i, k] = tmp;
                    }
                    else if (arrMatrix[i, k] > -1)
                    {
                        arrMatrix[i, k] = arrMatrix[i, k + 1];
                    }
                }
            }
        }
    }
    private void Swap_Block3_U(int j)
    {
        for (int k = j; k <= j + 2; k++)
        {
            var tmp = -1;
            if (k % 2 == 0)
            {
                for (int i = ROW - 2; i >= 1; i--)
                {
                    if (i == ROW - 2)
                    {
                        tmp = arrMatrix[i, k];
                        arrMatrix[i, k] = arrMatrix[i - 1, k];
                    }
                    else if ((i == 2 && k == 1) || (i == 1 && k != 1))
                    {
                        arrMatrix[i, k] = tmp;
                    }
                    else if (arrMatrix[i, k] > -1)
                    {
                        arrMatrix[i, k] = arrMatrix[i - 1, k];
                    }
                }
            }
            else
            {
                for (int i = 1; i <= ROW - 2; i++)
                {
                    if ((i == 2 && k == 1) || (i == 1 && k != 1))
                    {
                        tmp = arrMatrix[i, k];
                        arrMatrix[i, k] = arrMatrix[i + 1, k];
                    }
                    else if (i == ROW - 2)
                    {
                        arrMatrix[i, k] = tmp;
                    }
                    else if (arrMatrix[i, k] > -1)
                    {
                        arrMatrix[i, k] = arrMatrix[i + 1, k];
                    }
                }
            }
        }
    }
    private void Swap_Block3_D(int j)
    {
        for (int k = j; k <= j + 2; k++)
        {
            var tmp = -1;
            if (k % 2 == 0)
            {
                for (int i = 1; i <= ROW - 2; i++)
                {
                    if ((i == 2 && k == 1) || (i == 1 && k != 1))
                    {
                        tmp = arrMatrix[i, k];
                        arrMatrix[i, k] = arrMatrix[i + 1, k];
                    }
                    else if (i == ROW - 2)
                    {
                        arrMatrix[i, k] = tmp;
                    }
                    else if (arrMatrix[i, k] > -1)
                    {
                        arrMatrix[i, k] = arrMatrix[i + 1, k];
                    }
                }
            }
            else
            {
                for (int i = ROW - 2; i >= 1; i--)
                {
                    if (i == ROW - 2)
                    {
                        tmp = arrMatrix[i, k];
                        arrMatrix[i, k] = arrMatrix[i - 1, k];
                    }
                    else if ((i == 2 && k == 1) || (i == 1 && k != 1))
                    {
                        arrMatrix[i, k] = tmp;
                    }
                    else if (arrMatrix[i, k] > -1)
                    {
                        arrMatrix[i, k] = arrMatrix[i - 1, k];
                    }
                }
            }
        }
    }
    #region Block3 A - RDLU
    private void Swap_Block3A()
    {
        generateArray();
        int _type = 1;
        for (int j = 1; j <= COL - 2; j++)
        {
            if (j + 2 > COL - 2)
            {
                return;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_R(j);
                    break;
                case 2:
                    Swap_Block3_U(j);
                    break;
                case 3:
                    Swap_Block3_L(j);
                    break;
                case 4:
                    Swap_Block3_D(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
    }
    #endregion
    #region Block3 B - RULD
    private void Swap_Block3B()
    {
        generateArray();
        int _type = 1;
        for (int j = 1; j <= COL - 2; j++)
        {
            if (j + 2 > COL - 2)
            {
                return;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_R(j);
                    break;
                case 2:
                    Swap_Block3_D(j);
                    break;
                case 3:
                    Swap_Block3_L(j);
                    break;
                case 4:
                    Swap_Block3_U(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
    }
    #endregion
    #region Block3 C - LDRU
    private void Swap_Block3C()
    {
        generateArray();
        int _type = 1;
        for (int j = 1; j <= COL - 2; j++)
        {
            if (j + 2 > COL - 2)
            {
                return;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_L(j);
                    break;
                case 2:
                    Swap_Block3_U(j);
                    break;
                case 3:
                    Swap_Block3_R(j);
                    break;
                case 4:
                    Swap_Block3_D(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
    }
    #endregion
    #region Block3 D - LURD
    private void Swap_Block3D()
    {
        generateArray();
        int _type = 1;
        for (int j = 1; j <= COL - 2; j++)
        {
            if (j + 2 > COL - 2)
            {
                return;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_L(j);
                    break;
                case 2:
                    Swap_Block3_D(j);
                    break;
                case 3:
                    Swap_Block3_R(j);
                    break;
                case 4:
                    Swap_Block3_U(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
    }
    #endregion
    #region Block3 E - URDL
    private void Swap_Block3E()
    {
        generateArray();
        int _type = 1;
        for (int j = 1; j <= COL - 2; j++)
        {
            if (j + 2 > COL - 2)
            {
                return;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_U(j);
                    break;
                case 2:
                    Swap_Block3_R(j);
                    break;
                case 3:
                    Swap_Block3_D(j);
                    break;
                case 4:
                    Swap_Block3_L(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
    }
    #endregion
    #region Block3 F - ULDR
    private void Swap_Block3F()
    {
        generateArray();
        int _type = 1;
        for (int j = 1; j <= COL - 2; j++)
        {
            if (j + 2 > COL - 2)
            {
                return;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_U(j);
                    break;
                case 2:
                    Swap_Block3_L(j);
                    break;
                case 3:
                    Swap_Block3_D(j);
                    break;
                case 4:
                    Swap_Block3_R(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
    }
    #endregion
    #region Block3 G - DRUL
    private void Swap_Block3G()
    {
        generateArray();
        int _type = 1;
        for (int j = 1; j <= COL - 2; j++)
        {
            if (j + 2 > COL - 2)
            {
                return;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_D(j);
                    break;
                case 2:
                    Swap_Block3_R(j);
                    break;
                case 3:
                    Swap_Block3_U(j);
                    break;
                case 4:
                    Swap_Block3_L(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
    }
    #endregion
    #region Block3 H - DLUR
    private void Swap_Block3H()
    {
        generateArray();
        int _type = 1;
        for (int j = 1; j <= COL - 2; j++)
        {
            if (j + 2 > COL - 2)
            {
                return;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_D(j);
                    break;
                case 2:
                    Swap_Block3_L(j);
                    break;
                case 3:
                    Swap_Block3_U(j);
                    break;
                case 4:
                    Swap_Block3_R(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
    }
    #endregion
    #endregion
    #region Swap UpDown
    private void UpDownBase(bool _isTypeA)
    {
        for (int j = 1; j <= COL - 2; j++)
        {

            var isChan = (j % 2 == 0);
            if (!_isTypeA)
            {
                isChan = !isChan;
            }
            var tmp = -1;
            if (isChan)
            {
                for (int i = 1; i <= ROW - 2; i++)
                {
                    if ((i == 2 && j == 1) || (i == 1 && j != 1))
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i + 1, j];
                    }
                    else if (i == ROW - 2)
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i + 1, j];
                    }
                }
            }
            else
            {
                for (int i = ROW - 2; i >= 1; i--)
                {
                    if (i == ROW - 2)
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i - 1, j];
                    }
                    else if ((i == 2 && j == 1) || (i == 1 && j != 1))
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i - 1, j];
                    }
                }
            }
        }
    }
    private void Swap_UpDown(bool _isTypeA)
    {
        generateArray();
        UpDownBase(_isTypeA);
    }
    private void Swap_UpDown_R(bool _isTypeA)
    {
        generateArray();
        UpDownBase(_isTypeA);
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = COL - 2; j >= 1; j--)
            {
                if (j == COL - 2)
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j - 1];
                }
                else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    arrMatrix[i, j] = tmp;
                }
                else if (arrMatrix[i, j] > -1)
                {
                    arrMatrix[i, j] = arrMatrix[i, j - 1];
                }
            }
        }
    }
    private void Swap_UpDown_L(bool _isTypeA)
    {
        generateArray();
        UpDownBase(_isTypeA);
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = 1; j <= COL - 2; j++)
            {
                if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                }
                else if (j == COL - 2)
                {
                    arrMatrix[i, j] = tmp;
                }
                else if (arrMatrix[i, j] > -1)
                {
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                }
            }
        }
    }
    private void Swap_UpDown_U(bool _isTypeA)
    {
        generateArray();
        UpDownBase(_isTypeA);
        for (int j = 1; j <= COL - 2; j++)
        {
            var tmp = -1;
            for (int i = 1; i <= ROW - 2; i++)
            {
                if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i + 1, j];
                }
                else if (i == ROW - 2)
                {
                    arrMatrix[i, j] = tmp;
                }
                else if (arrMatrix[i, j] > -1)
                {
                    arrMatrix[i, j] = arrMatrix[i + 1, j];
                }
            }
        }
    }
    private void Swap_UpDown_D(bool _isTypeA)
    {
        generateArray();
        UpDownBase(_isTypeA);
        for (int j = 1; j <= COL - 2; j++)
        {
            var tmp = -1;
            for (int i = ROW - 2; i >= 1; i--)
            {
                if (i == ROW - 2)
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i - 1, j];
                }
                else if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    arrMatrix[i, j] = tmp;
                }
                else if (arrMatrix[i, j] > -1)
                {
                    arrMatrix[i, j] = arrMatrix[i - 1, j];
                }
            }
        }
    }
    private void Swap_UpDown_LR(bool _isTypeA)
    {
        generateArray();
        UpDownBase(_isTypeA);
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = COL - 2; j >= 1; j--)
                {

                    if (j == COL - 2)
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }

                }
            }
            else
            {
                for (int j = 1; j <= COL - 2; j++)
                {
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                    else if (j == COL - 2)
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                }
            }
        }
    }
    private void Swap_UpDown_RL(bool _isTypeA)
    {
        generateArray();
        UpDownBase(_isTypeA);
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = 1; j <= COL - 2; j++)
                {
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                    else if (j == COL - 2)
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                }
            }
            else
            {
                for (int j = COL - 2; j >= 1; j--)
                {

                    if (j == COL - 2)
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else if (arrMatrix[i, j] > -1)
                    {
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }

                }
            }
        }
    }

    private void Swap_UpDownA()
    {
        Swap_UpDown(true);
    }
    private void Swap_UpDownA_R()
    {
        Swap_UpDown_R(true);
    }
    private void Swap_UpDownA_L()
    {
        Swap_UpDown_L(true);
    }
    private void Swap_UpDownA_U()
    {
        Swap_UpDown_U(true);
    }
    private void Swap_UpDownA_D()
    {
        Swap_UpDown_D(true);
    }
    private void Swap_UpDownA_LR()
    {
        Swap_UpDown_LR(true);
    }
    private void Swap_UpDownA_RL()
    {
        Swap_UpDown_RL(true);
    }
    private void Swap_UpDownB()
    {
        Swap_UpDown(false);
    }
    private void Swap_UpDownB_R()
    {
        Swap_UpDown_R(false);
    }
    private void Swap_UpDownB_L()
    {
        Swap_UpDown_L(false);
    }
    private void Swap_UpDownB_U()
    {
        Swap_UpDown_U(false);
    }
    private void Swap_UpDownB_D()
    {
        Swap_UpDown_D(false);
    }
    private void Swap_UpDownB_LR()
    {
        Swap_UpDown_LR(false);
    }
    private void Swap_UpDownB_RL()
    {
        Swap_UpDown_RL(false);
    }   
    #endregion
    #region Swap Hoán vị chéo
    private void SwapHoanVi()
    {
        generateArray();
        int div = 1;
        var count = 0;
        var countTemp = 0;
        for (int i = 1; i <= ROW - 2; i++)
        {
            countTemp = 0;
            for (int j = div; j <= COL - 2; j++)
            {
                if (arrMatrix[i, j] <= -1)
                {
                    j++;
                    if (i == 1)
                    {
                        count++;
                    }
                    continue;
                }
                if (j + 1 <= COL - 2)
                {
                    var tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                    arrMatrix[i, j + 1] = tmp;
                    j++;
                    if(i == 1)
                    {
                        count++;
                    }
                    else
                    {
                        countTemp++;
                    }
                    if(countTemp == count)
                    {
                        break;
                    }
                }
                if (j == COL - 2)
                {
                    if (div <= 2)
                        continue;
                    for (int k = 1; k < div; k++)
                    {
                        var tmp = arrMatrix[i, k];
                        arrMatrix[i, k] = arrMatrix[i, k + 1];
                        arrMatrix[i, k + 1] = tmp;
                        k++;
                        if (i == 1)
                        {
                            count++;
                        }
                        else
                        {
                            countTemp++;
                        }
                        if (countTemp == count)
                        {
                            j = COL - 2;
                            break;
                        }
                        if (k + 1 >= div)
                        {
                            j = COL - 2;
                            break;
                        }
                    }
                }
                if ((j + 1) == COL - 2)
                {
                    for (int k = 1; k < div; k++)
                    {
                        var tmp = arrMatrix[i, j + 1];
                        arrMatrix[i, j + 1] = arrMatrix[i, k];
                        arrMatrix[i, k] = tmp;
                        k++;
                        if (i == 1)
                        {
                            count++;
                        }
                        else
                        {
                            countTemp++;
                        }
                        if (countTemp == count)
                        {
                            j = COL - 2;
                            break;
                        }
                        if (k + 1 >= div)
                        {
                            j = COL - 2;
                            break;
                        }
                    }
                }
            }
            div++;
        }
    }
    private void SwapHoanVi_R()
    {
        SwapHoanVi();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = COL - 2; j >= 1; j--)
            {
                if (arrMatrix[i, j] <= -1)
                    continue;
                if (j == COL - 2)
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j - 1];
                }
                else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    arrMatrix[i, j] = tmp;
                }
                else
                {
                    arrMatrix[i, j] = arrMatrix[i, j - 1];
                }
            }
        }
    }
    private void SwapHoanVi_L()
    {
        SwapHoanVi();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = 1; j <= COL - 2; j++)
            {
                if (arrMatrix[i, j] <= -1)
                    continue;
                if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                }
                else if (j == COL - 2)
                {
                    arrMatrix[i, j] = tmp;
                }
                else
                {
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                }
            }
        }
    }
    private void SwapHoanVi_U()
    {
        SwapHoanVi();
        for (int j = 1; j <= COL - 2; j++)
        {
            var tmp = -1;
            for (int i = 1; i <= ROW - 2; i++)
            {
                if (arrMatrix[i, j] <= -1)
                    continue;
                if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i + 1, j];
                }
                else if(i == ROW - 2)
                {
                    arrMatrix[i, j] = tmp;
                }
                else
                {
                    arrMatrix[i, j] = arrMatrix[i + 1, j];
                }
            }
        }
    }
    private void SwapHoanVi_D()
    {
        SwapHoanVi();
        for (int j = 1; j <= COL - 2; j++)
        {
            var tmp = -1;
            for (int i = ROW - 2; i >= 1; i--)
            {
                if (arrMatrix[i, j] <= -1)
                    continue;
                if (i == ROW - 2)
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i - 1, j];
                }
                else if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    arrMatrix[i, j] = tmp;
                }
                else
                {
                    arrMatrix[i, j] = arrMatrix[i - 1, j];
                }
            }
        }
    }
    private void SwapHoanVi_RL()
    {
        SwapHoanVi();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if(i%2 == 0)
            {
                for (int j = 1; j <= COL - 2; j++)
                {
                    if (arrMatrix[i, j] <= -1)
                        continue;
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                    else if (j == COL - 2)
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                }
            }
            else
            {
                for (int j = COL - 2; j >= 1; j--)
                {
                    if (arrMatrix[i, j] <= -1)
                        continue;
                    if (j == COL - 2)
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                }
            }
        }
    }
    private void SwapHoanVi_LR()
    {
        SwapHoanVi();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = COL - 2; j >= 1; j--)
                {
                    if (arrMatrix[i, j] <= -1)
                        continue;
                    if (j == COL - 2)
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                }
            }
            else
            {
                for (int j = 1; j <= COL - 2; j++)
                {
                    if (arrMatrix[i, j] <= -1)
                        continue;
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                    else if (j == COL - 2)
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                }
            }
        }
    }
    #endregion
    #region Thuật toán hình Sin
    private void Swap_Sin()
    {
        generateArray();
        var arrTmp =  new int[ROW, COL];
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                arrTmp[i, j] = arrMatrix[i, j];
            }
        }
        for (int i = 1; i <= ROW - 2; i++)
        {
            for (int j = 1; j <= COL - 2; j++)
            {
                if((i + j) >= ROW)
                {
                    if(i == (ROW - 2) && j == (COL - 2))
                    {
                        arrTmp[i, j] = arrMatrix[1 , j];
                    }
                    else
                    {
                        arrTmp[i , j] = arrMatrix[1 + (i + j - 1)%( ROW - 1), j];
                    }
                }
                else
                {
                    arrTmp[i , j] = arrMatrix[i + j - 1, j];
                }
            }
        }
        arrMatrix = arrTmp;
    }
    private void Swap_Sin_R()
    {
        Swap_Sin();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = COL - 2; j >= 1; j--)
            {
                if (arrMatrix[i, j] <= -1)
                    continue;
                if (j == COL - 2)
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j - 1];
                }
                else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    arrMatrix[i, j] = tmp;
                }
                else
                {
                    arrMatrix[i, j] = arrMatrix[i, j - 1];
                }
            }
        }
    }
    private void Swap_Sin_L()
    {
        Swap_Sin();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = 1; j <= COL - 2; j++)
            {
                if (arrMatrix[i, j] <= -1)
                    continue;
                if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                }
                else if (j == COL - 2)
                {
                    arrMatrix[i, j] = tmp;
                }
                else
                {
                    arrMatrix[i, j] = arrMatrix[i, j + 1];
                }
            }
        }
    }
    private void Swap_Sin_U()
    {
        Swap_Sin();
        for (int j = 1; j <= COL - 2; j++)
        {
            var tmp = -1;
            for (int i = 1; i <= ROW - 2; i++)
            {
                if (arrMatrix[i, j] <= -1)
                    continue;
                if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i + 1, j];
                }
                else if (i == ROW - 2)
                {
                    arrMatrix[i, j] = tmp;
                }
                else
                {
                    arrMatrix[i, j] = arrMatrix[i + 1, j];
                }
            }
        }
    }
    private void Swap_Sin_D()
    {
        Swap_Sin();
        for (int j = 1; j <= COL - 2; j++)
        {
            var tmp = -1;
            for (int i = ROW - 2; i >= 1; i--)
            {
                if (arrMatrix[i, j] <= -1)
                    continue;
                if (i == ROW - 2)
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i - 1, j];
                }
                else if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    arrMatrix[i, j] = tmp;
                }
                else
                {
                    arrMatrix[i, j] = arrMatrix[i - 1, j];
                }
            }
        }
    }
    private void Swap_Sin_RL()
    {
        Swap_Sin();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = 1; j <= COL - 2; j++)
                {
                    if (arrMatrix[i, j] <= -1)
                        continue;
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                    else if (j == COL - 2)
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                }
            }
            else
            {
                for (int j = COL - 2; j >= 1; j--)
                {
                    if (arrMatrix[i, j] <= -1)
                        continue;
                    if (j == COL - 2)
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                }
            }
        }
    }
    private void Swap_Sin_LR()
    {
        Swap_Sin();
        for (int i = 1; i <= ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = COL - 2; j >= 1; j--)
                {
                    if (arrMatrix[i, j] <= -1)
                        continue;
                    if (j == COL - 2)
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        arrMatrix[i, j] = arrMatrix[i, j - 1];
                    }
                }
            }
            else
            {
                for (int j = 1; j <= COL - 2; j++)
                {
                    if (arrMatrix[i, j] <= -1)
                        continue;
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = arrMatrix[i, j];
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                    else if (j == COL - 2)
                    {
                        arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        arrMatrix[i, j] = arrMatrix[i, j + 1];
                    }
                }
            }
        }
    }
    #endregion
    #endregion
    #endregion
    #region SubClass
    private class Info : MonoBehaviour
    {
        public Info() { }
        public int Current { get; set; }
        public int Origin { get; set; }
    }
    #endregion
}
