using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Core.DTOs.SchoolCheckup;

namespace SchoolMedical.Core.Interfaces.Services
{
    public interface ICheckupService
    {
        Task<List<SchoolCheckupDTO>> GetAllCheckupsAsync();
        Task<SchoolCheckupDTO> GetCheckupByIdAsync(int id);
        Task<List<SchoolCheckupDTO>> GetCheckupsByStudentIdAsync(int studentId);
        Task<SchoolCheckupDTO> CreateCheckupAsync(CreateSchoolCheckupRequest request);
        Task<bool> UpdateCheckupAsync(int id, CreateSchoolCheckupRequest request);
        Task<bool> DeleteCheckupAsync(int id);
    }
} 