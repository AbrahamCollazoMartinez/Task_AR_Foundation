using System.Collections.Generic;
using System.Threading.Tasks;
using GLTFast;
using UnityEngine;
using UnityEngine.Events;

public class GLTFLoader_Custom_Abraham : MonoBehaviour
{
	[SerializeField] private string url;
	[SerializeField] private UnityEvent<string> onDownloadStart, onDownloadComplete, onDownloadError;

	async void Start()
	{
		Implementation_01();
	}

	public async void Implementation_01()
	{
		var gltf = new GLTFast.GltfImport();

		// Create a settings object and configure it accordingly
		var settings = new ImportSettings
		{
			GenerateMipMaps = true,
			AnisotropicFilterLevel = 3,
			NodeNameMethod = NameImportMethod.OriginalUnique
		};
		// Load the glTF and pass along the settings
		onDownloadStart?.Invoke(url);
		var success = await gltf.Load(url, settings);

		if (success)
		{
			//var gameObject = new GameObject("glTF");
			await gltf.InstantiateMainSceneAsync(this.gameObject.transform);
			GlobalVariables.Instance.objects_app.Add(gltf);
			onDownloadComplete?.Invoke(url);
		}
		else
		{
			Debug.LogError("Loading glTF failed!");
			onDownloadError?.Invoke(url);


		}
	}
	async void Implementation_02()
	{
		// First step: load glTF
		var gltf = new GLTFast.GltfImport();
		var success = await gltf.Load(url);

		if (success)
		{
			// Here you can customize the post-loading behavior

			// Get the first material
			var material = gltf.GetMaterial();
			Debug.LogFormat("The first material is called {0}", material.name);

			// Instantiate the glTF's main scene
			await gltf.InstantiateMainSceneAsync(new GameObject("Instance 1").transform);
			// Instantiate the glTF's main scene
			await gltf.InstantiateMainSceneAsync(new GameObject("Instance 2").transform);

			// Instantiate each of the glTF's scenes
			for (int sceneId = 0; sceneId < gltf.SceneCount; sceneId++)
			{
				await gltf.InstantiateSceneAsync(transform, sceneId);
			}
		}
		else
		{
			Debug.LogError("Loading glTF failed!");


		}
	}

	async void Implementation_03()
	{
		var gltfImport = new GltfImport();
		await gltfImport.Load(url);
		var instantiator = new GameObjectInstantiator(gltfImport, transform);
		var success = await gltfImport.InstantiateMainSceneAsync(instantiator);
		if (success)
		{

			// Get the SceneInstance to access the instance's properties
			var sceneInstance = instantiator.SceneInstance;

			// Enable the first imported camera (which are disabled by default)
			if (sceneInstance.Cameras is { Count: > 0 })
			{
				sceneInstance.Cameras[0].enabled = true;
			}

			// Decrease lights' ranges
			if (sceneInstance.Lights != null)
			{
				foreach (var glTFLight in sceneInstance.Lights)
				{
					glTFLight.range *= 0.1f;
				}
			}

			// Play the default (i.e. the first) animation clip
			var legacyAnimation = instantiator.SceneInstance.LegacyAnimation;
			if (legacyAnimation != null)
			{
				legacyAnimation.Play();
			}
		}
		else
		{
			//error

		}
	}

	private string[] manyUrls = new string[]
		{
		"file:///path/to/file1.gltf",
		"file:///path/to/file2.gltf",
		"file:///path/to/file3.gltf",
		"file:///path/to/file4.gltf",
		"file:///path/to/file5.gltf",
		"file:///path/to/file6.gltf",
		"file:///path/to/file7.gltf",
		"file:///path/to/file8.gltf",
		"file:///path/to/file9.gltf",
		};
	async Task CustomDeferAgentPerGltfImport()
	{
		// Recommended: Use a common defer agent across multiple GltfImport instances!
		// For a stable frame rate:
		IDeferAgent deferAgent = gameObject.AddComponent<TimeBudgetPerFrameDeferAgent>();
		// Or for faster loading:
		deferAgent = new UninterruptedDeferAgent();

		var tasks = new List<Task>();

		foreach (var url in manyUrls)
		{
			var gltf = new GLTFast.GltfImport(null, deferAgent);
			var task = gltf.Load(url).ContinueWith(
				async t =>
				{
					if (t.Result)
					{
						await gltf.InstantiateMainSceneAsync(transform);
					}
				},
				TaskScheduler.FromCurrentSynchronizationContext()
				);
			tasks.Add(task);
		}

		await Task.WhenAll(tasks);
	}


}
