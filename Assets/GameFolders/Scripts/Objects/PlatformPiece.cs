using System.Collections.Generic;
using FluffyUnderware.Curvy;
using UnityEngine;

namespace GameFolders.Scripts.Objects
{
    public class PlatformPiece : MonoBehaviour
    {
        [SerializeField] private Transform pinIn;
        [SerializeField] private Transform pinOut;
        [SerializeField] private CurvySpline curvySpline;
        [SerializeField] private List<CurvySplineSegment> curvySplineSegments;

        public Transform PinIn => pinIn;
        public Transform PinOut => pinOut;
        public CurvySpline CurvySpline { get; }
        public List<CurvySplineSegment> CurvySplineSegments => curvySplineSegments;
    }
}