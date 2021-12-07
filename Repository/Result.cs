using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Result
    {
        public bool IsSuccess { get; set; } = true;
        public object Data { get; set; }
        public string Message { get; set; } = "Success";
    }
}
