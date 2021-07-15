using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Uptime.Services.Helpers
{
    public class Graph
    {
        private readonly List<string> pathPoints;
        private readonly List<RouteInfo> routeInfos;
        private readonly List<List<RouteInfo>> result;

        public Graph(List<string> vertices)
        {
            result = new List<List<RouteInfo>>();
            pathPoints = vertices;
            routeInfos = new List<RouteInfo>();
        }

        public void AddEdge(RouteInfo routeInfo)
        {
            routeInfos.Add(routeInfo);
        }

        public List<List<RouteInfo>> GetAllPaths(string from, string to)
        {
            var isVisited = new Dictionary<string, bool>();
            var pathList = new List<RouteInfo>();
            pathPoints.ForEach(x=>isVisited.Add(x,false));

            searchPathsDepthFirst(from, to, isVisited, pathList);
            return result;
        }

        // Search for all paths Depth First
        private void searchPathsDepthFirst(string from, string to,
            IDictionary<string, bool> isVisited,
            List<RouteInfo> localPathList)
        {
            if (from.Equals(to))
            {
                var temp = new List<RouteInfo>();
                localPathList.ForEach(x=>temp.Add(x));
                result.Add(temp);
                return;
            }

            isVisited[from] = true;

            var notVisited = routeInfos.Where(x=>x.From.Name==from).Where(x => !isVisited[x.To.Name]).ToList();
            foreach (var routeInfo in notVisited)
            {
                localPathList.Add(routeInfo);
                searchPathsDepthFirst(routeInfo.To.Name, to, isVisited, localPathList);
                localPathList.Remove(routeInfo);
            }

            isVisited[from] = false;
        }
    }
}
