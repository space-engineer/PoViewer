using System.Windows.Forms;

namespace PoViewer.Classes.Viewer
{
    public abstract class Viewer
    {
        public virtual string OpenFile(string extension) 
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = $"File|*.{extension}";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }

            return string.Empty;
        }
    }
}