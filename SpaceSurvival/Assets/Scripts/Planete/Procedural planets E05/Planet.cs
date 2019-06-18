using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class Planet : MonoBehaviour
{

    [Range(2, 255)]
    public int resolution = 10;
    private int resolutionMeshPerFace = 5; //ça marche bien avec ça, si on monte plus, ça marche plus ¯\_༼ ಥ ‿ ಥ ༽_/¯
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

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    public delegate void PlanetGenerationEnded();
    public event PlanetGenerationEnded OnPlanetGenerationEnded;
    public MinMax GetMinMax { get { return shapeGenerator.elevationMinMax; } }

    public void GameStart()
    {
        faceRenderMask = FaceRenderMask.All;
        GeneratePlanet();
    }

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        //foreach (MeshFilter m in meshFilters)
        //    DestroyImmediate(m.gameObject);

        //meshFilters = null;
        //terrainFaces = null;

        if (meshFilters == null || meshFilters.Length == 0 || meshFilters.Length != resolutionMeshPerFace * resolutionMeshPerFace * 6)
        {
            meshFilters = new MeshFilter[resolutionMeshPerFace * resolutionMeshPerFace * 6];
        }
        terrainFaces = new TerrainFace[resolutionMeshPerFace * resolutionMeshPerFace * 6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        int nbMeshPerFace = resolutionMeshPerFace * resolutionMeshPerFace;

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < nbMeshPerFace; j++)
            {
                if (meshFilters[j + (i * nbMeshPerFace)] == null)
                {
                    GameObject meshObj = new GameObject("mesh" + i + "|" + j);
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>();
                    meshFilters[j + (i * nbMeshPerFace)] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[j + (i * nbMeshPerFace)].sharedMesh = new Mesh();
                }
                meshFilters[j + (i * nbMeshPerFace)].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

                terrainFaces[j + (i * nbMeshPerFace)] = new TerrainFace(shapeGenerator, meshFilters[j + (i * nbMeshPerFace)].sharedMesh, resolution, directions[i], j, resolutionMeshPerFace);
                bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
                meshFilters[j + (i * nbMeshPerFace)].gameObject.SetActive(renderFace);
            }
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
        GenerateMeshColliders();
        OnPlanetGenerationEnded();
    }

    private void GenerateMeshColliders()
    {
        for (int i = 0; i < 6; i++)
        {
            MeshCollider col = meshFilters[i].GetComponent<MeshCollider>();
            col.sharedMesh = null;
            col.sharedMesh = meshFilters[i].sharedMesh;
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
        for (int i = 0; i < 150; i++)
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
