using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;

#region Unity
public static class Utility
{
    public static WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    #region Transforms

    #region Move

    public static void Move(this Transform transform, Vector3 trgPos, float duration, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(MoveGameObjectRoutine(transform, trgPos, duration, animationCurve));
    }
    private static IEnumerator MoveGameObjectRoutine(Transform transform, Vector3 trgPos, float duration, AnimationCurve animationCurve)
    {
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        Vector3 startPos = transform.position;
        for (int i = 1; i <= steps; i++)
        {
            progress = (float)i / (steps);
            transform.position = Vector3.Lerp(startPos, trgPos, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }

    public static IEnumerator MoveRoutine(this Transform transform, Vector3 trgPos, float duration, AnimationCurve animationCurve = null)
    {
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        Vector3 startPos = transform.position;
        for (int i = 1; i <= steps; i++)
        {
            progress = (float)i / (steps);
            transform.position = Vector3.Lerp(startPos, trgPos, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }

    #endregion

    #region Rotation

    public static void Rotate(this Transform transform, Quaternion trgRotation, float duration, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(RotateGameObjectRoutine(transform, trgRotation, duration, animationCurve));
    }
    private static IEnumerator RotateGameObjectRoutine(Transform transform, Quaternion trgRotation, float duration, AnimationCurve animationCurve)
    {
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        Quaternion startRotation = transform.rotation;
        for (int i = 1; i <= steps; i++)
        {
            progress = (float)i / steps;
            transform.rotation = Quaternion.Lerp(startRotation, trgRotation, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }

    public static IEnumerator RotateRoutine(this Transform transform, Quaternion trgRotation, float duration, AnimationCurve animationCurve = null)
    {
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        Quaternion startRotation = transform.rotation;
        for (int i = 1; i <= steps; i++)
        {
            progress = (float)i / steps;
            transform.rotation = Quaternion.Lerp(startRotation, trgRotation, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }

    #endregion

    #region Scale

    public static void Scale(this Transform transform, Vector3 targetScale, float duration, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(ScaleGameObjectRoutine(transform, targetScale, duration, animationCurve));
    }
    private static IEnumerator ScaleGameObjectRoutine(Transform transform, Vector3 targetScale, float duration, AnimationCurve animationCurve)
    {
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        Vector3 startScale = transform.localScale;
        for (int i = 1; i <= steps; i++)
        {
            progress = (float)i / steps;
            transform.localScale = Vector3.Lerp(startScale, targetScale, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }

    public static IEnumerator ScaleRoutine(this Transform transform, Vector3 targetScale, float duration, AnimationCurve animationCurve = null)
    {
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        Vector3 startScale = transform.localScale;
        for (int i = 1; i <= steps; i++)
        {
            progress = (float)i / steps;
            transform.localScale = Vector3.Lerp(startScale, targetScale, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }

    #endregion

    #endregion

    #region Image/Sprite/Text/Materials

    public static void SimpleAnimate(this SpriteRenderer spriteRend, float intervall, MonoBehaviour monoBehaviour, params Sprite[] sprites)
    {
        monoBehaviour.StartCoroutine(SimpleAnimateCR(spriteRend, intervall, sprites));
    }
    private static IEnumerator SimpleAnimateCR(SpriteRenderer spriteRend, float intervall, params Sprite[] sprites)
    {
        int index = 0;
        while (true)
        {
            spriteRend.sprite = sprites[index];
            ++index;
            if (index > sprites.Length - 1)
                index = 0;
            yield return new WaitForSeconds(intervall);
        }
    }

    public static IEnumerator SimpleAnimateRoutine(this SpriteRenderer spriteRend, float intervall, params Sprite[] sprites)
    {
        int index = 0;
        while (true)
        {
            spriteRend.sprite = sprites[index];
            ++index;
            if (index > sprites.Length - 1)
                index = 0;
            yield return new WaitForSeconds(intervall);
        }
    }

    public static void LerpColor(this Image image, Color targetColor, float duration, bool ignoreTimeScale, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(_LerpColorRoutine(image, targetColor, duration, ignoreTimeScale, animationCurve));
    }
    private static IEnumerator _LerpColorRoutine(Image image, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        image.gameObject.SetActive(true);
        image.enabled = true;
        Color startingColor = image.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                image.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                image.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }

    public static IEnumerator LerpColorRoutine(this Image image, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        image.gameObject.SetActive(true);
        image.enabled = true;
        Color startingColor = image.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                image.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                image.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }


    public static void LerpColor(this SpriteRenderer image, Color targetColor, float duration, bool ignoreTimeScale, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(_LerpColorRoutine(image, targetColor, duration, ignoreTimeScale, animationCurve));
    }
    private static IEnumerator _LerpColorRoutine(SpriteRenderer image, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        image.gameObject.SetActive(true);
        image.enabled = true;
        Color startingColor = image.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                image.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                image.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }

    public static IEnumerator LerpColorRoutine(this SpriteRenderer image, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        image.gameObject.SetActive(true);
        image.enabled = true;
        Color startingColor = image.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                image.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                image.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }


    public static void LerpColor(this TMPro.TextMeshProUGUI text, Color targetColor, float duration, bool ignoreTimeScale, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(_LerpColorRoutine(text, targetColor, duration, ignoreTimeScale, animationCurve));
    }
    private static IEnumerator _LerpColorRoutine(TMPro.TextMeshProUGUI text, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        Color startingColor = text.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                text.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                text.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }

    public static IEnumerator LerpColorRoutine(this TMPro.TextMeshProUGUI text, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        text.gameObject.SetActive(true);
        text.enabled = true;
        Color startingColor = text.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                text.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                text.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }


    public static void LerpColor(this TMPro.TextMeshPro text, Color targetColor, float duration, bool ignoreTimeScale, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(_LerpColorRoutine(text, targetColor, duration, ignoreTimeScale, animationCurve));
    }
    private static IEnumerator _LerpColorRoutine(TMPro.TextMeshPro text, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        Color startingColor = text.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                text.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                text.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }

    public static IEnumerator LerpColorRoutine(this TMPro.TextMeshPro text, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        text.gameObject.SetActive(true);
        text.enabled = true;
        Color startingColor = text.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                text.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                text.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }


    public static void LerpColor(this MeshRenderer meshRenderer, Color targetColor, float duration, bool ignoreTimeScale, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(_LerpColorRoutine(meshRenderer, targetColor, duration, ignoreTimeScale, animationCurve));
    }
    private static IEnumerator _LerpColorRoutine(MeshRenderer meshRenderer, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        Material mat = meshRenderer.material;
        Color startingColor = mat.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                mat.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                mat.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }

    public static IEnumerator LerpColorRoutine(this MeshRenderer meshRenderer, Color targetColor, float duration, bool ignoreTimeScale, AnimationCurve animationCurve)
    {
        Material mat = meshRenderer.material;
        Color startingColor = mat.color;
        float progress = 0f;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);

        if (ignoreTimeScale)
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                mat.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        else
        {
            for (int i = 1; i <= steps; i++)
            {
                progress = (float)i / steps;
                mat.color = Color.Lerp(startingColor, targetColor, animationCurve == null ? progress : animationCurve.Evaluate(progress));
                yield return waitForFixedUpdate;
            }
        }
    }

    #endregion

    #region Audio

    public static void LerpVolume(this AudioSource source, float targetVolume, float duration, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(LerpAudioVolumeCR(source, targetVolume, duration, animationCurve));
    }
    private static IEnumerator LerpAudioVolumeCR(AudioSource source, float targetVolume, float duration, AnimationCurve animationCurve)
    {
        float startingValue = source.volume;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        for (int i = 0; i < steps; i++)
        {
            progress = (float)i / steps;
            source.volume = Mathf.Lerp(startingValue, targetVolume, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }

    public static IEnumerator LerpVolumeRoutine(this AudioSource source, float targetVolume, float duration, AnimationCurve animationCurve = null)
    {
        float startingValue = source.volume;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        for (int i = 0; i < steps; i++)
        {
            progress = (float)i / steps;
            source.volume = Mathf.Lerp(startingValue, targetVolume, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }


    public static void LerpPitch(this AudioSource source, float targetVolume, float duration, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(LerpPitchCR(source, targetVolume, duration, animationCurve));
    }
    private static IEnumerator LerpPitchCR(AudioSource source, float targetPitch, float duration, AnimationCurve animationCurve)
    {
        float startingValue = source.pitch;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        for (int i = 0; i < steps; i++)
        {
            progress = (float)i / steps;
            source.pitch = Mathf.Lerp(startingValue, targetPitch, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }

    public static IEnumerator LerpPitchRoutine(this AudioSource source, float targetPitch, float duration, AnimationCurve animationCurve = null)
    {
        float startingValue = source.volume;
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        for (int i = 0; i < steps; i++)
        {
            progress = (float)i / steps;
            source.pitch = Mathf.Lerp(startingValue, targetPitch, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
    }

    #endregion

    #region SceneManagement
    public static void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void TransitionToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public static void TransitionToScene(string sceneName, MonoBehaviour monoBehaviour, float delay = 0f)
    {
        monoBehaviour.StartCoroutine(SceneTransition(sceneName, delay));
    }
    public static void TransitionToScene(int sceneIndex, MonoBehaviour monoBehaviour, float delay = 0f)
    {
        monoBehaviour.StartCoroutine(SceneTransition(sceneIndex, delay));
    }
    private static IEnumerator SceneTransition(string sceneName, float delayBeforeTrans)
    {
        yield return new WaitForSeconds(delayBeforeTrans);
        SceneManager.LoadScene(sceneName);
    }
    private static IEnumerator SceneTransition(int sceneIndex, float delayBeforeTrans)
    {
        yield return new WaitForSeconds(delayBeforeTrans);
        SceneManager.LoadScene(sceneIndex);
    }
    #endregion

    #region Other

    public static bool RoughlyEqual(this Vector3 pos1, Vector3 pos2, float tolerance = 0.01f)
    {
        if (Vector3.Distance(pos1, pos2) <= tolerance)
            return true;
        else
            return false;
    }

    public static bool RoughlyEqual(this Vector2 pos1, Vector2 pos2, float tolerance = 0.01f)
    {
        if (Vector2.Distance(pos1, pos2) <= tolerance)
            return true;
        else
            return false;
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    #endregion
}
#endregion

#region CollectionExtensions
public static class CollectionExtentions
{
    // The method to retrieve all matching objects in a sorted or unsorted List<T>
    public static IEnumerable<T> GetAll<T>(this List<T> myList, T searchValue) => myList.Where(t => t.Equals(searchValue));
    // Count the number of times an item appears in this unsorted or sorted List<T>
    public static int CountAll<T>(this List<T> myList, T searchValue) => myList.GetAll(searchValue).Count();

    // The method to retrieve all matching objects in a sorted List<T>
    public static T[] BinarySearchGetAll<T>(this List<T> myList, T searchValue)
    {
        List<T> retObjs = new List<T>();
        int center = myList.BinarySearch(searchValue);
        if (center > 0)
        {
            retObjs.Add(myList[center]);
            int left = center;
            while (left > 0 && myList[left - 1].Equals(searchValue))
            {
                left -= 1;
                retObjs.Add(myList[left]);
            }
            int right = center;
            while (right < (myList.Count - 1) &&
            myList[right + 1].Equals(searchValue))
            {
                right += 1;
                retObjs.Add(myList[right]);
            }
        }
        return (retObjs.ToArray());
    }
    // Count the number of times an item appears in this sorted List<T>
    public static int BinarySearchCountAll<T>(this List<T> myList, T searchValue) => BinarySearchGetAll(myList, searchValue).Count();

    // Get last element in collection.
    public static T LastElement<T>(this ICollection<T> array)
    {
        if (array == null)
            throw new NullReferenceException("GetLastElement: array is null!");
        int length = array.Count;
        if (length == 0)
            throw new Exception("GetLastElement: array has no elements!");
        return array.ElementAt(length - 1);
    }

}
#endregion

#region Random
static class RandomExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void Shuffle<T>(this Container<T> container)
    {
        List<T> list = container.ToList();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        container.FromList(list);
    }

    public static T RandomElement<T>(this ICollection<T> list)
    {
        return list.ElementAt(rng.Next(list.Count));
    }
}
#endregion

#region CostumContainer
public class Container<T> : IEnumerable<T>
{
    public Container() { }
    private List<T> _internalList = new List<T>();
    // This iterator iterates over each element from first to last
    public IEnumerator<T> GetEnumerator() => _internalList.GetEnumerator();
    // This iterator iterates over each element from last to first
    public IEnumerable<T> GetReverseOrderEnumerator()
    {
        foreach (T item in ((IEnumerable<T>)_internalList).Reverse())
            yield return item;
    }
    // This iterator iterates over each element from first to last, stepping
    // over a predefined number of elements
    public IEnumerable<T> GetForwardStepEnumerator(int step)
    {
        foreach (T item in _internalList.EveryNthItem(step))
            yield return item;
    }
    // This iterator iterates over each element from last to first, stepping
    // over a predefined number of elements
    public IEnumerable<T> GetReverseStepEnumerator(int step)
    {
        foreach (T item in ((IEnumerable<T>)_internalList).Reverse().EveryNthItem(step))
            yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Clear()
    {
        _internalList.Clear();
    }
    public void Add(T item)
    {
        _internalList.Add(item);
    }

    public void AddRange(ICollection<T> collection)
    {
        _internalList.AddRange(collection);
    }
}

public static class ContainerExtension
{
    public static IEnumerable<T> EveryNthItem<T>(this IEnumerable<T> enumerable, int step)
    {
        int current = 0;
        foreach (T item in enumerable)
        {
            ++current;
            if (current % step == 0)
                yield return item;
        }
    }

    public static void FromList<T>(this Container<T> container, List<T> list)
    {
        container.Clear();
        foreach (T item in list) container.Add(item);
    }
}
#endregion
