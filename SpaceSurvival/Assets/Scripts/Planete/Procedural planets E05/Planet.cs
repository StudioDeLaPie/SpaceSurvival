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
    MeshFilter[] meshFilters_0;
    MeshFilter[] meshFilters_1;
    MeshFilter[] meshFilters_2;
    TerrainFace[] terrainFaces_0;
    TerrainFace[] terrainFaces_1;
    TerrainFace[] terrainFaces_2;

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

        int nbMeshesTotal = resolutionMeshPerFace * resolutionMeshPerFace * 6;

        meshCenterTransforms = new Transform[nbMeshesTotal];

        meshFilters_0 = new MeshFilter[nbMeshesTotal];
        meshFilters_1 = new MeshFilter[nbMeshesTotal];
        meshFilters_2 = new MeshFilter[nbMeshesTotal];
        terrainFaces_0 = new TerrainFace[nbMeshesTotal];
        terrainFaces_1 = new TerrainFace[nbMeshesTotal];
        terrainFaces_2 = new TerrainFace[nbMeshesTotal];

        GenerateTerrainFaces(meshFilters_0, terrainFaces_0, 0, 1, true);
        GenerateTerrainFaces(meshFilters_1, terrainFaces_1, 1, 3, true);
        GenerateTerrainFaces(meshFilters_2, terrainFaces_2, 2, 6, true);

    }

    public void GeneratePlanet(bool inEditor)
    {
        Initialize();
        GenerateMeshes(meshFilters_0, terrainFaces_0);
        GenerateMeshes(meshFilters_1, terrainFaces_1);
        GenerateMeshes(meshFilters_2, terrainFaces_2);
        GenerateColours();
        GenerateMeshColliders();
        OrganizeHierarchy(meshFilters_0);
        OrganizeHierarchy(meshFilters_1);
        OrganizeHierarchy(meshFilters_2);
        SetHideFlags();
        if (!inEditor)
            OnPlanetGenerationEnded();
    }


    private void GenerateTerrainFaces(MeshFilter[] meshFilters, TerrainFace[] terrainFaces, int LOD_level, int resolutionDivider, bool activateMeshes)
    {
        int nbMeshPerFace = resolutionMeshPerFace * resolutionMeshPerFace;

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < nbMeshPerFace; j++)
            {
                int index = j + (i * nbMeshPerFace);

                //Création des GameObjects
                if (meshFilters[index] == null)
                {
                    GameObject meshObj = new GameObject("mesh_LOD" + LOD_level);
                    meshObj.tag = "Ground";
                    meshObj.layer = LayerMask.NameToLayer("Ground");

                    meshObj.AddComponent<MeshCollider>();
                    meshObj.AddComponent<MeshRenderer>();
                    meshFilters[index] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[index].sharedMesh = new Mesh();
                }

                meshFilters[index].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

                //Création du Transform qui servira de parent
                if (meshCenterTransforms[index] == null)
                {
                    Transform meshCenterTransform = new GameObject(j + "_LOD_Group").transform;
                    meshCenterTransform.parent = parentFacesTransform[i];
                    meshCenterTransforms[index] = meshCenterTransform;
                }
                terrainFaces[index] = new TerrainFace(shapeGenerator, meshFilters[index].sharedMesh, resolution / resolutionDivider, directions[i], j, resolutionMeshPerFace, meshCenterTransforms[index]);

                bool renderFace = (faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i) && activateMeshes;
                meshFilters[index].gameObject.SetActive(renderFace);
            }
        }
    }

    private void GenerateMeshColliders()
    {
        for (int i = 0; i < meshFilters_0.Length; i++)
        {
            MeshCollider col = meshFilters_0[i].GetComponent<MeshCollider>();
            col.sharedMesh = null;
            col.sharedMesh = meshFilters_0[i].sharedMesh;
        }
    }

    private void OrganizeHierarchy(MeshFilter[] meshFilters)
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
        for (int i = 0; i < meshFilters_0.Length; i++)
        {
            if (save)
            {
                meshFilters_0[i].gameObject.hideFlags = HideFlags.None;
                meshFilters_1[i].gameObject.hideFlags = HideFlags.None;
                meshFilters_2[i].gameObject.hideFlags = HideFlags.None;
            }
            else
            {
                meshFilters_0[i].gameObject.hideFlags = HideFlags.DontSave;
                meshFilters_1[i].gameObject.hideFlags = HideFlags.DontSave;
                meshFilters_2[i].gameObject.hideFlags = HideFlags.DontSave;
            }
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
            GenerateMeshes(meshFilters_0, terrainFaces_0);
            GenerateMeshes(meshFilters_1, terrainFaces_1);
            GenerateMeshes(meshFilters_2, terrainFaces_2);
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

    void GenerateMeshes(MeshFilter[] meshFilters, TerrainFace[] terrainFaces)
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
