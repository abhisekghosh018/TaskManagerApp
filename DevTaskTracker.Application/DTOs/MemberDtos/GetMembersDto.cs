﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.DTOs.MemberDtos
{
    public class GetMembersDto
    {        
        public string Id { get;set; }
        public string FirstName { get; set; }
        public string lastName { get; set; }
        public string WorkEmail { get; set; }
        public string Role { get; set; }
        public string? GitRepo { get; set; }               
        public string Organization { get; set; }
        public string OrganizationId { get; set; }
        public string Status { get; set; }
        public string RowVersion { get; set; }
        public bool IsActive { get; set; }
        public int  TotalCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ImageUrl { get; set; }    
    }
}
