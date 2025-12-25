using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Encadri_Backend.Data;

namespace Encadri_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get dashboard statistics for a user (student or supervisor)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<object>> GetDashboardStats(
            [FromQuery] string userEmail,
            [FromQuery] string userRole)
        {
            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userRole))
            {
                return BadRequest(new { message = "userEmail and userRole are required" });
            }

            if (userRole.ToLower() == "student")
            {
                return await GetStudentStats(userEmail);
            }
            else if (userRole.ToLower() == "supervisor")
            {
                return await GetSupervisorStats(userEmail);
            }

            return BadRequest(new { message = "Invalid user role" });
        }

        private async Task<ActionResult<object>> GetStudentStats(string studentEmail)
        {
            // Get student's projects
            var projects = await _context.Projects
                .Where(p => p.StudentEmail == studentEmail)
                .ToListAsync();

            var projectIds = projects.Select(p => p.Id).ToList();

            // Get submissions
            var submissions = await _context.Submissions
                .Where(s => s.SubmittedBy == studentEmail)
                .ToListAsync();

            // Get milestones
            var milestones = await _context.Milestones
                .Where(m => projectIds.Contains(m.ProjectId))
                .ToListAsync();

            // Get meetings
            var meetings = await _context.Meetings
                .Where(m => projectIds.Contains(m.ProjectId))
                .ToListAsync();

            // Calculate statistics
            var stats = new
            {
                // Project stats
                totalProjects = projects.Count,
                activeProjects = projects.Count(p => p.Status == "in_progress"),
                completedProjects = projects.Count(p => p.Status == "completed"),

                // Submission stats
                totalSubmissions = submissions.Count,
                pendingSubmissions = submissions.Count(s => s.Status == "pending"),
                approvedSubmissions = submissions.Count(s => s.Status == "approved"),
                rejectedSubmissions = submissions.Count(s => s.Status == "rejected"),
                needsRevisionSubmissions = submissions.Count(s => s.Status == "needs_revision"),
                reviewedSubmissions = submissions.Count(s => s.Status == "reviewed"),
                averageGrade = submissions.Any(s => s.Grade.HasValue)
                    ? Math.Round(submissions.Where(s => s.Grade.HasValue).Average(s => s.Grade.Value), 2)
                    : 0,

                // Milestone stats
                totalMilestones = milestones.Count,
                completedMilestones = milestones.Count(m => m.Status == "completed"),
                inProgressMilestones = milestones.Count(m => m.Status == "in_progress"),
                pendingMilestones = milestones.Count(m => m.Status == "pending"),

                // Meeting stats
                totalMeetings = meetings.Count,
                upcomingMeetings = meetings.Count(m => m.ScheduledAt > DateTime.UtcNow && m.Status != "cancelled"),
                completedMeetings = meetings.Count(m => m.Status == "completed"),

                // Recent activity
                recentSubmissions = submissions
                    .OrderByDescending(s => s.CreatedDate)
                    .Take(5)
                    .Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.Status,
                        s.Grade,
                        s.CreatedDate
                    })
                    .ToList(),

                // Grade trend (last 10 graded submissions)
                gradeTrend = submissions
                    .Where(s => s.Grade.HasValue)
                    .OrderBy(s => s.CreatedDate)
                    .Take(10)
                    .Select(s => new
                    {
                        title = s.Title,
                        grade = s.Grade,
                        date = s.CreatedDate
                    })
                    .ToList()
            };

            return Ok(stats);
        }

        private async Task<ActionResult<object>> GetSupervisorStats(string supervisorEmail)
        {
            // Get supervisor's projects
            var projects = await _context.Projects
                .Where(p => p.SupervisorEmail == supervisorEmail)
                .ToListAsync();

            var projectIds = projects.Select(p => p.Id).ToList();

            // Get submissions for supervised projects
            var submissions = await _context.Submissions
                .Where(s => projectIds.Contains(s.ProjectId))
                .ToListAsync();

            // Get milestones
            var milestones = await _context.Milestones
                .Where(m => projectIds.Contains(m.ProjectId))
                .ToListAsync();

            // Get meetings
            var meetings = await _context.Meetings
                .Where(m => projectIds.Contains(m.ProjectId))
                .ToListAsync();

            // Get students count
            var uniqueStudents = projects
                .Where(p => !string.IsNullOrEmpty(p.StudentEmail))
                .Select(p => p.StudentEmail)
                .Distinct()
                .Count();

            // Calculate statistics
            var stats = new
            {
                // Project stats
                totalProjects = projects.Count,
                activeProjects = projects.Count(p => p.Status == "in_progress"),
                completedProjects = projects.Count(p => p.Status == "completed"),
                proposedProjects = projects.Count(p => p.Status == "proposed"),

                // Student stats
                totalStudents = uniqueStudents,

                // Submission stats
                totalSubmissions = submissions.Count,
                pendingSubmissions = submissions.Count(s => s.Status == "pending"),
                approvedSubmissions = submissions.Count(s => s.Status == "approved"),
                rejectedSubmissions = submissions.Count(s => s.Status == "rejected"),
                needsReviewSubmissions = submissions.Count(s => s.Status == "pending" || s.Status == "needs_revision"),

                // Milestone stats
                totalMilestones = milestones.Count,
                completedMilestones = milestones.Count(m => m.Status == "completed"),

                // Meeting stats
                totalMeetings = meetings.Count,
                upcomingMeetings = meetings.Count(m => m.ScheduledAt > DateTime.UtcNow && m.Status != "cancelled"),

                // Recent submissions needing review
                recentPendingSubmissions = submissions
                    .Where(s => s.Status == "pending" || s.Status == "needs_revision")
                    .OrderByDescending(s => s.CreatedDate)
                    .Take(5)
                    .Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.Status,
                        s.SubmittedBy,
                        s.CreatedDate
                    })
                    .ToList(),

                // Project progress overview
                projectProgress = projects
                    .Select(p => new
                    {
                        p.Id,
                        p.Title,
                        p.ProgressPercentage,
                        p.Status,
                        studentEmail = p.StudentEmail
                    })
                    .ToList()
            };

            return Ok(stats);
        }
    }
}
