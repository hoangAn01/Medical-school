using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Core.DTOs.SchoolCheckup;
using SchoolMedical.Core.Entities;
using SchoolMedical.Core.Interfaces.Services;
using SchoolMedical.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolMedical.Services
{
    public class CheckupService : ICheckupService
    {
        private readonly ApplicationDbContext _context;

        public CheckupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SchoolCheckupDTO>> GetAllCheckupsAsync()
        {
            return await _context.SchoolCheckup
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Student)
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Nurse)
                .Select(sc => new SchoolCheckupDTO
                {
                    CheckupID = sc.CheckupID,
                    ReportID = sc.ReportID,
                    Weight = sc.Weight,
                    Height = sc.Height,
                    BloodPressure = sc.BloodPressure,
                    VisionLeft = sc.VisionLeft,
                    VisionRight = sc.VisionRight,
                    Date = sc.HealthReport.Date,
                    Description = sc.HealthReport.Description,
                    StudentID = sc.HealthReport.StudentID,
                    StudentName = sc.HealthReport.Student.FullName,
                    NurseID = sc.HealthReport.NurseID,
                    NurseName = sc.HealthReport.Nurse.FullName
                })
                .ToListAsync();
        }

        public async Task<SchoolCheckupDTO> GetCheckupByIdAsync(int id)
        {
            return await _context.SchoolCheckup
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Student)
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Nurse)
                .Where(sc => sc.CheckupID == id)
                .Select(sc => new SchoolCheckupDTO
                {
                    CheckupID = sc.CheckupID,
                    ReportID = sc.ReportID,
                    Weight = sc.Weight,
                    Height = sc.Height,
                    BloodPressure = sc.BloodPressure,
                    VisionLeft = sc.VisionLeft,
                    VisionRight = sc.VisionRight,
                    Date = sc.HealthReport.Date,
                    Description = sc.HealthReport.Description,
                    StudentID = sc.HealthReport.StudentID,
                    StudentName = sc.HealthReport.Student.FullName,
                    NurseID = sc.HealthReport.NurseID,
                    NurseName = sc.HealthReport.Nurse.FullName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<SchoolCheckupDTO>> GetCheckupsByStudentIdAsync(int studentId)
        {
            return await _context.SchoolCheckup
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Student)
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Nurse)
                .Where(sc => sc.HealthReport.StudentID == studentId)
                .Select(sc => new SchoolCheckupDTO
                {
                    CheckupID = sc.CheckupID,
                    ReportID = sc.ReportID,
                    Weight = sc.Weight,
                    Height = sc.Height,
                    BloodPressure = sc.BloodPressure,
                    VisionLeft = sc.VisionLeft,
                    VisionRight = sc.VisionRight,
                    Date = sc.HealthReport.Date,
                    Description = sc.HealthReport.Description,
                    StudentID = sc.HealthReport.StudentID,
                    StudentName = sc.HealthReport.Student.FullName,
                    NurseID = sc.HealthReport.NurseID,
                    NurseName = sc.HealthReport.Nurse.FullName
                })
                .ToListAsync();
        }

        public async Task<SchoolCheckupDTO> CreateCheckupAsync(CreateSchoolCheckupRequest request)
        {
            var healthReport = await _context.HealthReport.FindAsync(request.ReportID);
            if (healthReport == null)
            {
                throw new Exception("HealthReport not found");
            }

            var checkup = new SchoolCheckup
            {
                ReportID = request.ReportID,
                Weight = request.Weight,
                Height = request.Height,
                BloodPressure = request.BloodPressure,
                VisionLeft = request.VisionLeft,
                VisionRight = request.VisionRight
            };

            _context.SchoolCheckup.Add(checkup);
            await _context.SaveChangesAsync();

            return await GetCheckupByIdAsync(checkup.CheckupID);
        }

        public async Task<bool> UpdateCheckupAsync(int id, CreateSchoolCheckupRequest request)
        {
            var checkup = await _context.SchoolCheckup.FindAsync(id);
            if (checkup == null)
            {
                return false;
            }

            checkup.Weight = request.Weight;
            checkup.Height = request.Height;
            checkup.BloodPressure = request.BloodPressure;
            checkup.VisionLeft = request.VisionLeft;
            checkup.VisionRight = request.VisionRight;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCheckupAsync(int id)
        {
            var checkup = await _context.SchoolCheckup.FindAsync(id);
            if (checkup == null)
            {
                return false;
            }

            _context.SchoolCheckup.Remove(checkup);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 