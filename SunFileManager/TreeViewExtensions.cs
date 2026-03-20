using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SunFileManager
{
    public static class TreeViewExtensions
    {
        public static List<string> GetExpansionState(this ObservableCollection<SunNode> nodes)
        {
            return nodes.Descendants()
                        .Where(n => n.IsExpanded)
                        .Select(n => GetFullPath(n))
                        .ToList();
        }

        public static void SetExpansionState(this ObservableCollection<SunNode> nodes, List<string> savedExpansionState)
        {
            foreach (var node in nodes.Descendants()
                                      .Where(n => savedExpansionState.Contains(GetFullPath(n))))
            {
                node.IsExpanded = true;
            }
        }

        private static string GetFullPath(SunNode node)
        {
            var parts = new Stack<string>();
            var current = node;
            while (current != null)
            {
                parts.Push(current.Name);
                current = current.Parent;
            }
            return string.Join("\\", parts);
        }

        public static IEnumerable<SunNode> Descendants(this ObservableCollection<SunNode> nodes)
        {
            foreach (var node in nodes)
            {
                yield return node;
                foreach (var child in node.Children.Descendants())
                    yield return child;
            }
        }
    }
}
