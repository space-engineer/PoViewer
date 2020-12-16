using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PoViewer.Classes.Parser.Po
{
    public class PoData
    {
        public Dictionary<long, PoDataContainer> data;

        public PoData()
        {
            data = new Dictionary<long, PoDataContainer>();
        }
    }
    public class PoDataContainer
    {
        [ReadOnly(true)]
        public int id { get; set; }

        [ReadOnly(true)]
        [Browsable(true)]
        [Description("Description: Поле контекста.")]
        [Category("Property Name")]
        [DisplayName("msgctxt")]
        public string msgctxt { get; set; }

        [ReadOnly(true)]
        [Browsable(true)]
        [Description("Description: Оригинальная строка.")]
        [Category("Property Name")]
        [DisplayName("msgid")]
        public string msgid { get; set; }

        [Browsable(true)]
        [Description("Description: Измененная строка.")]
        [Category("Property Name")]
        [DisplayName("msgstr")]
        public string msgstr { get; set; }
    }
}