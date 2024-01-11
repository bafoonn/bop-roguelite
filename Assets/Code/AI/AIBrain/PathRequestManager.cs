using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<pathRequest> pathRequestQueue = new Queue<pathRequest>();
        pathRequest currentPathRequest;

        static PathRequestManager instance;
        Pathfinding pathFinding;
        bool isProcessingPath;

		private void Awake()
		{
            instance = this;
            pathFinding = GetComponent<Pathfinding> ();
		}


		public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
		{
            pathRequest newRequest = new pathRequest(pathStart, pathEnd, callback);
            instance.pathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();
		}

        void TryProcessNext()
		{
            if(!isProcessingPath && pathRequestQueue.Count > 0)
			{
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
			}
		}

        public void Finished(Vector3[]path, bool success)
		{
            currentPathRequest.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
		}
        struct pathRequest
		{
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public pathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> m_callback)
			{
                pathStart = start;
                pathEnd = end;
                callback = m_callback;
			}
		}
    }
}
