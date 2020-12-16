using System;
using System.Windows.Forms;

using PoViewer.Classes.Helpers.IO;
using PoViewer.Classes.Parser.Po;
using PoViewer.Classes.UI.TreeView;

namespace PoViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItemOpenPoFile_Click(object sender, EventArgs e)
        {
            try
            {
                // Пользователь выбирает файл, с помощью файлового диалога OS.
                var ofd = new InputFile();
                var filePath = ofd.GetFilePath(".po");
                if (String.IsNullOrWhiteSpace(filePath)) return;

                // Парсим выбранный пользователем файл. После завершения слушаем событие - CompleteEvent.
                var poParser = new PoParser(filePath);
                poParser.ParseCompletedEvent += PoFileParseCompleted;
                poParser.Parse();         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        PoData _poData;
        private void PoFileParseCompleted(bool parseCancel, PoData poData)
        {
            try
            {
                if (parseCancel) return;

                _poData = poData;

                // Load TreeView
                var tvHelper = new TreeViewHelper();
                tvHelper.Load(poData, treeViewMain);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void treeViewMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int selectedNode = Convert.ToInt32(e.Node.Name);
            PoDataContainer cont = _poData.data[selectedNode];
            loadDataToPropGrid(cont);
        }

        void loadDataToPropGrid(PoDataContainer cont)
        {
            propertyGridData.SelectedObject = removeTrash(cont);
            propertyGridData.Refresh();
        }

        PoDataContainer removeTrash(PoDataContainer cont)
        {
            try
            {
                if (cont.msgctxt.Contains("msgctxt \"")) 
                {
                    cont.msgctxt = cont.msgctxt.Replace("msgctxt \"", "");
                    cont.msgctxt = cont.msgctxt.Remove(cont.msgctxt.Length - 1);
                }

                if (cont.msgid.Contains("msgid \""))
                {
                    cont.msgid = cont.msgid.Replace("msgid \"", "");
                    cont.msgid = cont.msgid.Remove(cont.msgid.Length - 1);
                }

                if (cont.msgstr.Contains("msgstr \""))
                {
                    cont.msgstr = cont.msgstr.Replace("msgstr \"", "");
                    cont.msgstr = cont.msgstr.Remove(cont.msgstr.Length - 1);
                }

                return cont;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

    }
}