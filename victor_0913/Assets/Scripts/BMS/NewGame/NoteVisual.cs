using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Because we use NGUI systems so all the UI element we can directly code based on reference of them in the scene
/// This class is only containing all the methods and variable of the visual of the note
/// </summary>
public class NoteVisual : MonoBehaviour
{
    [Header("Note object visual")]
    //Note object elements
    public Transform noteObject;
    public Transform lineHolder;
    public UISprite[] lineArray;
    public Transform cornerHolder;
    public Transform[] cornerArray;
    public Transform directionHolder;
    public Transform[] directionArray;
    [Header("Screen button visual")]
    //Screen button elements
    public Transform screenButton;
    public Transform screenStickHolder;
    public Transform[] screenStickArray;

    public ParticleSystem blastParticle;
    public AnimatedAlpha noteAnimated;
    public AnimatedAlpha screenBtnAnimated;
    public UISprite[] glowingEffect;

    public Transform fadingSpriteTransform;
    //Props

    public UIEventTrigger UIEventTrigger { get => screenButton.GetComponent<UIEventTrigger>(); }
    public UISprite ScreenSprite { get => screenButton.GetComponent<UISprite>(); }
    public Vector3 NotePosition { get => noteObject.localPosition; set => noteObject.localPosition = value; }
    public Vector3 ScreenButtonPosition { get => screenButton.localPosition; set => screenButton.localPosition = value; }
    public bool playingAnimation = false;

    public void SetUp()
    {
        noteObject.gameObject.SetActive(false);
        screenButton.gameObject.SetActive(false);
        screenBtnAnimated.alpha = 0f;
        noteAnimated.alpha = 0f;
    }

    public void PlayAnimation(bool result)
    {
        noteObject.gameObject.SetActive(false);

        StartCoroutine(Animation(result));
        //OnAnimationStart();

    }

    /// <summary>
    /// Based on result the animation will act differently
    /// </summary>
    /// <param name="result">The result when player play the note</param>
    /// <returns></returns>
    private IEnumerator Animation(bool result)
    {
        playingAnimation = true;
        //turn off collider
        BoxCollider box = screenButton.GetComponent<BoxCollider>();
        box.enabled = false;

        //play particle
        blastParticle.transform.position = screenButton.position;
        blastParticle.Play();
        fadingSpriteTransform.localPosition = ScreenButtonPosition;

        float eslapsedTime = 0f;
        float animationTime = 0.2f;
        float fromWidth = 90f, fromHeight = 90f;
        float toWidth = 130f, toHeight = 130f;
        AnimatedAlpha animatedAlpha = fadingSpriteTransform.GetComponent<AnimatedAlpha>();
        UISprite sprite = screenButton.GetComponent<UISprite>();
        UISprite fadingSprite = fadingSpriteTransform.GetComponent<UISprite>();

        //change color based on result
        fadingSprite.color = result ? Color.white : Color.red;
        //lerping
        while (eslapsedTime < animationTime)
        {
            Debug.Log("Animation");
            eslapsedTime += Time.deltaTime;

            sprite.width = (int)Mathf.Lerp(fromWidth, toWidth, eslapsedTime / animationTime);
            sprite.height = (int)Mathf.Lerp(fromHeight, toHeight, eslapsedTime / animationTime);
            // screenButton.localScale = Vector3.Lerp(screenButton.localScale, new Vector3(1.5f, 1.5f, 1f), eslapsedTime / animationTime);

            fadingSprite.width = (int)Mathf.Lerp(fromWidth, toWidth, eslapsedTime / animationTime);
            fadingSprite.height = (int)Mathf.Lerp(fromHeight, toHeight, eslapsedTime / animationTime);

            animatedAlpha.alpha = Mathf.Lerp(0f, 0.5f, eslapsedTime / animationTime);
            yield return null;
        }
        
        //reset values
        eslapsedTime = 0f;
        box.enabled = true;
        sprite.width = (int)fromWidth;
        sprite.height = (int)fromHeight;
        fadingSprite.width = (int)fromWidth;
        fadingSprite.height = (int)fromHeight;
        animatedAlpha.alpha = 0f;
        playingAnimation = false;
    }

}
