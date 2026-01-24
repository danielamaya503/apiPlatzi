using System;
using System.Collections.Generic;
using System.Text;

namespace CursoLinqCShar
{
    public class book
    {
        public string Title { get; set; }
        public int Pagecount { get; set; }
        public string Status { get; set; }
        public DateTime PublishedDate { get; set; }

        public string[] Authors { get; set; }

        public string[] Categories { get; set; }
    }
}
