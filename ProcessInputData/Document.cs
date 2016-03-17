using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessInputData
{
    class Document
    {
        public string  ClassLabel;
        public List<String> Words;
        public Document (string cl)
        {
            ClassLabel = cl;
            Words = new List<string>();
        }
    }
}
