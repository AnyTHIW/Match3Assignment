using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType
{
    Splash,
    Title,
    Main,
    Rank,
    InGame,
}

public class SceneCtrl : MonoBehaviour
{
    public static SceneCtrl Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static SceneCtrl instance;

    public Canvas LoadingCanvas;
    public GameObject LoadingPanel;
    public Image ProgressBar;
    public Camera MainCamera;

    private Image LoadingImage;
    private GraphicRaycaster LoadingCanvasRaycaster;
    private const float FAKE_GAUGE_AMOUNT = 0.7F;
    private const float FULL_GAUGE_AMOUNT = 1.0F;
    private const float FAKE_TIME = 3.0F;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            // �̰Ŵ� �� �ȵ�?
            //DestroyImmediate(gameObject);
        }

        if (Camera.main != MainCamera)
        {
            Destroy(Camera.main.gameObject);
        }

        DontDestroyOnLoad(MainCamera);
        DontDestroyOnLoad(gameObject);

        LoadingImage = LoadingPanel.GetComponent<Image>();
        LoadingCanvas.gameObject.SetActive(false);
        LoadingCanvasRaycaster = LoadingCanvas.GetComponent<GraphicRaycaster>();
        LoadingCanvasRaycaster.enabled = false;

    }

    // Start is called before the first frame update
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)SceneType.Splash)
        {
            SceneManager.LoadScene((int)SceneType.Splash);
        }

        StartCoroutine(Count());
        Debug.Log("HelloStart");
        StartCoroutine(Load(SceneType.Title));
    }

    private IEnumerator Count()
    {
        Debug.Log("HelloCountBefore");

        yield return new WaitForSeconds(5F);

        Debug.Log("HelloCountAfter");


    }

    private IEnumerator Load(SceneType scene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Single);
        op.allowSceneActivation = false;//�ε��� ������ ���� �ٷ� ���� ���ϰ� �Ѵ�
        Debug.Log(string.Format("allowSceneActivation : {0} | progress : {1} | isDone : {2}", op.allowSceneActivation, op.progress, op.isDone));

        //�񵿱� ������� ���� �ҷ����� ���߿��� �ٸ� �۾��� �� ��  LoadSceneAsync �Լ�
        //�ε��� ���������� AsyncOperation Class�� ��ȯ�ȴ�

        while (op.progress <= FAKE_GAUGE_AMOUNT)//isDone�� false�� �� ����, �� Load�� �������� �ǹ��Ѵ�
        {
            Debug.Log(string.Format("allowSceneActivation : {0} | progress : {1} | isDone : {2}", op.allowSceneActivation, op.progress, op.isDone));

            // yield return null ���ְ�
            yield return null;
        }

        // ���⿡ new WaitForSeconds ����?
        Debug.Log(string.Format("allowSceneActivation : {0} | progress : {1} | isDone : {2}", op.allowSceneActivation, op.progress, op.isDone));

        new WaitForSeconds(FAKE_TIME);
        Debug.Log(string.Format("allowSceneActivation : {0} | progress : {1} | isDone : {2}", op.allowSceneActivation, op.progress, op.isDone));

        yield return new WaitForSeconds(FAKE_TIME);

        Debug.Log(string.Format("allowSceneActivation : {0} | progress : {1} | isDone : {2}", op.allowSceneActivation, op.progress, op.isDone));

        //�ε��� ������ ���� �ٷ� ���� ���ϰ� �Ѵ�
        if (op.allowSceneActivation == false)
        {
            Debug.Log(string.Format("allowSceneActivation : {0} | progress : {1} | isDone : {2}", op.allowSceneActivation, op.progress, op.isDone));

            yield break;
        }

    }

    private IEnumerator LoadSceneWithLoading(SceneType scene)
    {
        StartCoroutine(FadeOutUpToAlphaValue(0.5F));

        yield return new WaitForSecondsRealtime(0.5F);

        AsyncOperation op = SceneManager.LoadSceneAsync((int)scene);
        op.allowSceneActivation = false;

        float timer = 0F;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < FAKE_GAUGE_AMOUNT)
            {
                ProgressBar.fillAmount = op.progress;
            }
            // ����ũ �ε�
            else
            {
                timer += Time.unscaledDeltaTime;
                ProgressBar.fillAmount = Mathf.Lerp(FAKE_GAUGE_AMOUNT, FULL_GAUGE_AMOUNT, timer);

                if (ProgressBar.fillAmount >= FULL_GAUGE_AMOUNT)
                {
                    StartCoroutine(FadeInUpToAlphaValue(1.0F));

                    yield return new WaitForSecondsRealtime(0.5F);

                    break;
                }
            }
        }

        yield return true;
    }

    private IEnumerator FadeOutUpToAlphaValue(float toValue)
    {
        LoadingCanvas.gameObject.SetActive(true);
        LoadingCanvasRaycaster.enabled = true;

        float currValue = LoadingImage.color.r;

        while (currValue >= toValue)
        {
            currValue -= 0.01F;
            yield return new WaitForSecondsRealtime(0.01F);
            LoadingImage.color = new Color(0, 0, 0, currValue);
        }
    }

    private IEnumerator FadeInUpToAlphaValue(float toValue)
    {
        float currValue = LoadingImage.color.r;

        while (currValue < toValue)
        {
            currValue += 0.01F;
            yield return new WaitForSecondsRealtime(0.01F);
            LoadingImage.color = new Color(0, 0, 0, currValue);
        }

        LoadingCanvasRaycaster.enabled = false;
        LoadingCanvas.gameObject.SetActive(false);
    }

}
