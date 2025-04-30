using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.DTOs
{
    public class CommonReturnDto
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string AlreadyExistMessage { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Token { get; set; }       
        public object Data { get; set; }

    }
}
