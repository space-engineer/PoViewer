using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using PoViewer.Interfaces;

namespace PoViewer.Classes.Parser.Po
{
    /// <summary>
    /// Производный от Parser класс для парсинга .po файлов.
    /// </summary>
    public class PoParser : Parser, IPoFile
    {
        public delegate void ParseCompletedHandler(bool parseCancel, PoData _poData);
        public event ParseCompletedHandler ParseCompletedEvent;

        private PoData _poData;
        private bool _stopParse;

        public PoParser(string filePath)
        {
            _filePath = filePath;
            _poData = new PoData();
        }

        public void Parse()
        {
            try
            {
                var tmpDataContainer = new PoDataContainer();
                var tmpContainer = new List<string>();

                ParallelLoopResult result =
                    Parallel.ForEach(File.ReadLines(_filePath), new ParallelOptions { MaxDegreeOfParallelism = 1 }, 
                    (line, pLoopState, inc) =>
                    {
                        if (pLoopState.ShouldExitCurrentIteration || _stopParse)
                        {
                            pLoopState.Break();
                        }

                        if (line.Length > 0)
                        {
                            tmpContainer.Add(line);
                        }
                            

                        if (tmpContainer.Count == 3)
                        {
                            tmpDataContainer.id = _poData.data.Count();
                            tmpDataContainer.msgctxt = tmpContainer[0];
                            tmpDataContainer.msgid = tmpContainer[1];
                            tmpDataContainer.msgstr = tmpContainer[2];

                            _poData.data.Add(_poData.data.Count(), tmpDataContainer);

                            tmpContainer = new List<string>();
                            tmpDataContainer = new PoDataContainer();
                        }
                    });

                if (result.IsCompleted && _poData != null)
                {
                    ParseCompletedEvent?.Invoke(false, _poData);
                }
                else
                {
                    ParseCompletedEvent?.Invoke(true, _poData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}