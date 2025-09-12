using DocTask.Core.Models;
using Microsoft.EntityFrameworkCore;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Frequency> Frequencies { get; set; }

    public virtual DbSet<FrequencyDetail> FrequencyDetails { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Org> Orgs { get; set; }

    public virtual DbSet<Period> Periods { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Progress> Progresses { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<Reminderunit> Reminderunits { get; set; }

    public virtual DbSet<ReportReview> ReportReviews { get; set; }

    public virtual DbSet<Reportsummary> Reportsummaries { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Taskunitassignment> Taskunitassignments { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<Unituser> Unitusers { get; set; }

    public virtual DbSet<Uploadfile> Uploadfiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer("Server=123.31.20.167,1435;Database=DoctaskAI;User ID=sa;Password=Pisa123;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Frequency>(entity =>
        {
            entity.ToTable("frequency");

            entity.Property(e => e.FrequencyId).HasColumnName("frequencyId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.FrequencyDetail)
                .HasMaxLength(100)
                .HasColumnName("frequencyDetail");
            entity.Property(e => e.FrequencyType)
                .HasMaxLength(20)
                .HasColumnName("frequencyType");
            entity.Property(e => e.IntervalValue).HasColumnName("intervalValue");
        });

        modelBuilder.Entity<FrequencyDetail>(entity =>
        {
            entity.ToTable("frequency_detail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DayOfMonth).HasColumnName("dayOfMonth");
            entity.Property(e => e.DayOfWeek).HasColumnName("dayOfWeek");
            entity.Property(e => e.FrequencyId).HasColumnName("frequencyId");

            entity.HasOne(d => d.Frequency).WithMany(p => p.FrequencyDetails)
                .HasForeignKey(d => d.FrequencyId)
                .HasConstraintName("fk_frequency_detail_frequency");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notification");

            entity.Property(e => e.NotificationId).HasColumnName("notificationId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("isRead");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.TaskId).HasColumnName("taskId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Task).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkNotificationTask");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkNotificationUser");
        });

        modelBuilder.Entity<Org>(entity =>
        {
            entity.ToTable("org");

            entity.Property(e => e.OrgId).HasColumnName("orgId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.OrgName)
                .HasMaxLength(100)
                .HasColumnName("orgName");
            entity.Property(e => e.ParentOrgId).HasColumnName("parentOrgId");
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.ToTable("period");

            entity.Property(e => e.PeriodId).HasColumnName("periodId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.PeriodName)
                .HasMaxLength(50)
                .HasColumnName("periodName");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("position");

            entity.Property(e => e.PositionId).HasColumnName("positionId");
            entity.Property(e => e.PositionName)
                .HasMaxLength(255)
                .HasColumnName("positionName");
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.ToTable("progress");

            entity.Property(e => e.ProgressId).HasColumnName("progressId");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.Feedback).HasColumnName("feedback");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("fileName");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .HasColumnName("filePath");
            entity.Property(e => e.PercentageComplete)
                .HasDefaultValue(0)
                .HasColumnName("percentageComplete");
            entity.Property(e => e.PeriodId).HasColumnName("periodId");
            entity.Property(e => e.Proposal).HasColumnName("proposal");
            entity.Property(e => e.Result).HasColumnName("result");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TaskId).HasColumnName("taskId");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");

            entity.HasOne(d => d.Period).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.PeriodId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkProgressPeriod");

            entity.HasOne(d => d.Task).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("fkProgressTask");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkProgressUpdatedBy");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.ToTable("reminder");

            entity.Property(e => e.Reminderid).HasColumnName("reminderid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Isauto)
                .HasDefaultValue(false)
                .HasColumnName("isauto");
            entity.Property(e => e.Isnotified)
                .HasDefaultValue(false)
                .HasColumnName("isnotified");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Notificationid).HasColumnName("notificationid");
            entity.Property(e => e.Notifiedat)
                .HasColumnType("datetime")
                .HasColumnName("notifiedat");
            entity.Property(e => e.Periodid).HasColumnName("periodid");
            entity.Property(e => e.Taskid).HasColumnName("taskid");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Triggertime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("triggertime");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.ReminderCreatedbyNavigations)
                .HasForeignKey(d => d.Createdby)
                .HasConstraintName("reminder_ibfk_3");

            entity.HasOne(d => d.Notification).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.Notificationid)
                .HasConstraintName("reminder_ibfk_4");

            entity.HasOne(d => d.Period).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.Periodid)
                .HasConstraintName("reminder_ibfk_2");

            entity.HasOne(d => d.Task).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.Taskid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reminder_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.ReminderUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_reminder_User_UserId");
        });

        modelBuilder.Entity<Reminderunit>(entity =>
        {
            entity.ToTable("reminderunit");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Reminderid).HasColumnName("reminderid");
            entity.Property(e => e.Unitid).HasColumnName("unitid");

            entity.HasOne(d => d.Reminder).WithMany(p => p.Reminderunits)
                .HasForeignKey(d => d.Reminderid)
                .HasConstraintName("fk_reminder");

            entity.HasOne(d => d.Unit).WithMany(p => p.Reminderunits)
                .HasForeignKey(d => d.Unitid)
                .HasConstraintName("fk_unit");
        });

        modelBuilder.Entity<ReportReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__report_r__2ECD6E04C573B4B5");

            entity.ToTable("report_review");

            entity.Property(e => e.ReviewId).HasColumnName("reviewId");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.ProgressId).HasColumnName("progressId");
            entity.Property(e => e.ReviewedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("reviewedAt");
            entity.Property(e => e.ReviewerId).HasColumnName("reviewerId");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.Progress).WithMany(p => p.ReportReviews)
                .HasForeignKey(d => d.ProgressId)
                .HasConstraintName("FK_ReportReview_Progress");

            entity.HasOne(d => d.Reviewer).WithMany(p => p.ReportReviews)
                .HasForeignKey(d => d.ReviewerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ReportReview_User");
        });

        modelBuilder.Entity<Reportsummary>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.ToTable("reportsummary");

            entity.Property(e => e.ReportId).HasColumnName("reportId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.PeriodId).HasColumnName("periodId");
            entity.Property(e => e.ReportFile).HasColumnName("reportFile");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.TaskId).HasColumnName("taskId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Reportsummaries)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkReportCreatedBy");

            entity.HasOne(d => d.Period).WithMany(p => p.Reportsummaries)
                .HasForeignKey(d => d.PeriodId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkReportPeriod");

            entity.HasOne(d => d.ReportFileNavigation).WithMany(p => p.Reportsummaries)
                .HasForeignKey(d => d.ReportFile)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkReportFile");

            entity.HasOne(d => d.Task).WithMany(p => p.Reportsummaries)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkReportTask");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("task");

            entity.Property(e => e.TaskId).HasColumnName("taskId");
            entity.Property(e => e.AssigneeId).HasColumnName("assigneeId");
            entity.Property(e => e.AssignerId).HasColumnName("assignerId");
            entity.Property(e => e.AttachedFile).HasColumnName("attachedFile");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DueDate).HasColumnName("dueDate");
            entity.Property(e => e.FrequencyId).HasColumnName("frequencyId");
            entity.Property(e => e.OrgId).HasColumnName("orgId");
            entity.Property(e => e.ParentTaskId).HasColumnName("parentTaskId");
            entity.Property(e => e.Percentagecomplete)
                .HasDefaultValue(0)
                .HasColumnName("percentagecomplete");
            entity.Property(e => e.PeriodId).HasColumnName("periodId");
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasDefaultValue("medium")
                .HasColumnName("priority");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UnitId).HasColumnName("unitId");

            entity.HasOne(d => d.Assignee).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkTaskAssignee");

            entity.HasOne(d => d.Frequency).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.FrequencyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_taskitem_frequency");

            entity.HasMany(d => d.Users).WithMany(p => p.TasksNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "Taskassignee",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("taskassignees_ibfk_2"),
                    l => l.HasOne<Task>().WithMany()
                        .HasForeignKey("TaskId")
                        .HasConstraintName("taskassignees_ibfk_1"),
                    j =>
                    {
                        j.HasKey("TaskId", "UserId");
                        j.ToTable("taskassignees");
                    });
        });

        modelBuilder.Entity<Taskunitassignment>(entity =>
        {
            entity.ToTable("taskunitassignment");

            entity.HasOne(d => d.Task).WithMany(p => p.Taskunitassignments)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taskunitassignment_ibfk_1");

            entity.HasOne(d => d.Unit).WithMany(p => p.Taskunitassignments)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taskunitassignment_ibfk_2");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.ToTable("unit");

            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.OrgId).HasColumnName("orgId");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasDefaultValue("official")
                .HasColumnName("type");
            entity.Property(e => e.UnitName)
                .HasMaxLength(255)
                .HasColumnName("unitName");
            entity.Property(e => e.UnitParent).HasColumnName("unitParent");

            entity.HasOne(d => d.Org).WithMany(p => p.Units)
                .HasForeignKey(d => d.OrgId)
                .HasConstraintName("fkUnitOrg");
        });

        modelBuilder.Entity<Unituser>(entity =>
        {
            entity.ToTable("unituser");

            entity.Property(e => e.UnitUserId).HasColumnName("unitUserId");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Unit).WithMany(p => p.Unitusers)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("fkUnitUserUnit");

            entity.HasOne(d => d.User).WithMany(p => p.Unitusers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fkUnitUserUser");
        });

        modelBuilder.Entity<Uploadfile>(entity =>
        {
            entity.HasKey(e => e.FileId);

            entity.ToTable("uploadfile");

            entity.Property(e => e.FileId).HasColumnName("fileId");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("fileName");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .HasColumnName("filePath");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("uploadedAt");
            entity.Property(e => e.UploadedBy).HasColumnName("uploadedBy");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Uploadfiles)
                .HasForeignKey(d => d.UploadedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkUploadFileUploadedBy");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "UQ_user_email").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_user_username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.OrgId).HasColumnName("orgId");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.PositionId).HasColumnName("positionId");
            entity.Property(e => e.PositionName)
                .HasMaxLength(255)
                .HasColumnName("positionName");
            entity.Property(e => e.Refreshtoken)
                .HasMaxLength(255)
                .HasColumnName("refreshtoken");
            entity.Property(e => e.Refreshtokenexpirytime)
                .HasColumnType("datetime")
                .HasColumnName("refreshtokenexpirytime");
            entity.Property(e => e.Role)
                .HasMaxLength(11)
                .HasDefaultValue("0")
                .HasColumnName("role");
            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.UnitUserId).HasColumnName("unitUserId");
            entity.Property(e => e.UserParent).HasColumnName("userParent");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Org).WithMany(p => p.Users)
                .HasForeignKey(d => d.OrgId)
                .HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.Position).WithMany(p => p.Users)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("user_ibfk_1");

            entity.HasOne(d => d.Unit).WithMany(p => p.Users)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("user_ibfk_3");

            entity.HasOne(d => d.UnitUser).WithMany(p => p.Users)
                .HasForeignKey(d => d.UnitUserId)
                .HasConstraintName("fk_user_unitUser");

            entity.HasOne(d => d.UserParentNavigation).WithMany(p => p.InverseUserParentNavigation)
                .HasForeignKey(d => d.UserParent)
                .HasConstraintName("user_ibfk_4");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.ToTable("userrole");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Role).WithMany(p => p.Userroles)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("fk_userrole_role");

            entity.HasOne(d => d.User).WithMany(p => p.Userroles)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_userrole_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
