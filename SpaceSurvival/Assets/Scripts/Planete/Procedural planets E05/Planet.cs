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
    public int resolutionMeshPerFace = 5;
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
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        if (parentFacesTransform == null || parentFacesTransform.Length != 6)
        {
            parentFacesTransform = new Transform[6];
        }
        for (int i = 0; i < 6; i++)
        {
            if (parentFacesTransform[i] != null)
            {
                DestroyImmediate(parentFacesTransform[i].gameObject);
            }
            parentFacesTransform[i] = new GameObject(i + "_Face").transform;
            parentFacesTransform[i].transform.parent = this.transform;
        }

        int nbMeshPerFace = resolutionMeshPerFace * resolutionMeshPerFace;
        
        //Création tableaux de meshs et de transforms
        meshFilters = new MeshFilter[nbMeshPerFace * 6];
        meshCenterTransforms = new Transform[nbMeshPerFace * 6];
        terrainFaces = new TerrainFace[nbMeshPerFace * 6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < nbMeshPerFace; j++)
            {
                int index = j + (i * nbMeshPerFace);

                //Création du Transform qui servira de parent
                Transform meshCenterTransform = new GameObject(j +"_LOD_Group").transform;
                meshCenterTransform.parent = parentFacesTransform[i];

                //Création des GameObjects
                if (meshFilters[index] == null)
                {
                    GameObject meshObj = new GameObject("mesh" + i + "|" + j);
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
        OrganizeHierarchy();
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

    private void OrganizeHierarchy()
    {
        for (int i = 0; i < meshFilters.Length; i++)
        {
            meshFilters[i].transform.parent = meshCenterTransforms[i];
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
