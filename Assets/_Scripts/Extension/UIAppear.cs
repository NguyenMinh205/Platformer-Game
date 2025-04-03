using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace _Scripts.Extension
{
	public enum EAppearType
	{
		scale, move, rotate
	}

	[System.Serializable]
	public class AppearAnim
	{
		public EAppearType appearType;
		public Ease ease;
		public Ease closeEase;
		public AnimationCurve appearAC;
		public Vector3 startValue = new Vector3(0, 0, 1);
		public RotateMode rotateMode;
		public Tweener tweener;

		public void Stop()
		{
			tweener.Kill();
		}
	}

	public class UIAppear : MonoBehaviour
	{
		public float appearDelay;
		public float duration = .5f;
		public float closeDuration = 0f;
		[SerializeField] bool fading = false;
		[SerializeField] AppearAnim[] anims;
		[SerializeField] CanvasGroup canvasGroup;
		[SerializeField] UnityEvent onStartEvt, onFinishEvt;

		Vector3 originPos;

		void Reset()
		{
			if (canvasGroup == null)
				canvasGroup = GetComponent<CanvasGroup>();
			if (canvasGroup == null)
				canvasGroup = gameObject.AddComponent<CanvasGroup>();


		}

		void Awake()
		{
			originPos = transform.localPosition;
		}

		void OnEnable()
		{
			Appear();
		}

		public void SetAnims(AppearAnim[] anims)
		{
			this.anims = anims;
		}

		public void ChangePos(Vector2 localPos)
		{
			originPos = localPos;
		}

		void OnDisable()
		{
			StopAllCoroutines();
			for (int i = 0; i < anims.Length; i++)
			{
				anims[i].Stop();
			}
		}

		public void Appear(UnityAction appearedCallback = null, float appearedCallbackDelay = 0)
		{
			if (!gameObject.activeInHierarchy)
				gameObject.SetActive(true);
			StartCoroutine(appearing(appearedCallback, appearedCallbackDelay));
		}

		public void SetOnStartEvent(UnityEvent e)
		{
			onStartEvt = e;
		}

		public void SetOnFinishEvent(UnityEvent e)
		{
			onFinishEvt = e;
		}

		IEnumerator appearing(UnityAction appearedCallback, float appearedCallbackDelay)
		{
			// prepare
			float a = 0;
			canvasGroup.alpha = a;

			for (int i = 0; i < anims.Length; i++)
			{
				if (anims[i].appearType == EAppearType.move)
					transform.localPosition += anims[i].startValue;
				else if (anims[i].appearType == EAppearType.scale)
					transform.localScale = anims[i].startValue;
				else if (anims[i].appearType == EAppearType.rotate)
					transform.localEulerAngles = anims[i].startValue;
			}

			float t = appearDelay;
			while (t > 0)
			{
				t -= Time.unscaledDeltaTime;
				yield return null;
			}

			// if (playAppearClip)
			//     SoundManager.instance.PlayRndSfx(GameManager.instance.gameData.btnAppearClips);

			//start
			if (onStartEvt != null)
				onStartEvt.Invoke();

			//playing
			for (int i = 0; i < anims.Length; i++)
			{
				if (anims[i].appearType == EAppearType.move)
				{
					if (anims[i].ease == Ease.Unset)
						anims[i].tweener = transform.DOLocalMove(originPos, duration).SetEase(anims[i].appearAC).SetUpdate(true);
					else
						anims[i].tweener = transform.DOLocalMove(originPos, duration).SetEase(anims[i].ease).SetUpdate(true);
				}
				else if (anims[i].appearType == EAppearType.scale)
				{
					transform.localScale = anims[i].startValue;
					if (anims[i].ease == Ease.Unset)
						anims[i].tweener = transform.DOScale(Vector3.one, duration).SetEase(anims[i].appearAC).SetUpdate(true);
					else
						anims[i].tweener = transform.DOScale(Vector3.one, duration).SetEase(anims[i].ease).SetUpdate(true);
				}
				else if (anims[i].appearType == EAppearType.rotate)
				{
					transform.localEulerAngles = anims[i].startValue;
					if (anims[i].ease == Ease.Unset)
						anims[i].tweener = transform.DOLocalRotate(Vector3.zero, duration, anims[i].rotateMode).SetEase(anims[i].appearAC).SetUpdate(true);
					else
						anims[i].tweener = transform.DOLocalRotate(Vector3.zero, duration, anims[i].rotateMode).SetEase(anims[i].ease).SetUpdate(true);
				}
			}

			// finish
			t = duration;
			if (!fading) a = 1f;
			canvasGroup.alpha = a;

			while (t > 0)
			{
				a = Mathf.MoveTowards(a, 1f, 2f / duration * Time.unscaledDeltaTime);
				canvasGroup.alpha = a;
				t -= Time.unscaledDeltaTime;
				yield return null;
			}
			canvasGroup.alpha = 1;
			if (onFinishEvt != null)
				onFinishEvt.Invoke();


			t = appearedCallbackDelay;
			while (t > 0)
			{
				t -= Time.unscaledDeltaTime;
				yield return null;
			}
			if (appearedCallback != null)
				appearedCallback();

		}

		public void _Close(bool disable = false)
		{
			if (!gameObject.activeInHierarchy) return;

			StopAllCoroutines();
			StartCoroutine(disappearing(null, disable));
		}

		public void Close(UnityAction onClosedCallback, bool disable = false, float delay = 0)
		{
			if (!gameObject.activeInHierarchy) return;

			StopAllCoroutines();
			StartCoroutine(disappearing(onClosedCallback, disable, delay));
		}

		IEnumerator disappearing(UnityAction onClosedCallback, bool disable, float delay = 0)
		{
			float t = delay;
			while (t > 0)
			{
				t -= Time.unscaledDeltaTime;
				yield return null;
			}

			//playing
			for (int i = 0; i < anims.Length; i++)
			{
				if (anims[i].appearType == EAppearType.move)
				{
					anims[i].tweener = transform.DOLocalMove(originPos + anims[i].startValue, closeDuration).SetEase(anims[i].closeEase).SetUpdate(true);
				}
				else if (anims[i].appearType == EAppearType.scale)
				{
					anims[i].tweener = transform.DOScale(anims[i].startValue, closeDuration).SetEase(anims[i].closeEase).SetUpdate(true);
				}
				else if (anims[i].appearType == EAppearType.rotate)
				{
					anims[i].tweener = transform.DOLocalRotate(anims[i].startValue, closeDuration).SetEase(anims[i].closeEase).SetUpdate(true);
				}
			}

			// finish
			// if (fading)
			// 	t = closeDuration;
			// else
			// 	t = 0;
			t = closeDuration;
			float a = canvasGroup.alpha;

			while (t > 0)
			{
				if (fading)
				{
					a = Mathf.MoveTowards(a, 0f, 1f / closeDuration * Time.unscaledDeltaTime);
					canvasGroup.alpha = a;
				}
				
				t -= Time.unscaledDeltaTime;
				yield return null;
			}
			canvasGroup.alpha = 0;
			if (disable)
				gameObject.SetActive(false);
			if (onClosedCallback != null)
				onClosedCallback();
		}

	}
}