using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slicerMovemet : MonoBehaviour
{
    GameObject slicingPlane;
    //float desiredDepth = 0.1f;
    public float moveSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        slicingPlane = GameObject.FindGameObjectWithTag("SlicingPlane");
    }

    void Update()
    {   
        // Get the HoloLens' transformation matrix
            Matrix4x4 holoLensMatrix = Camera.main.transform.localToWorldMatrix;

        // Position of the HoloLens camera
        Vector3 cameraPosition = holoLensMatrix.GetColumn(3);
        Vector3 slicerPosition = slicingPlane.transform.position;

        // Calculate distances between objects
        float distanceLensToSlicer = Vector3.Distance(cameraPosition, slicerPosition);

        // Calculate the target positions
        Vector3 directionToSlicer = (cameraPosition - slicerPosition).normalized;
        Vector3 targetSlicerPosition = cameraPosition + directionToSlicer * distanceLensToSlicer;



        // Move the objects toward the target positions
        slicerPosition = Vector3.MoveTowards(slicerPosition, targetSlicerPosition, moveSpeed * Time.deltaTime);
        slicingPlane.transform.rotation = holoLensMatrix.rotation;
        slicingPlane.transform.localScale = Vector3.one;


        /*Simple implementaion to move slicer with hololens
         // Calculate the depth value based on the distance
         float newZPosition = slicingPlane.transform.position.z - desiredDepth;

         // Modify the Z value in the matrix
         holoLensMatrix[2, 3] = newZPosition;

         // Apply the transformation matrix to slicing plane
         zNewPosition = new Vector3(initialPosition.x, initialPosition.y, zPos);
         slicingPlane.transform.position.z = (a, b, holoLensMatrix[2, 3]);
            // = holoLensMatrix.GetColumn(3);
         slicingPlane.transform.rotation = holoLensMatrix.rotation;
         slicingPlane.transform.localScale = Vector3.one; // Adjust as needed*/
    }

}
