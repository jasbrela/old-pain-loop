using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    [System.Serializable]
    public class MapWaypoint
    {
        public Transform transform;
        public int weight;
    }

    public class MapWaypoints : MonoBehaviour
    {
        [SerializeField] List<MapWaypoint> mapWaypoints = new List<MapWaypoint>();

        private List<Vector3> _waypoints = null;
        public List<Vector3> waypoints
        {
            get
            {
                if (_waypoints == null)
                    InitializeWaypointsList();

                return _waypoints;
            }
        }

        /// <summary>
        /// Initializes the waypoints list while also getting the weight sum.
        /// </summary>
        void InitializeWaypointsList()
        {
            List<Vector3> waypointsList = new List<Vector3>(mapWaypoints.Count);
            foreach (MapWaypoint mapWaypoint in mapWaypoints)
            {
                waypointsList.Add(mapWaypoint.transform.position);
            }
            _waypoints = waypointsList;
        }

        public Vector3 GetRandomWaypoint(Vector3 waypointToExclude)
        {
            List<Vector3> listHolder = new List<Vector3>();
            listHolder.Add(waypointToExclude);
            return GetRandomWaypoint(listHolder);
        }

        public Vector3 GetRandomWaypoint(List<Vector3> waypointsToExclude = null)
        {
            int weightSum = 0;

            List<MapWaypoint> waypointCandidates = new List<MapWaypoint>(mapWaypoints);
            if (waypointsToExclude != null && waypointsToExclude.Count > 0)
            {
                foreach (MapWaypoint mapWaypoint in mapWaypoints)
                {
                    if (waypointsToExclude.Contains(mapWaypoint.transform.position))
                    {
                        waypointCandidates.Remove(mapWaypoint);
                    }
                    else
                    {
                        weightSum += mapWaypoint.weight;
                    }
                }
            }

            // Get a random value.
            int rnd = Random.Range(0, weightSum);

            // Foreach waypoint, if its weight is less than our current value,
            // then return him. Else subtract weight from our current value,
            // and go to next iteration.
            foreach (MapWaypoint candidate in waypointCandidates)
            {
                if (rnd < candidate.weight)
                {
                    return candidate.transform.position;
                }
                rnd -= candidate.weight;
            }

            // Returns Vector3.zero as fallback.
            return Vector3.zero;
        }

        void Awake()
        {
            InitializeWaypointsList();
        }
    }
}
