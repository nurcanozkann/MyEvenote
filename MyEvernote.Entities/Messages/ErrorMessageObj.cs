using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.Messages
{
    public class ErrorMessageObj
    {
       public ErrorMessageCode code { get; set; }
       public string message { get; set; }
    }
}
