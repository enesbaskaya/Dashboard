using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dashboard.Migrations
{
    public partial class migration_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    adminId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    mail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin", x => x.adminId);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    regionId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    regionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.regionId);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    cityId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cityName = table.Column<string>(nullable: true),
                    regionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.cityId);
                    table.ForeignKey(
                        name: "FK_city_regions_regionId",
                        column: x => x.regionId,
                        principalTable: "regions",
                        principalColumn: "regionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    districtId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    districtName = table.Column<string>(nullable: true),
                    cityId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_districts", x => x.districtId);
                    table.ForeignKey(
                        name: "FK_districts_city_cityId",
                        column: x => x.cityId,
                        principalTable: "city",
                        principalColumn: "cityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contactInfo",
                columns: table => new
                {
                    contactId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    telephone = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    mail = table.Column<string>(nullable: true),
                    districtId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactInfo", x => x.contactId);
                    table.ForeignKey(
                        name: "FK_contactInfo_districts_districtId",
                        column: x => x.districtId,
                        principalTable: "districts",
                        principalColumn: "districtId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "branch",
                columns: table => new
                {
                    branchId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    admin = table.Column<string>(nullable: true),
                    taxNumber = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    identityNumber = table.Column<string>(nullable: true),
                    openDays = table.Column<string>(nullable: true),
                    shoesRent = table.Column<bool>(nullable: false),
                    market = table.Column<bool>(nullable: false),
                    catering = table.Column<bool>(nullable: false),
                    shower = table.Column<bool>(nullable: false),
                    paymentMethods = table.Column<string>(nullable: true),
                    branchType = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    registerDate = table.Column<string>(nullable: true),
                    contactId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branch", x => x.branchId);
                    table.ForeignKey(
                        name: "FK_branch_contactInfo_contactId",
                        column: x => x.contactId,
                        principalTable: "contactInfo",
                        principalColumn: "contactId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    userId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    dateOfBirth = table.Column<string>(nullable: true),
                    position = table.Column<string>(nullable: true),
                    footType = table.Column<string>(nullable: true),
                    height = table.Column<int>(nullable: false),
                    weight = table.Column<int>(nullable: false),
                    contactId = table.Column<long>(nullable: false),
                    registerDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.userId);
                    table.ForeignKey(
                        name: "FK_user_contactInfo_contactId",
                        column: x => x.contactId,
                        principalTable: "contactInfo",
                        principalColumn: "contactId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "areaInfo",
                columns: table => new
                {
                    areaId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    areaName = table.Column<string>(nullable: true),
                    ground = table.Column<string>(nullable: true),
                    heightWidth = table.Column<string>(nullable: true),
                    maxPlayer = table.Column<string>(nullable: true),
                    openCloseTime = table.Column<string>(nullable: true),
                    recordingMatch = table.Column<bool>(nullable: false),
                    price = table.Column<double>(nullable: false),
                    time = table.Column<string>(nullable: true),
                    roof = table.Column<bool>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    registerDate = table.Column<string>(nullable: true),
                    branchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areaInfo", x => x.areaId);
                    table.ForeignKey(
                        name: "FK_areaInfo_branch_branchId",
                        column: x => x.branchId,
                        principalTable: "branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "branchCards",
                columns: table => new
                {
                    cardId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    iban = table.Column<string>(nullable: true),
                    cardOwner = table.Column<string>(nullable: true),
                    bankName = table.Column<string>(nullable: true),
                    branchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branchCards", x => x.cardId);
                    table.ForeignKey(
                        name: "FK_branchCards_branch_branchId",
                        column: x => x.branchId,
                        principalTable: "branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "branchEconomy",
                columns: table => new
                {
                    branchEconomyId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    year = table.Column<string>(nullable: true),
                    month = table.Column<string>(nullable: true),
                    giro = table.Column<double>(nullable: false),
                    branchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branchEconomy", x => x.branchEconomyId);
                    table.ForeignKey(
                        name: "FK_branchEconomy_branch_branchId",
                        column: x => x.branchId,
                        principalTable: "branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "branchNotifications",
                columns: table => new
                {
                    notificationId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    content = table.Column<string>(nullable: true),
                    header = table.Column<string>(nullable: true),
                    sender = table.Column<string>(nullable: true),
                    isRead = table.Column<bool>(nullable: false),
                    date = table.Column<string>(nullable: true),
                    branchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branchNotifications", x => x.notificationId);
                    table.ForeignKey(
                        name: "FK_branchNotifications_branch_branchId",
                        column: x => x.branchId,
                        principalTable: "branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "branchStars",
                columns: table => new
                {
                    starId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    branchId = table.Column<long>(nullable: false),
                    point = table.Column<int>(nullable: false),
                    year = table.Column<string>(nullable: true),
                    count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branchStars", x => x.starId);
                    table.ForeignKey(
                        name: "FK_branchStars_branch_branchId",
                        column: x => x.branchId,
                        principalTable: "branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "branchWallet",
                columns: table => new
                {
                    walletId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    balance = table.Column<double>(nullable: false),
                    debt = table.Column<double>(nullable: false),
                    branchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branchWallet", x => x.walletId);
                    table.ForeignKey(
                        name: "FK_branchWallet_branch_branchId",
                        column: x => x.branchId,
                        principalTable: "branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "companySupportRequests",
                columns: table => new
                {
                    requestId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    header = table.Column<string>(nullable: true),
                    content = table.Column<string>(nullable: true),
                    date = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    branchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companySupportRequests", x => x.requestId);
                    table.ForeignKey(
                        name: "FK_companySupportRequests_branch_branchId",
                        column: x => x.branchId,
                        principalTable: "branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "depositTransActions",
                columns: table => new
                {
                    transId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date = table.Column<string>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    checkActive = table.Column<bool>(nullable: false),
                    branchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_depositTransActions", x => x.transId);
                    table.ForeignKey(
                        name: "FK_depositTransActions_branch_branchId",
                        column: x => x.branchId,
                        principalTable: "branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    teamId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    shortName = table.Column<string>(nullable: true),
                    players = table.Column<string>(nullable: true),
                    avatarPath = table.Column<string>(nullable: true),
                    userId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team", x => x.teamId);
                    table.ForeignKey(
                        name: "FK_team_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userData",
                columns: table => new
                {
                    dataId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    wins = table.Column<int>(nullable: false),
                    loses = table.Column<int>(nullable: false),
                    draws = table.Column<int>(nullable: false),
                    userRateCounter = table.Column<int>(nullable: false),
                    ratePoints = table.Column<int>(nullable: false),
                    year = table.Column<string>(nullable: true),
                    userId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userData", x => x.dataId);
                    table.ForeignKey(
                        name: "FK_userData_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userFriends",
                columns: table => new
                {
                    userFriendId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    friendId = table.Column<long>(nullable: false),
                    userId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userFriends", x => x.userFriendId);
                    table.ForeignKey(
                        name: "FK_userFriends_user_friendId",
                        column: x => x.friendId,
                        principalTable: "user",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userFriends_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deleteAreaRequests",
                columns: table => new
                {
                    deleteRequestId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    areaId = table.Column<long>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deleteAreaRequests", x => x.deleteRequestId);
                    table.ForeignKey(
                        name: "FK_deleteAreaRequests_areaInfo_areaId",
                        column: x => x.areaId,
                        principalTable: "areaInfo",
                        principalColumn: "areaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "branchTransActions",
                columns: table => new
                {
                    transId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cardId = table.Column<long>(nullable: false),
                    date = table.Column<string>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    checkActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branchTransActions", x => x.transId);
                    table.ForeignKey(
                        name: "FK_branchTransActions_branchCards_cardId",
                        column: x => x.cardId,
                        principalTable: "branchCards",
                        principalColumn: "cardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "matchHistory",
                columns: table => new
                {
                    matchId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    winnerTeam = table.Column<long>(nullable: false),
                    winnerteamId = table.Column<long>(nullable: true),
                    loserTeam = table.Column<long>(nullable: false),
                    loserteamId = table.Column<long>(nullable: true),
                    areaId = table.Column<long>(nullable: false),
                    date = table.Column<string>(nullable: true),
                    score = table.Column<string>(nullable: true),
                    clock = table.Column<double>(nullable: false),
                    appointmentType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matchHistory", x => x.matchId);
                    table.ForeignKey(
                        name: "FK_matchHistory_areaInfo_areaId",
                        column: x => x.areaId,
                        principalTable: "areaInfo",
                        principalColumn: "areaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_matchHistory_team_loserteamId",
                        column: x => x.loserteamId,
                        principalTable: "team",
                        principalColumn: "teamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_matchHistory_team_winnerteamId",
                        column: x => x.winnerteamId,
                        principalTable: "team",
                        principalColumn: "teamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "branchMatchWorth",
                columns: table => new
                {
                    worthId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    amount = table.Column<double>(nullable: false),
                    paymentType = table.Column<string>(nullable: true),
                    matchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branchMatchWorth", x => x.worthId);
                    table.ForeignKey(
                        name: "FK_branchMatchWorth_matchHistory_matchId",
                        column: x => x.matchId,
                        principalTable: "matchHistory",
                        principalColumn: "matchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_areaInfo_branchId",
                table: "areaInfo",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_branch_contactId",
                table: "branch",
                column: "contactId");

            migrationBuilder.CreateIndex(
                name: "IX_branchCards_branchId",
                table: "branchCards",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_branchEconomy_branchId",
                table: "branchEconomy",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_branchMatchWorth_matchId",
                table: "branchMatchWorth",
                column: "matchId");

            migrationBuilder.CreateIndex(
                name: "IX_branchNotifications_branchId",
                table: "branchNotifications",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_branchStars_branchId",
                table: "branchStars",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_branchTransActions_cardId",
                table: "branchTransActions",
                column: "cardId");

            migrationBuilder.CreateIndex(
                name: "IX_branchWallet_branchId",
                table: "branchWallet",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_city_regionId",
                table: "city",
                column: "regionId");

            migrationBuilder.CreateIndex(
                name: "IX_companySupportRequests_branchId",
                table: "companySupportRequests",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_contactInfo_districtId",
                table: "contactInfo",
                column: "districtId");

            migrationBuilder.CreateIndex(
                name: "IX_deleteAreaRequests_areaId",
                table: "deleteAreaRequests",
                column: "areaId");

            migrationBuilder.CreateIndex(
                name: "IX_depositTransActions_branchId",
                table: "depositTransActions",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_districts_cityId",
                table: "districts",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_matchHistory_areaId",
                table: "matchHistory",
                column: "areaId");

            migrationBuilder.CreateIndex(
                name: "IX_matchHistory_loserteamId",
                table: "matchHistory",
                column: "loserteamId");

            migrationBuilder.CreateIndex(
                name: "IX_matchHistory_winnerteamId",
                table: "matchHistory",
                column: "winnerteamId");

            migrationBuilder.CreateIndex(
                name: "IX_team_userId",
                table: "team",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_user_contactId",
                table: "user",
                column: "contactId");

            migrationBuilder.CreateIndex(
                name: "IX_userData_userId",
                table: "userData",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_userFriends_friendId",
                table: "userFriends",
                column: "friendId");

            migrationBuilder.CreateIndex(
                name: "IX_userFriends_userId",
                table: "userFriends",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropTable(
                name: "branchEconomy");

            migrationBuilder.DropTable(
                name: "branchMatchWorth");

            migrationBuilder.DropTable(
                name: "branchNotifications");

            migrationBuilder.DropTable(
                name: "branchStars");

            migrationBuilder.DropTable(
                name: "branchTransActions");

            migrationBuilder.DropTable(
                name: "branchWallet");

            migrationBuilder.DropTable(
                name: "companySupportRequests");

            migrationBuilder.DropTable(
                name: "deleteAreaRequests");

            migrationBuilder.DropTable(
                name: "depositTransActions");

            migrationBuilder.DropTable(
                name: "userData");

            migrationBuilder.DropTable(
                name: "userFriends");

            migrationBuilder.DropTable(
                name: "matchHistory");

            migrationBuilder.DropTable(
                name: "branchCards");

            migrationBuilder.DropTable(
                name: "areaInfo");

            migrationBuilder.DropTable(
                name: "team");

            migrationBuilder.DropTable(
                name: "branch");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "contactInfo");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "regions");
        }
    }
}
