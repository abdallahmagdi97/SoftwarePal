﻿using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ILicenseRepository _licenseRepository;

        public LicenseService(ILicenseRepository licenseRepository)
        {
            _licenseRepository = licenseRepository;
        }

        public async Task<License> Add(License license)
        {
            return await _licenseRepository.Add(license);
        }

        public void Delete(License license)
        {
            _licenseRepository.Delete(license);
        }

        public Task<IEnumerable<License>> GetAll()
        {
            return _licenseRepository.GetAll();
        }

        public Task<License> GetById(int id)
        {
            return _licenseRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _licenseRepository.SaveChanges();
        }

        public async Task<License> Update(License license)
        {
            return await _licenseRepository.Update(license);
        }
    }

    public interface ILicenseService
    {
        Task<License> Add(License license);
        Task<IEnumerable<License>> GetAll();
        Task<License> GetById(int id);
        Task<License> Update(License license);
        void Delete(License license);
        Task SaveChanges();
    }
}