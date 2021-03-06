﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{

    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int resolution;
    int faceResolution;
    int indexOnFace;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    public Transform meshCenter;


    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp, int indexOnFace, int faceResolution, Transform meshCenter)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this.indexOnFace = indexOnFace;
        this.faceResolution = faceResolution;
        this.meshCenter = meshCenter;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        //Vector3[] normals = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        int yOnFace = indexOnFace / faceResolution;
        int xOnFace = indexOnFace % faceResolution;
        Vector2 faceOffset = new Vector2(xOnFace, yOnFace) / faceResolution;

        //CENTER TRANSFORM
        //Placement au centre du mesh en cours de création
        meshCenter.position = (localUp + (faceOffset.x - (.5f - ((1f / faceResolution) / 2f))) * 2 * axisA + (faceOffset.y - (.5f - ((1f / faceResolution) / 2f))) * 2 * axisB);

        //Placement sur la sphere et décalage jusqu'à la surface de la planète
        meshCenter.position = meshCenter.position.normalized * shapeGenerator.GetRadius();
        meshCenter.up = meshCenter.position.normalized; //On assume que la planète se trouve en (0, 0, 0) (le calcul du up est -(centrePlanete - transform))

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percentOnMesh = new Vector2(x, y) / ((resolution - 1) * faceResolution);
                Vector3 pointOnUnitCube = (localUp + faceOffset.x * 2 * axisA + faceOffset.y * 2 * axisB) + (percentOnMesh.x - .5f) * 2 * axisA + (percentOnMesh.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                //vertices[i] = pointOnUnitCube;
                //vertices[i] = pointOnUnitSphere;
                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }

        //for (int i = 0; i < triangles.Length; i += 3)
        //{
        //    //Vector3 normal = Vector3.down;
        //    int index0 = triangles[i];
        //    int index1 = triangles[i + 1];
        //    int index2 = triangles[i + 2];

        //    Vector3 p1 = vertices[index1] - vertices[index0];
        //    Vector3 p2 = vertices[index2] - vertices[index0];

        //    Vector3 normal = Vector3.Cross(p1, p2).normalized;
        //    //Debug.DrawRay(vertices[index0], normal * 100, Color.blue, 10);
        //    //Debug.DrawRay(vertices[index1], normal * 1000, Color.green, 10);
        //    ////Debug.DrawRay(vertices[index2], normal * 10000, Color.yellow, 10);
        //    normals[index0] = normal;
        //    normals[index1] = normal;
        //    normals[index2] = normal;
        //}


        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.normals = normals;
        mesh.RecalculateNormals();
    }
}
