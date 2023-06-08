using System.Collections.Generic;
using System.Linq;
using FluffyUnderware.Curvy;
using UnityEngine;

namespace GameFolders.Scripts.Objects
{
    public class LevelItem : MonoBehaviour
    {
        private Transform platformPiecesParrent;

        public Material SkyboxMaterial;
        public CurvySpline CurvySpline;

        private List<PlatformPiece> listOfPlatformPieces;


        public void GenerateTheLevel(bool isRuntime = true)
        {
            platformPiecesParrent = transform.GetChild(0);
            if (CurvySpline != null)
            {
                if (isRuntime)
                    Destroy(CurvySpline.gameObject);
                else
                    DestroyImmediate(CurvySpline.gameObject);
            }

            listOfPlatformPieces =
                (from Transform child in platformPiecesParrent select child.GetComponent<PlatformPiece>()).ToList();

            List<CurvySplineSegment> css = listOfPlatformPieces
                .SelectMany(platformPiece => platformPiece.CurvySplineSegments)
                .ToList();

            for (int i = 1; i < listOfPlatformPieces.Count; i++)
            {
                listOfPlatformPieces[i].transform.position = listOfPlatformPieces[i - 1].PinOut.position;
                listOfPlatformPieces[i].transform.rotation = listOfPlatformPieces[i - 1].PinOut.rotation;
            }

            // if (isRuntime)
            // {
            GenerateTheCurvySpline(css);

            foreach (var piece in listOfPlatformPieces)
            {
                if (piece.CurvySpline != null)
                    Destroy(piece.CurvySpline.gameObject);
            }
            // }
        }

        private void GenerateTheCurvySpline(List<CurvySplineSegment> segments)
        {
            CurvySpline = CurvySpline.Create();
            CurvySpline.transform.parent = transform;
            CurvySpline.transform.position = Vector3.zero;
            CurvySpline.Interpolation = CurvyInterpolation.Bezier;
            CurvySpline.name = "CurrentLevelCurvySpline";
            CurvySpline.Add(Vector3.zero);

            foreach (var segment in segments)
            {
                var css = CurvySpline.ControlPointsList[CurvySpline.ControlPointsList.Count - 1];
                var createdSplineSegment = CurvySpline.InsertAfter(css,
                    segment.transform.position, true);

                createdSplineSegment.AutoHandles = segment.AutoHandles;
                createdSplineSegment.HandleInPosition = segment.HandleInPosition;
                createdSplineSegment.HandleOutPosition = segment.HandleOutPosition;
                createdSplineSegment.AutoBakeOrientation = segment.AutoBakeOrientation;
            }

            CurvySpline.Refresh();
        }
    }
}