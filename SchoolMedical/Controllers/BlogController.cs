using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SchoolMedical.Core.Entities;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Infrastructure.Data;

namespace SchoolMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Blog (Lấy tất cả các bài blog đã xuất bản)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetPublishedBlogs()
        {
            var blogs = await (from blog in _context.Blogs
                               where blog.IsPublished
                               join account in _context.Accounts on blog.AuthorID equals account.UserID
                               let nurse = _context.Nurses.FirstOrDefault(n => n.UserID == account.UserID)
                               let manager = _context.ManagerAdmins.FirstOrDefault(m => m.UserID == account.UserID)
                               select new BlogDto
                               {
                                   BlogID = blog.BlogID,
                                   Title = blog.Title,
                                   Content = blog.Content,
                                   ImageUrl = blog.ImageUrl,
                                   AuthorID = blog.AuthorID,
                                   AuthorUsername = account.Username,
                                   AuthorFullName = (account.Role == "Nurse" && nurse != null) ? (nurse.FullName ?? "") :
                                                    ((account.Role == "Admin" && manager != null) ? (manager.FullName ?? "") : ""),
                                   CreatedDate = blog.CreatedDate,
                                   UpdatedDate = blog.UpdatedDate,
                                   IsPublished = blog.IsPublished
                               })
                               .ToListAsync();

            if (!blogs.Any())
            {
                return NotFound("Không tìm thấy bài blog nào đã xuất bản.");
            }

            return Ok(blogs);
        }

        // GET: api/Blog/{id} (Lấy một bài blog cụ thể theo ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDto>> GetBlog(int id)
        {
            var blog = await (from b in _context.Blogs
                               where b.BlogID == id && b.IsPublished
                               join a in _context.Accounts on b.AuthorID equals a.UserID
                               let nurse = _context.Nurses.FirstOrDefault(n => n.UserID == a.UserID)
                               let manager = _context.ManagerAdmins.FirstOrDefault(m => m.UserID == a.UserID)
                               select new BlogDto
                               {
                                   BlogID = b.BlogID,
                                   Title = b.Title,
                                   Content = b.Content,
                                   ImageUrl = b.ImageUrl,
                                   AuthorID = b.AuthorID,
                                   AuthorUsername = a.Username,
                                   AuthorFullName = (a.Role == "Nurse" && nurse != null) ? (nurse.FullName ?? "") :
                                                    ((a.Role == "Admin" && manager != null) ? (manager.FullName ?? "") : ""),
                                   CreatedDate = b.CreatedDate,
                                   UpdatedDate = b.UpdatedDate,
                                   IsPublished = b.IsPublished
                               })
                               .FirstOrDefaultAsync();

            if (blog == null)
            {
                return NotFound("Không tìm thấy bài blog đã xuất bản với ID này.");
            }

            return Ok(blog);
        }

        // GET: api/Blog/pending (Lấy tất cả các bài blog đang chờ duyệt - chỉ Admin)
        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetPendingBlogs()
        {
            var blogs = await (from b in _context.Blogs
                               where !b.IsPublished
                               join a in _context.Accounts on b.AuthorID equals a.UserID
                               let nurse = _context.Nurses.FirstOrDefault(n => n.UserID == a.UserID)
                               let manager = _context.ManagerAdmins.FirstOrDefault(m => m.UserID == a.UserID)
                               select new BlogDto
                               {
                                   BlogID = b.BlogID,
                                   Title = b.Title,
                                   Content = b.Content,
                                   ImageUrl = b.ImageUrl,
                                   AuthorID = b.AuthorID,
                                   AuthorUsername = a.Username,
                                   AuthorFullName = (a.Role == "Nurse" && nurse != null) ? (nurse.FullName ?? "") :
                                                    ((a.Role == "Admin" && manager != null) ? (manager.FullName ?? "") : ""),
                                   CreatedDate = b.CreatedDate,
                                   UpdatedDate = b.UpdatedDate,
                                   IsPublished = b.IsPublished
                               })
                               .ToListAsync();

            if (!blogs.Any())
            {
                return NotFound("Không có bài blog nào đang chờ duyệt.");
            }

            return Ok(blogs);
        }

        // POST: api/Blog (Tạo bài blog mới - Nurse/Admin)
        [HttpPost]
        [Authorize(Roles = "Nurse,Admin")]
        public async Task<ActionResult<Blog>> PostBlog(BlogCreateModel blogCreateModel)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null) return Unauthorized("Không xác định được người dùng.");
            var authorId = int.Parse(currentUserId);

            var blog = new Blog
            {
                Title = blogCreateModel.Title,
                Content = blogCreateModel.Content,
                ImageUrl = blogCreateModel.ImageUrl,
                AuthorID = authorId,
                CreatedDate = DateTime.Now,
                IsPublished = false
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlog), new { id = blog.BlogID }, blog);
        }

        // PUT: api/Blog/{id} (Cập nhật bài blog - Nurse (tác giả) hoặc Admin)
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> PutBlog(int id, BlogUpdateModel blogUpdateModel)
        {
            if (id != blogUpdateModel.BlogID)
            {
                return BadRequest("ID bài blog không khớp.");
            }

            var blogToUpdate = await _context.Blogs.FindAsync(id);
            if (blogToUpdate == null)
            {
                return NotFound("Không tìm thấy bài blog.");
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserId == null || (!blogToUpdate.AuthorID.ToString().Equals(currentUserId) && currentUserRole != "Admin"))
            {
                return Forbid("Bạn không có quyền chỉnh sửa bài blog này.");
            }

            blogToUpdate.Title = blogUpdateModel.Title;
            blogToUpdate.Content = blogUpdateModel.Content;
            blogToUpdate.ImageUrl = blogUpdateModel.ImageUrl;
            blogToUpdate.UpdatedDate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PATCH: api/Blog/publish/{id} (Thay đổi trạng thái xuất bản - chỉ Admin)
        [HttpPatch("publish/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PublishBlog(int id, [FromBody] bool isPublished)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound("Không tìm thấy bài blog.");
            }

            blog.IsPublished = isPublished;
            blog.UpdatedDate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Blog/{id} (Xóa bài blog - Nurse (tác giả) hoặc Admin)
        [HttpDelete("{id}")]
       
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound("Không tìm thấy bài blog.");
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserId == null || (!blog.AuthorID.ToString().Equals(currentUserId) && currentUserRole != "Admin"))
            {
                return Forbid("Bạn không có quyền xóa bài blog này.");
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.BlogID == id);
        }
    }
}