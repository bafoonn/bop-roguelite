using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace Pasta
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathResult> results = new Queue<PathResult>();
        //Queue<pathRequest> pathRequestQueue = new Queue<pathRequest>();
        //pathRequest currentPathRequest;

        static PathRequestManager instance;
        Pathfinding pathFinding;
        //bool isProcessingPath;

		private void Awake()
		{
            instance = this;
            pathFinding = GetComponent<Pathfinding> ();
		}

		private void Update()
		{
			if(results.Count > 0)
			{
                int itemsInQue = results.Count;
				lock (results)
				{
                    for(int i = 0; i < itemsInQue; i++)
					{
                        PathResult result = results.Dequeue();
                        result.callback(result.path, result.success);
					}
				}
			}
		}


		public static void RequestPath(/*Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback*/pathRequest request)
		{
            //pathRequest newRequest = new pathRequest(pathStart, pathEnd, callback);
            //instance.pathRequestQueue.Enqueue(newRequest);
            //instance.TryProcessNext();

            ThreadStart threadSStart = delegate
            {
                //instance.pathFinding.StartFindPath(request, Finished);
                instance.pathFinding.FindPath(request, instance.Finished);
            };
            threadSStart.Invoke();
		}

  //      void TryProcessNext()
		//{
  //          if(!isProcessingPath && pathRequestQueue.Count > 0)
		//	{
  //              currentPathRequest = pathRequestQueue.Dequeue();
  //              isProcessingPath = true;
  //              pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		//	}
		//}

        public void Finished(/*Vector3[]path, bool success, pathRequest orginalRequest*/ PathResult result)
		{
            //currentPathRequest.callback(path, success);
            //isProcessingPath = false;
            //TryProcessNext();

            //PathResult result = new PathResult(path, success, orginalRequest.callback);
			lock (results)
			{
                results.Enqueue(result);
            }
            
		}

        
        
    }

    public struct PathResult
    {
        public Vector3[] path;
        public bool success;
        public Action<Vector3[], bool> callback;


        public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
        {
            this.path = path;
            this.success = success;
            this.callback = callback;
        }
    }

    public struct pathRequest
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
