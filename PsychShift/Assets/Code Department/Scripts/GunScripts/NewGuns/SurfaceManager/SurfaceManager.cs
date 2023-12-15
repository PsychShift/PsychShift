using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ImpactSystem.Effects;
using ImpactSystem.Pool;

namespace ImpactSystem
{
    [RequireComponent(typeof(SpecialEffectManager))]
    public class SurfaceManager : MonoBehaviour
    {
        private static SurfaceManager _instance;
        public static SurfaceManager Instance
        {
            get
            {
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one SurfaceManager active in the scene! Destroying latest one: " + name);
                Destroy(gameObject);
                return;
            }

            Instance = this;

        }
        [SerializeField]
        private SpecialEffectManager specialEffectManager;

        [SerializeField]
        private List<SurfaceType> Surfaces = new List<SurfaceType>();
        [SerializeField]
        private Surface DefaultSurface;
        private Dictionary<GameObject, ObjectPool<GameObject>> ObjectPools = new();

        public void HandleImpact(GameObject HitObject, Vector3 HitPoint, Vector3 HitNormal, ImpactType Impact, int TriangleIndex)
        {
            if (!Impact.CheckForRenderer)
            {
                foreach (Surface.SurfaceImpactTypeEffect typeEffect in DefaultSurface.ImpactTypeEffects)
                {
                    if (typeEffect.ImpactType == Impact)
                    {
                        PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, 1);
                        break;
                    }
                }
            }
            else if (HitObject.TryGetComponent(out Renderer renderer))
            {
                Texture activeTexture = GetActiveTextureFromRenderer(renderer, TriangleIndex);

                SurfaceType surfaceType = Surfaces.Find(surface => surface.Albedo == activeTexture);
                if (surfaceType != null)
                {
                    foreach (Surface.SurfaceImpactTypeEffect typeEffect in surfaceType.Surface.ImpactTypeEffects)
                    {
                        if (typeEffect.ImpactType == Impact)
                        {
                            PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, 1);
                        }
                    }
                }
                else
                {
                    foreach (Surface.SurfaceImpactTypeEffect typeEffect in DefaultSurface.ImpactTypeEffects)
                    {
                        if (typeEffect.ImpactType == Impact)
                        {
                            PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, 1);
                        }
                    }
                }
            }
        }

        private Texture GetActiveTextureFromRenderer(Renderer Renderer, int TriangleIndex)
        {
            if (Renderer.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
            {
                Mesh mesh = meshFilter.mesh;

                return GetTextureFromMesh(mesh, TriangleIndex, Renderer.sharedMaterials);
            }
            else if (Renderer is SkinnedMeshRenderer)
            {
                SkinnedMeshRenderer smr = (SkinnedMeshRenderer)Renderer;
                Mesh mesh = smr.sharedMesh;

                return GetTextureFromMesh(mesh, TriangleIndex, Renderer.sharedMaterials);
            }

            Debug.LogError($"{Renderer.name} has no MeshFilter or SkinnedMeshRenderer! Using default impact effect instead of texture-specific one because we'll be unable to find the correct texture!");
            return null;
        }

        private Texture GetTextureFromMesh(Mesh Mesh, int TriangleIndex, Material[] Materials)
        {
            if (Mesh.subMeshCount > 1)
            {
                int[] hitTriangleIndices = new int[]
                {
                    Mesh.triangles[TriangleIndex * 3],
                    Mesh.triangles[TriangleIndex * 3 + 1],
                    Mesh.triangles[TriangleIndex * 3 + 2]
                };

                for (int i = 0; i < Mesh.subMeshCount; i++)
                {
                    int[] submeshTriangles = Mesh.GetTriangles(i);
                    for (int j = 0; j < submeshTriangles.Length; j += 3)
                    {
                        if (submeshTriangles[j] == hitTriangleIndices[0]
                            && submeshTriangles[j + 1] == hitTriangleIndices[1]
                            && submeshTriangles[j + 2] == hitTriangleIndices[2])
                        {
                            return Materials[i].mainTexture;
                        }
                    }
                }
            }

            return Materials[0].mainTexture;
        }

        private void PlayEffects(Vector3 HitPoint, Vector3 HitNormal, SurfaceEffect SurfaceEffect, float SoundOffset)
        {
            foreach (SpawnObjectEffect spawnObjectEffect in SurfaceEffect.SpawnObjectEffects)
            {
                if (spawnObjectEffect.Probability > Random.value)
                {
                    if (!ObjectPools.ContainsKey(spawnObjectEffect.Prefab))
                    {
                        ObjectPools.Add(spawnObjectEffect.Prefab, new ObjectPool<GameObject>(() => Instantiate(spawnObjectEffect.Prefab)));
                    }

                    GameObject instance = ObjectPools[spawnObjectEffect.Prefab].Get();

                    if (instance.TryGetComponent(out PoolableObject poolable))
                    {
                        poolable.Parent = ObjectPools[spawnObjectEffect.Prefab];
                    }
                    instance.SetActive(true);
                    instance.transform.position = HitPoint + HitNormal * 0.001f;
                    instance.transform.forward = HitNormal;

                    if (spawnObjectEffect.RandomizeRotation)
                    {
                        Vector3 offset = new Vector3(
                            Random.Range(0, 180 * spawnObjectEffect.RandomizedRotationMultiplier.x),
                            Random.Range(0, 180 * spawnObjectEffect.RandomizedRotationMultiplier.y),
                            Random.Range(0, 180 * spawnObjectEffect.RandomizedRotationMultiplier.z)
                        );

                        instance.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + offset);
                    }
                }
            }

            foreach (PlayAudioEffect playAudioEffect in SurfaceEffect.PlayAudioEffects)
            {
                if(playAudioEffect.AudioClips.Count == 0) continue;
                if (!ObjectPools.ContainsKey(playAudioEffect.AudioSourcePrefab.gameObject))
                {
                    ObjectPools.Add(playAudioEffect.AudioSourcePrefab.gameObject, new ObjectPool<GameObject>(() => Instantiate(playAudioEffect.AudioSourcePrefab.gameObject)));
                }

                AudioClip clip = playAudioEffect.AudioClips[Random.Range(0, playAudioEffect.AudioClips.Count)];
                GameObject instance = ObjectPools[playAudioEffect.AudioSourcePrefab.gameObject].Get();
                instance.SetActive(true);
                AudioSource audioSource = instance.GetComponent<AudioSource>();

                audioSource.transform.position = HitPoint;
                audioSource.PlayOneShot(clip, SoundOffset * Random.Range(playAudioEffect.VolumeRange.x, playAudioEffect.VolumeRange.y));
                StartCoroutine(DisableAudioSource(ObjectPools[playAudioEffect.AudioSourcePrefab.gameObject], audioSource, clip.length));
            }

            foreach (SpawnSpecialEffect spawnSpecialEffect in SurfaceEffect.SpawnSpecialEffects)
            {
                Debug.Log("trying to play special effect");
                if (spawnSpecialEffect.Probability > Random.value)
                {
                    Debug.Log("playing special effect");
                    spawnSpecialEffect.SpecialEffect.PlaySpecialEffect(specialEffectManager);
                }
            }
        }

        private IEnumerator DisableAudioSource(ObjectPool<GameObject> Pool, AudioSource AudioSource, float Time)
        {
            yield return new WaitForSeconds(Time);

            AudioSource.gameObject.SetActive(false);
            Pool.Release(AudioSource.gameObject);
        }

        private class TextureAlpha
        {
            public float Alpha;
            public Texture Texture;
        }
    }
}