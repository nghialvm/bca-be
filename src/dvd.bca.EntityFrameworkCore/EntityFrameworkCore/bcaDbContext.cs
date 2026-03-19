using dvd.bca.Configuration;
using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Entity.Recruitment;
using dvd.bca.Entity.Results;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
namespace dvd.bca.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName("Default")]
public class bcaDbContext :AbpDbContext<bcaDbContext>,IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */


    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext 
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext .
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    public DbSet<Department> Departments { get; set; }
    public DbSet<JobPosition> JobPositions { get; set; }
    public DbSet<RecruitmentRequest> RecruitmentRequests { get; set; }
    public DbSet<RecruitmentApproval> RecruitmentApprovals { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<CandidateDocument> CandidateDocuments { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<ApplicationScreening> ApplicationScreenings { get; set; }
    public DbSet<InterviewSchedule> InterviewSchedules { get; set; }
    public DbSet<InterviewEvaluation> InterviewEvaluations { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<CandidateResponse> CandidateResponses { get; set; }

    #endregion

    public bcaDbContext(DbContextOptions<bcaDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(bcaConsts.DbTablePrefix + "YourEntities", bcaConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.ApplyConfiguration(new DepartmentConfig());
        builder.ApplyConfiguration(new JobPositionConfig());
        builder.ApplyConfiguration(new RecruitmentRequestConfig());
        builder.ApplyConfiguration(new RecruitmentApprovalConfig());
        builder.ApplyConfiguration(new CandidateConfig());
        builder.ApplyConfiguration(new CandidateDocumentConfig());
        builder.ApplyConfiguration(new ApplicationConfig());
        builder.ApplyConfiguration(new ApplicationScreeningConfig());
        builder.ApplyConfiguration(new InterviewScheduleConfig());
        builder.ApplyConfiguration(new InterviewEvaluationConfig());
        builder.ApplyConfiguration(new OfferConfig());
        builder.ApplyConfiguration(new CandidateResponseConfig());
        builder.ApplyConfiguration(new EmployeeConfiguration());
    }
}
