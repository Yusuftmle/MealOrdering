using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealOrdering.Shared.ResponseModel
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            Success = true;
        }
        public bool Success { get; set; }   
        public string? Message {get;set;}

        public void SetExcption(Exception exception)
        {
            Success=false;
            Message=exception.Message;
    }
    }

   
}
