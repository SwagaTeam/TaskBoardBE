﻿using System.ComponentModel;
using SharedLibrary.ProjectModels;
namespace SharedLibrary.Models;

public class UserProjectModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? ProjectId { get; set; }
    public int Privilege { get; set; }
    
    [DefaultValue(null)]
    public int? RoleId { get; set; }

    public ProjectModel? Project { get; set; }
    public RoleModel? Role { get; set; }
}