using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // MANAGERS
    protected GameManager GM;
    protected AudioManager AM;

    [Header("General Timing [s]")]
    [SerializeField]
    private float animationDelay = 0.1f;
    [SerializeField]
    private float animationDuration = 1.0f;

    [Header("Dafault Panel")]
    [SerializeField]
    public GameObject nextPanel;

    // UI (intro or pause) currently diplayed panel
    public GameObject currentPanel;

    public float canvasWidth;

    private bool firstStart;

    protected static bool isShowPanelCoroutineActive;

    private void Awake()
    {
        // find the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();
        AM = gameManagerGameObject.GetComponent<AudioManager>();

        // get Canvas width
        RectTransform canvasRectTransform = this.GetComponent<RectTransform>();
        canvasWidth = canvasRectTransform.rect.width;
    }

    public void showPanel(string side)
    {
        if (isShowPanelCoroutineActive == false)
        {
            StartCoroutine(showPanelCoroutine(side));
            StartCoroutine(SetPanelsCoroutine());
        }
    }

    public IEnumerator showPanelCoroutine(string side)
    {
        isShowPanelCoroutineActive = true;

        float startPosX = 0;

        UIPausePanel UIPP = currentPanel.GetComponent<UIPausePanel>();

        if (side == "right")
        {
            // check if a panel exists to the right
            if (UIPP.rightPanel != null)
            {
                startPosX = canvasWidth;
                nextPanel = UIPP.rightPanel;
            }
        }
        // special case for controls panel in title scene
        else if (side == "top")
        {
            // check if a panel exists to the right
            if (UIPP.rightPanel != null)
            {
                startPosX = canvasWidth;
                nextPanel = UIPP.topPanel;
            }
        }
        // if (side == "left")
        else
        {
            // check if a panel exists to the right
            if (UIPP.leftPanel != null)
            {
                startPosX = -canvasWidth;
                nextPanel = UIPP.leftPanel;
            }
        }

        // get active panel rect transform
        RectTransform panelToHideRectTransform = currentPanel.GetComponent<RectTransform>();

        // place panel to show
        RectTransform panelToShowRectTransform = nextPanel.GetComponent<RectTransform>();
        panelToShowRectTransform.offsetMin = new Vector2(startPosX, panelToShowRectTransform.offsetMin.y);
        panelToShowRectTransform.offsetMax = new Vector2(startPosX, panelToShowRectTransform.offsetMax.y);
        nextPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(animationDelay);

        float newPosX;
        float endPosX = 0;

        float newPosX2;
        float startPosX2 = 0;
        float endPosX2 = -startPosX;

        for (float t = 0.0f; t < animationDuration; t += Time.unscaledDeltaTime / animationDuration)
        {
            newPosX = Mathf.Lerp(startPosX, endPosX, t);

            panelToShowRectTransform.offsetMin = new Vector2(newPosX, panelToShowRectTransform.offsetMin.y);
            panelToShowRectTransform.offsetMax = new Vector2(newPosX, panelToShowRectTransform.offsetMax.y);

            newPosX2 = Mathf.Lerp(startPosX2, endPosX2, t);

            panelToHideRectTransform.offsetMin = new Vector2(newPosX2, panelToHideRectTransform.offsetMin.y);
            panelToHideRectTransform.offsetMax = new Vector2(newPosX2, panelToHideRectTransform.offsetMax.y);

            yield return null;
        }

        panelToHideRectTransform.offsetMin = new Vector2(0, panelToHideRectTransform.offsetMin.y);
        panelToHideRectTransform.offsetMax = new Vector2(0, panelToHideRectTransform.offsetMax.y);
        currentPanel.SetActive(false);
        currentPanel = nextPanel;
    }

    public IEnumerator SetPanelsCoroutine()
    {
        yield return new WaitForSecondsRealtime(animationDelay + animationDuration);

        Canvas.ForceUpdateCanvases();

        isShowPanelCoroutineActive = false;
    }

    public void ShowWarning(GameObject warningPanel)
    {
        warningPanel.SetActive(true);
    }

    public void HideWarning(GameObject warningPanel)
    {
        warningPanel.SetActive(false);
    }

    public IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        yield return new WaitForSecondsRealtime(animationDelay);
        SceneManager.LoadScene(sceneName: sceneName);
    }

    public IEnumerator SplashScreenCoroutine(GameObject splashPanel, GameObject titlePanel, float delay)
    {
        if (GM.GetFirstStart() == true)
        {
            yield return new WaitForSeconds(delay);
        }

        // hide Splash Panel
        splashPanel.SetActive(false);
        // show Title Panel
        titlePanel.SetActive(true);

        GM.SetFirstStart(false);
    }

    public bool GetIsShowPanelCoroutineActive()
    {
        return isShowPanelCoroutineActive;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
