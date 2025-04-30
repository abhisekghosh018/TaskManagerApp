using DevTaskTracker.Application.DTOs;
using DevTaskTracker.Application.DTOs.AuthDtos;
using DevTaskTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.Interfaces
{
    public interface IAuth
    {
       Task<CommonReturnDto>Register(RegisterDto dto);

       Task<CommonReturnDto> Login(LoginDto dto);
    }
}
