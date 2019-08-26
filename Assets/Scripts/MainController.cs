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
    /// Đối tượng hiển thị stt của ảnh
    /// </summary>
    public Text txtCount;
    /// <summary>
    /// Đối tượng hiển thị thời gian thực hiện màn chơi
    /// </summary>
    public Text txtTime;
    /// <summary>
    /// Đối tượng thông báo trạng thái đã hoàn thành game
    /// </summary>
    public Text txtComplete;
    /// <summary>
    /// Đối tượng Canvas showDialog
    /// </summary>
    public Canvas cvShowDialog;
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
    public Button btnOK;
    public Button btnShare;
    public GameObject objControlHandle;
    /// <summary>
    /// Biến check game đang ở mode EASY hay mode NORMAL
    /// </summary>
    public Toggle chkMode;
    #endregion
    #region Các biến private
    //
    private List<GameObject> lstGameObject = new List<GameObject>();
    //
    private List<GameObject> lstGameObject_BackGround = new List<GameObject>();
    //Hằng số giá trị index của ảnh đầu tiên và ảnh cuối cùng
    private const int FIRST_IMAGE = 1, LAST_IMAGE = 12;
    //Biến kiểm tra keyCode nào được bấm
    private bool keyLeft, keyRight, keyUp, keyDown;
    //Biến lưu index của cell trống hiện tại(Cell không chứa miếng ghép)
    private int currentEmptyRow = 1, currentEmptyCol = 1;
    // Biến chứa đường dẫn được lựa chọn 
    private string path;
    
   
    //Biến check trạng thái đang chơi hay ở màn hình chờ
    private bool isView = true;
    /// <summary>
    /// Thời điểm ban đầu
    /// </summary>
    private float timeStart = 0;
    #endregion
    #region Start/Update
    void Start()
    {
        setStatus(true);
        //////////////////////////////////////////
        Common.setMode(chkMode.isOn);
        //////////////////////////////////////////
        generatePiece_BackGround();
        generatePiece();
        LoadDefaultImage();

        txtCount.text = Common.curImage.ToString();
        objControlHandle.SetTransparency(0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        if (isView)
            return;
        if (Common.checkComplete())
        {
            txtTime.gameObject.SetActive(false);
            txtComplete.text = txtTime.text;
            cvShowDialog.gameObject.SetActive(true);
        }
        else
        {
            ThoiGian();
        }
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
        LoadDefaultImage(++Common.curImage);
        if (Common.curImage >= LAST_IMAGE)
        {
            btnNext.gameObject.SetActive(false);
        }
        btnBack.gameObject.SetActive(true);
        txtCount.text = Common.curImage.ToString();

        Common.curSwap = Common.MAX_SWAP;
        Common.Swap_Normal();
        reArrange();
    }
    /// <summary>
    /// Quay về ảnh trước đó
    /// </summary>
    public void PrevImage()
    {
        LoadDefaultImage(--Common.curImage);
        if (Common.curImage <= FIRST_IMAGE)
        {
            btnBack.gameObject.SetActive(false);
        }
        btnNext.gameObject.SetActive(true);
        txtCount.text = Common.curImage.ToString();

        Common.curSwap = Common.MAX_SWAP;
        Common.Swap_Normal();
        reArrange();
    }
    /// <summary>
    /// Button Play Game
    /// </summary>
    public void ButtonPlay()
    {
        isView = false;

        if (Common.curSwap == Common.MAX_SWAP)
        {
            ButtonSwap();
        }
        //
        setStatus(false);
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
        Common.swap();
        reArrange();
    }
    public void ButtonOK()
    {
        setStatus(true);
        if(Common.curImage == LAST_IMAGE)
        {
            Common.curImage = FIRST_IMAGE + 1;
            PrevImage();
        }
        else
        {
            NextImage();
        }
    }
    public void Button_Left()
    {
        Left();
    }
    public void Button_Right()
    {
        Right();
    }
    public void Button_Up()
    {
        Up();
    }
    public void Button_Down()
    {
        Down();
    }
    int index = 13;
    public void ButtonShare()
    {
        
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
    /// Thiết lập trạng thái Active/DeActive của các nút
    /// </summary>
    private void setStatus(bool _isView)
    {
        btnPlay.gameObject.SetActive(_isView);
        btnBack.gameObject.SetActive(_isView);
        btnNext.gameObject.SetActive(_isView);
        btnSetting.gameObject.SetActive(_isView);
        btnHighScore.gameObject.SetActive(_isView);
        btnReLoad.gameObject.SetActive(_isView);

        btnCompass.gameObject.SetActive(!_isView);
        txtTime.gameObject.SetActive(!_isView);
        objControlHandle.gameObject.SetActive(!_isView);

        cvShowDialog.gameObject.SetActive(false);
        #region Tạm thời ẩn nút này đi
        btnLoadImage.gameObject.SetActive(false);
        chkMode.gameObject.SetActive(false);
        #endregion
    }
    private void ThoiGian()
    {
        timeStart += Time.deltaTime;
        TimeSpan t = TimeSpan.FromSeconds(timeStart);

        string strTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);
        txtTime.text = strTime;
    }
     
    /// <summary>
    /// Add các RawImage cho background
    /// </summary>
    private void generatePiece_BackGround()
    {
        //clear List
        lstGameObject_BackGround.Clear();
        RectTransform rt = objMain.GetComponent<RectTransform>();
        for (int i = 0; i < Common.ROW - 2; i++)
        {
            for (int j = 0; j < Common.COL - 2; j++)
            {
                GameObject go = new GameObject("gameobject");
                var rectTransform = go.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.localScale = new Vector2(1.0f, 1.0f);
                rectTransform.sizeDelta = new Vector2(Common.stepW, Common.stepH);

                var image = go.gameObject.AddComponent<RawImage>();
                image.color = new Color(255, 255, 0);

                go.transform.SetParent(rt, false);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(Common.startPosX + Common.stepW * j, Common.startPosY - Common.stepH * i);
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
        var count = 1;
        for (int i = 0; i < Common.ROW - 2; i++)
        {
            for (int j = 0; j < Common.COL - 2; j++)
            {
                GameObject go = new GameObject("gameobject");
                var rectTransform = go.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.localScale = new Vector2(1.0f, 1.0f);
                rectTransform.sizeDelta = new Vector2(Common.stepW - 3, Common.stepH - 3);

                if (!(i == 0 && j == 0))
                {
                    var image = go.gameObject.AddComponent<RawImage>();

                    var info = go.AddComponent<Info>();
                    Common.initInfo(ref info, i + 1, j + 1, true);
                }

                go.transform.SetParent(rt, false);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(Common.startPosX + Common.stepW * j, Common.startPosY - Common.stepH * i);
                //add to List
                lstGameObject.Add(go);
            }
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
            _stepW = myTexture2D.width / (Common.COL - 2);
            _stepH = myTexture2D.height / (Common.ROW - 2);

            startX = (float)Math.Round(_stepW * (i % (Common.COL - 2)), 2);
            startY = (float)Math.Round(_stepH * (Common.ROW - (3 + i / (Common.COL - 2))), 2);

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
            if (currentEmptyCol <= Common.COL - 2)
            {
                int rowNext = -1, colNext = -1;
                rowNext = currentEmptyRow;
                colNext = currentEmptyCol + 1;
                var val = Common.arrMatrix[rowNext, colNext];
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
                        Common.arrMatrix[currentEmptyRow, currentEmptyCol] = Common.arrMatrix[rowNext, colNext];
                        Common.arrMatrix[rowNext, colNext] = -1;
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
                var val = Common.arrMatrix[rowNext, colNext];
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
                        Common.arrMatrix[currentEmptyRow, currentEmptyCol] = Common.arrMatrix[rowNext, colNext];
                        Common.arrMatrix[rowNext, colNext] = -1;
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
            if (currentEmptyRow <= Common.ROW - 2)
            {
                int rowNext = -1, colNext = -1;
                rowNext = currentEmptyRow + 1;
                colNext = currentEmptyCol;
                var val = Common.arrMatrix[rowNext, colNext];
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
                        Common.arrMatrix[currentEmptyRow, currentEmptyCol] = Common.arrMatrix[rowNext, colNext];
                        Common.arrMatrix[rowNext, colNext] = -1;
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
                var val = Common.arrMatrix[rowNext, colNext];
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
                        Common.arrMatrix[currentEmptyRow, currentEmptyCol] = Common.arrMatrix[rowNext, colNext];
                        Common.arrMatrix[rowNext, colNext] = -1;
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
    private void Up()
    {
        keyLeft = false;
        keyRight = false;
        keyUp = true;
        keyDown = false;
        handle();
    }
    private void Down()
    {
        keyLeft = false;
        keyRight = false;
        keyUp = false;
        keyDown = true;
        handle();
    }
    private void Left()
    {
        keyLeft = true;
        keyRight = false;
        keyUp = false;
        keyDown = false;
        handle();
    }
    private void Right()
    {
        keyLeft = false;
        keyRight = true;
        keyUp = false;
        keyDown = false;
        handle();
    }
    /// <summary>
    /// Hàm set lại location các ảnh ứng với index của mảng
    /// </summary>
    private void reArrange()
    {
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            for (int j = 1; j <= Common.COL - 2; j++)
            {
                var gameObjectBkgr = getGameObjectBackground(i, j);
                foreach (var item in lstGameObject)
                {
                    var inf = item.GetComponent<Info>();
                    if (inf == null)
                    {
                        continue;
                    }
                    if (inf.Current == Common.arrMatrix[i, j])
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
    private GameObject getGameObjectBackground(int _row, int _col)
    {
        var i = (_row - 1) * (Common.COL - 2) + (_col - 1);
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
    #endregion
}
