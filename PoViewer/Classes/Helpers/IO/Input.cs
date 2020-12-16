using System;
using System.Windows.Forms;

namespace PoViewer.Classes.Helpers.IO
{
    public abstract class Input
    {
        public virtual string GetFilePath(string fileExtension)
        {
            try
            {
                var ofd = new OpenFileDialog();
                ofd.Filter = $"File|*{fileExtension}";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return string.Empty;
            }
        }
    }
}