using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//参照：https://kenko-san.com/unity-fade/#toc4

/*　〇特定のシーンをフェードインで始めたい場合
 *    そのシーン内にある何れかのスクリプトのStart()内に以下の1行を追加
 *     FadeManager.FadeIn()；
 *  
 *  〇現在のシーンをフェードアウトで終わって、次のシーンにフェードインした場合
 *    任意のタイミングで以下の一行を追加
 *    FadeManager.FadeOut(遷移したいシーン番号);
 *    
 *    シーン遷移も同時に出来ます。
 */

public class FadeManager : MonoBehaviour
{
    //フェード用のCanvasとImage
    private static Canvas fadeCanvas;
    private static Image fadeImage;

    //フェード用Imageの透明度
    private static float alpha = 0.0f;

    //フェードインアウトのフラグ
    public static bool IsFadeIn = false;
    public static bool IsFadeOut = false;

    //フェードの長さ（単位は秒）
    private static float fadeTime = 0.4f;

    //遷移先のシーン番号
    private static int nextScene = 1;

    //フェード用のCanvasとImage生成
    static void Init()
    {
        //フェード用のCanvas生成
        GameObject FadeCanvasObject = new GameObject("CanvasFade");
        fadeCanvas = FadeCanvasObject.AddComponent<Canvas>();
        FadeCanvasObject.AddComponent<GraphicRaycaster>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        FadeCanvasObject.AddComponent<FadeManager>();

        //最前面になるよう適当なソートオーダー設定
        fadeCanvas.sortingOrder = 100;

        //フェード用のImage生成
        fadeImage = new GameObject("ImageFade").AddComponent<Image>();
        fadeImage.transform.SetParent(fadeCanvas.transform, false);
        fadeImage.rectTransform.anchoredPosition = Vector3.zero;

        //Imageサイズは適当に大きく設定
        fadeImage.rectTransform.sizeDelta = new Vector2(9999, 9999);
    }

    //フェードイン開始
    public static void FadeIn()
    {
        if (fadeImage == null) Init();
        fadeImage.color = Color.black;
        IsFadeIn = true;
    }

    //フェードアウト開始
    //引数は遷移したいシーンのシーン番号
    public static void FadeOut(int n)
    {
        if (fadeImage == null) Init();
        nextScene = n;
        fadeImage.color = Color.clear;
        fadeCanvas.enabled = true;
        IsFadeOut = true;
    }

    void Update()
    {
        //フラグ有効なら毎フレームフェードイン/アウト処理
        if (IsFadeIn)
        {
            //経過時間から透明度計算
            alpha -= Time.deltaTime / fadeTime;

            //フェードイン終了判定
            if (alpha <= 0.0f)
            {
                IsFadeIn = false;
                alpha = 0.0f;
                fadeCanvas.enabled = false;
            }

            //フェード用Imageの色・透明度設定
            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
        else if (IsFadeOut)
        {
            //経過時間から透明度計算
            alpha += Time.deltaTime / fadeTime;

            //フェードアウト終了判定
            if (alpha >= 1.0f)
            {
                IsFadeOut = false;
                alpha = 1.0f;

                //次のシーンへ遷移
                SceneManager.LoadScene(nextScene);
            }

            //フェード用Imageの色・透明度設定
            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
    }

}
