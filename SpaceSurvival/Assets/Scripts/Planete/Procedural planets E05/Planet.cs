using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class Planet : MonoBehaviour
{

    public bool save = false;

    [Range(2, 255)]
    public int resolution = 10;
    public int resolutionMeshPerFace = 10; //ça marche bien avec 5, si on monte plus, ça marche plus ¯\_༼ ಥ ‿ ಥ ༽_/¯
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColourGenerator colourGenerator = new ColourGenerator();

    private Transform[] parentFacesTransform;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private Transform[] meshCenterTransforms;

    public delegate void PlanetGenerationEnded();
    public event PlanetGenerationEnded OnPlanetGenerationEnded;
    public MinMax GetMinMax { get { return shapeGenerator.elevationMinMax; } }

    public void GameStart()
    {
        faceRenderMask = FaceRenderMask.All;
        GeneratePlanet(false);
    }

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        if (parentFacesTransform == null || parentFacesTransform.Length != 6)
        {
            parentFacesTransform = new Transform[6];
        }
        for (int i = 0; i < 6; i++)
        {
            if (parentFacesTransform[i] == null)
            {
                parentFacesTransform[i] = new GameObject("Face " + i).transform;
                parentFacesTransform[i].transform.parent = this.transform;
            }
        }

        //Création tableaux de meshs et de transforms
        if (meshFilters == null || meshFilters.Length != resolutionMeshPerFace * resolutionMeshPerFace * 6)
        {
            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i] != null && meshFilters[i].gameObject != null)
                    DestroyImmediate(meshFilters[i].gameObject);
            }
            meshFilters = new MeshFilter[resolutionMeshPerFace * resolutionMeshPerFace * 6];
        }
        if (meshCenterTransforms == null || meshCenterTransforms.Length != resolutionMeshPerFace * resolutionMeshPerFace * 6)
        {
            if (meshCenterTransforms != null)
            {
                for (int i = 0; i < meshCenterTransforms.Length; i++)
                {
                    if (meshCenterTransforms[i] != null && meshCenterTransforms[i].gameObject != null)
                        DestroyImmediate(meshCenterTransforms[i].gameObject);
                }
            }
            meshCenterTransforms = new Transform[resolutionMeshPerFace * resolutionMeshPerFace * 6];
        }
        terrainFaces = new TerrainFace[resolutionMeshPerFace * resolutionMeshPerFace * 6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        int nbMeshPerFace = resolutionMeshPerFace * resolutionMeshPerFace;

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < nbMeshPerFace; j++)
            {
                int index = j + (i * nbMeshPerFace);

                //Création du Transform qui servira de parent
                Transform meshCenterTransform = new GameObject("Center" + i + "|" + j).transform;
                //meshCenterTransform.parent = parentFacesTransform[i];

                //Création des GameObjects
                if (meshFilters[index] == null)
                {
                    GameObject meshObj = new GameObject("mesh" + i + "|" + j);
                    meshObj.transform.parent = parentFacesTransform[i];
                    //meshObj.transform.parent = meshCenterTransform;
                    meshObj.tag = "Ground";
                    meshObj.layer = LayerMask.NameToLayer("Ground");

                    meshObj.AddComponent<MeshCollider>();
                    meshObj.AddComponent<MeshRenderer>();
                    meshFilters[index] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[index].sharedMesh = new Mesh();
                }

                meshFilters[index].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;


                terrainFaces[index] = new TerrainFace(shapeGenerator, meshFilters[index].sharedMesh, resolution, directions[i], j, resolutionMeshPerFace, meshCenterTransform);
                meshCenterTransforms[index] = meshCenterTransform;


                //DestroyImmediate(meshCenter.gameObject);
                //Mettre terrain face en parent de la face et vérifier sa position à ce moment.
                //    Ne pas le faire ici, le faire avec la génération des couleurs
                bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
                meshFilters[index].gameObject.SetActive(renderFace);
            }
        }
    }

    public void GeneratePlanet(bool inEditor)
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
        GenerateMeshColliders();
        SetHideFlags();
        if (!inEditor)
            OnPlanetGenerationEnded();
    }

    private void GenerateMeshColliders()
    {
        for (int i = 0; i < meshFilters.Length; i++)
        {
            MeshCollider col = meshFilters[i].GetComponent<MeshCollider>();
            col.sharedMesh = null;
            col.sharedMesh = meshFilters[i].sharedMesh;
        }
    }

    private void SetHideFlags()
    {
        for (int i = 0; i < parentFacesTransform.Length; i++)
        {
            if (save)
                parentFacesTransform[i].gameObject.hideFlags = HideFlags.None;
            else
                parentFacesTransform[i].gameObject.hideFlags = HideFlags.DontSave;
        }
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (save)
                meshFilters[i].gameObject.hideFlags = HideFlags.None;
            else
                meshFilters[i].gameObject.hideFlags = HideFlags.DontSave;
        }
        for (int i = 0; i < meshCenterTransforms.Length; i++)
        {
            if (save)
                meshCenterTransforms[i].gameObject.hideFlags = HideFlags.None;
            else
                meshCenterTransforms[i].gameObject.hideFlags = HideFlags.DontSave;
        }
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColours()
    {
        colourGenerator.UpdateColours();
    }

    [ContextMenu("RandomPlanete")]
    public void RandomPlanete()
    {
        int min = 0;
        int max = 255;

        foreach (var noiseLayer in shapeSettings.noiseLayers)
        {
            noiseLayer.noiseSettings.simpleNoiseSettings.centre = new Vector3(Aleatoire.AleatoireBetween(min, max), Aleatoire.AleatoireBetween(min, max), Aleatoire.AleatoireBetween(min, max));
            noiseLayer.noiseSettings.ridgidNoiseSettings.centre = new Vector3(Aleatoire.AleatoireBetween(min, max), Aleatoire.AleatoireBetween(min, max), Aleatoire.AleatoireBetween(min, max));
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
