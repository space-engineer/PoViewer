using System.Threading.Tasks;
using System.Windows.Forms;

using PoViewer.Classes.Parser.Po;

namespace PoViewer.Classes.UI.TreeView
{
    public class TreeViewHelper
    {
        public void Load(PoData poData, System.Windows.Forms.TreeView tv)
        {
            ParallelLoopResult result =
            Parallel.ForEach(poData.data.Values, new ParallelOptions { MaxDegreeOfParallelism = 1 }, (pdContainer, pLoopState, inc) =>
            {
                var id = pdContainer.id;
                var nodeName = pdContainer.msgctxt;

                nodeName = nodeName.Replace("\"", "");

                if (nodeName.Contains("."))
                {
                    var n = nodeName.Split('.');
                    tv.Nodes.Add(id.ToString(), n[0]);
                    for (int i = 1; i < n.Length; i++)
                    {
                        TreeNode lastNode = getLastNode(tv.Nodes[tv.Nodes.Count - 1]);
                        lastNode.Nodes.Add(id.ToString(), n[i]);
                    }
                }
                else
                {
                    tv.Invoke((MethodInvoker)(() =>
                        tv.Nodes.Add(id.ToString(), nodeName)
                    ));
                }
            });

            tv.ExpandAll();
            tv.SelectedNode = null;
        }
        private TreeNode getLastNode(TreeNode subroot)
        {
            if (subroot.Nodes.Count == 0)
                return subroot;

            return getLastNode(subroot.Nodes[subroot.Nodes.Count - 1]);
        }
    }
}