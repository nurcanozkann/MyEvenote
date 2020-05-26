using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer.Results
{
    public class BusinessLayerResult<T> where T: class
    {
        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObj> ();
        }

        public void AddError(ErrorMessageCode code, string message)
        {
            Errors.Add(new ErrorMessageObj() { code = code, message = message });
        }

        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }
    }

}
