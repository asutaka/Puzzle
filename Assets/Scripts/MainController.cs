using SimpleFileBrowser;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour {
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
                                    (_path) => {
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
        ++curSwap;
        if (curSwap > MAX_SWAP)
        {
            curSwap = 0;
        }
        switch (curSwap)
        {
            case 0:
                Swap_Normal();
                break;
            case 1:
                Swap_Sole();
                break;
            case 2:
                Swap_TinhTienNgang();
                break;
            case 3:
                Swap_TinhTienDoc();
                break;
            case 4:
                Swap_NgauNhien();
                break;
        }
        showArray();
    }
    //fake
    private void showArray()
    {
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                Debug.Log(string.Format("index[{0},{1}]: {2} ",i,j,arrMatrix[i,j]));
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
                strInput = "Disney/1_Dumbo";
                break;
            case 2:
                strInput = "Disney/2_Frozen";
                break;
            case 3:
                strInput = "Disney/3_MickeyMouse";
                break;
            case 4:
                strInput = "Disney/4_Parrot";
                break;
            case 5:
                strInput = "Disney/5_PeterPan";
                break;
            case 6:
                strInput = "Disney/6_SnowWhiteAndTheSevenDwarfs";
                break;
            case 7:
                strInput = "Disney/7_Tangled";
                break;
            case 8:
                strInput = "Disney/8_TheLittleMermaid";
                break;
            case 9:
                strInput = "Disney/9_TheBeautyAndTheBeast";
                break;
            case 10:
                strInput = "Disney/10_TheLionKing";
                break;
            case 11:
                strInput = "Disney/11_Up";
                break;
            case 12:
                strInput = "Disney/12_WinnieThePooh";
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
                GameObject gameObjectBkgr = getGameObjectBackground();
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
                GameObject gameObjectBkgr = getGameObjectBackground();
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
                GameObject gameObjectBkgr = getGameObjectBackground();
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
                GameObject gameObjectBkgr = getGameObjectBackground();
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
    /// Hàm xử lý chi tiết: get đối được Nền
    /// </summary>
    /// <returns></returns>
    private GameObject getGameObjectBackground()
    {
        var i = (currentEmptyRow - 1) * (COL - 2) + (currentEmptyCol - 1);
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
    //private void Swap_Sole_Right()
    //{
    //    Swap_Sole();
    //    for (int i = 0; i < ROW; i++)
    //    {
    //        for (int j = 0; j < COL; j++)
    //        {
    //            if (arrMatrix[i, j] > -1 && (j + 2) < COL)
    //            {

    //            }
    //        }
    //    }
    //}
    private void Swap_Sole_Right()
    {
        Swap_Sole();
        for (int i = 1; i < ROW; i++)
        {
            var tmp = -1;
            for (int j = COL - 2; j > 0; j--)
            {
                if(j == COL - 2)
                {
                    tmp = arrMatrix[i, j];
                    arrMatrix[i, j] = arrMatrix[i, j - 1];
                }
                else if((i == 1 && j == 2) || (i != 1 && j == 1))
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
    #endregion

    #region Sole + Dịch lên
    #endregion

    #region Sole + Dịch xuống
    #endregion

    #region Sole + Two Ways R-L
    #endregion

    #region Sole + Two Ways L-R
    #endregion
    #endregion



    /// <summary>
    /// Swap2: swap ở trạng thái tịnh tiến ngang
    /// </summary>
    private void Swap_TinhTienNgang()
    {
        generateArray();
    }
    /// <summary>
    /// Swap3: swap ở trạng thái tịnh tiến dọc
    /// </summary>
    private void Swap_TinhTienDoc()
    {
        generateArray();
    }
    /// <summary>
    /// Swap4: swap ở trạng thái ngẫu nhiên(chéo dịch phải)
    /// </summary>
    private void Swap_NgauNhien()
    {
        generateArray();
    }
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
