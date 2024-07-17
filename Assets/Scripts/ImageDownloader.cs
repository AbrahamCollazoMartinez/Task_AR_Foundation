using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Net.Http;
using System.IO;
using UnityEngine.Events;


//the comment code at this script is to expand the behaviour and adapt the needs of the project, despite is a technical task
public class ImageDownloader : MonoBehaviour
{
	[SerializeField] private string imageUrl;
	// public Image targetImage;
	// public Slider loadingBar;
	[SerializeField] private UnityEvent<string> onImageStartsLoad, onImageLoaded, onImageLoadError;

	private void Start()
	{
		DownloadImageAsync().Forget();
	}

	public void TryDownloadImage()
	{
		DownloadImageAsync().Forget();
	}

	private async UniTaskVoid DownloadImageAsync()
	{
		onImageStartsLoad?.Invoke(imageUrl);
		try
		{
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(imageUrl);
				response.EnsureSuccessStatusCode();

				var totalBytes = response.Content.Headers.ContentLength ?? 0;
				var receivedBytes = 0L;

				using (var stream = await response.Content.ReadAsStreamAsync())
				{
					var buffer = new byte[4096];
					int bytesRead;

					while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
					{
						receivedBytes += bytesRead;
						//UpdateLoadingBar((float)receivedBytes / totalBytes);
						await UniTask.Yield();
					}
				}

				var texture = new Texture2D(2, 2, TextureFormat.RG32, false);
				texture.LoadImage(response.Content.ReadAsByteArrayAsync().Result);

				//var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
				//SetImageWithAspectRatio(sprite);
				GlobalVariables.Instance.texture2Ds.Add(texture);
				onImageLoaded.Invoke(imageUrl);
			}
		}
		catch (System.Exception ex)
		{
			Debug.Log($"Error downloading image: {ex.Message}");
			onImageLoadError.Invoke(imageUrl);
		}
	}

	// private void UpdateLoadingBar(float progress)
	// {
	// 	loadingBar.value = progress;
	// }

	// private void SetImageWithAspectRatio(Sprite sprite)
	// {
	// 	var imageRectTransform = targetImage.GetComponent<RectTransform>();
	// 	var spriteAspectRatio = (float)sprite.texture.width / sprite.texture.height;
	// 	var imageAspectRatio = imageRectTransform.rect.width / imageRectTransform.rect.height;

	// 	if (spriteAspectRatio > imageAspectRatio)
	// 	{
	// 		imageRectTransform.sizeDelta = new Vector2(imageRectTransform.rect.width, imageRectTransform.rect.width / spriteAspectRatio);
	// 	}
	// 	else
	// 	{
	// 		imageRectTransform.sizeDelta = new Vector2(imageRectTransform.rect.height * spriteAspectRatio, imageRectTransform.rect.height);
	// 	}

	// 	targetImage.sprite = sprite;
	// }
}