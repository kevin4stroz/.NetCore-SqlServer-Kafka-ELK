using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PermissionsN5.Core.DTO;
using PermissionsN5.Core.Interfaces;
using PermissionsN5.Core.Entities;
using System.Linq;



namespace PermissionsN5.Core.Services
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IUnitOfWork _unitOfWorkRepo;
        private readonly IUtils _utils;
        
        public PermissionsService(IUnitOfWork uow, IUtils utils)
        {
            _unitOfWorkRepo = uow;
            _utils = utils;
        }

        public async Task<List<PermissionsDTO>> GetAll()
        {
            List<Permissions> allPermissions = await _unitOfWorkRepo.PermissionRepository.GetAll();
            List<PermissionsDTO> AllPermissionsDTO = allPermissions.Select(
                x => new PermissionsDTO
                {
                    Id = x.Id,
                    EmployeeForename = x.EmployeeForename,
                    EmployeeSurname = x.EmployeeSurname,
                    PermissionDate = x.PermissionDate,
                    PermissionType = x.PermissionType
                }
            ).ToList();

            // enviar evento a kafka de get
            await _utils.SendToKafka(new KafkaDTO("GET"));

            return AllPermissionsDTO;
        }

        public async Task<PermissionsDTO> Add(ModPermissionDTO obj)
        {
            if (await _unitOfWorkRepo.PermissionTypeRepository.GetById(obj.PermissionType) == null)
                return null;

            Permissions permTemp = new Permissions
            {
                EmployeeForename = obj.EmployeeForename,
                EmployeeSurname = obj.EmployeeSurname,
                PermissionType = obj.PermissionType,
                PermissionDate = DateTime.Now
            };

            permTemp = await _unitOfWorkRepo.PermissionRepository.Add(permTemp);
            if (permTemp == null)
                return null;

            PermissionsDTO nPermi = new PermissionsDTO
            {
                Id = permTemp.Id,
                EmployeeForename = permTemp.EmployeeForename,
                EmployeeSurname = permTemp.EmployeeSurname,
                PermissionType = permTemp.PermissionType,
                PermissionDate = permTemp.PermissionDate
            };

            await _utils.SendToKafka(new KafkaDTO("REQUEST"));
            await _utils.SendToElastic(nPermi);            

            return nPermi;
        }

        public async Task<PermissionsDTO> Update(int id, ModPermissionDTO modPermission)
        {
            Permissions permission = await _unitOfWorkRepo.PermissionRepository.GetById(id);
            if (permission == null)
                return null;

            if (await _unitOfWorkRepo.PermissionTypeRepository.GetById(modPermission.PermissionType) == null)
                return null;

            permission.EmployeeForename = modPermission.EmployeeForename;
            permission.EmployeeSurname = modPermission.EmployeeSurname;
            permission.PermissionType = modPermission.PermissionType;
            permission.PermissionDate = DateTime.Now;


            permission = await _unitOfWorkRepo.PermissionRepository.Update(permission);
            if (permission == null)
                return null;

            PermissionsDTO nPermi = new PermissionsDTO
            {
                Id = permission.Id,
                EmployeeForename = permission.EmployeeForename,
                EmployeeSurname = permission.EmployeeSurname,
                PermissionDate = permission.PermissionDate,
                PermissionType = permission.PermissionType
            };

            await _utils.SendToKafka(new KafkaDTO("MODIFY"));
            await _utils.SendToElastic(nPermi);            
            
            return nPermi;

        }
    }
}
