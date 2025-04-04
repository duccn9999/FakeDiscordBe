﻿using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.RolePermissions;
using DataAccesses.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RolePermissionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // POST api/<RolePermissionsController>
        [HttpPost]
        public async Task<IActionResult> Post(RolePermissionDTO model)
        {
            _unitOfWork.BeginTransaction();
            var rolePermission = _mapper.Map<RolePermission>(model);
            _unitOfWork.RolePermissions.ToggleRolePermission(rolePermission);
            _unitOfWork.Commit();
            return NoContent();
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> Get(int roleId)
        {
            var result = _unitOfWork.RolePermissions.GetRolePermissionsByRoleId(roleId);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetPermissionName([FromQuery] string roleIds)
        {
            // Convert the comma-separated string to a list of integers
            var permissionIdList = roleIds.Split(',').Select(int.Parse).ToList();
            var result = _unitOfWork.RolePermissions.GetPermissionNameByRoleIds(permissionIdList);
            return Ok(result);
        }
    }
}
