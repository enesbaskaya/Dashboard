using Microsoft.EntityFrameworkCore;

namespace Dashboard.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
    : base(options)
        { }

        public DbSet<Admin> admin { get; set; }
        public DbSet<TeamPlayers> teamPlayers { get; set; }
        public DbSet<NotificationTypes> notificationTypes { get; set; }
        public DbSet<AdminNotification> adminNotification { get; set; }
        public DbSet<AreaInfo> areaInfo { get; set; }
        public DbSet<Branch> branch { get; set; }
        public DbSet<BranchPaymentMethods> branchPaymentMethods { get; set; }
        public DbSet<PaymentMethods> paymentMethods { get; set; }
        public DbSet<BranchCards> branchCards { get; set; }
        public DbSet<BranchEconomy> branchEconomy { get; set; }
        public DbSet<BranchNotifications> branchNotifications { get; set; }
        public DbSet<BranchStars> branchStars { get; set; }
        public DbSet<BranchWallet> branchWallet { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<CompanySupportRequests> companySupportRequests { get; set; }
        public DbSet<ContactInfo> contactInfo { get; set; }
        public DbSet<DeleteAreaRequests> deleteAreaRequests { get; set; }
        public DbSet<Districts> districts { get; set; }
        public DbSet<MatchHistory> matchHistory { get; set; }
        public DbSet<Regions> regions { get; set; }
        public DbSet<Team> team { get; set; }
        public DbSet<TeamData> teamData { get; set; }
        public DbSet<BranchTransActions> branchTransActions { get; set; }
        public DbSet<DepositTransActions> depositTransActions { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<UserData> userData { get; set; }
        public DbSet<UserFriends> userFriends { get; set; }
        public DbSet<AppointmentTypes> appointmentTypes { get; set; }

    }
}
