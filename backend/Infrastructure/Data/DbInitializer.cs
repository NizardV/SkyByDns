using Domain.Entities;
using Faker;

namespace Infrastructure.Data
{
    /// <summary>
    /// Database seeder for initializing test data.
    /// Creates sample users, DNS servers, domains, and DNS records for development and testing.
    /// </summary>
    /// <remarks>
    /// Seeded data includes:
    /// - 10 users (3 predefined admin/test users + 7 randomly generated)
    /// - 5 DNS servers (Google DNS, Cloudflare, Quad9, OpenDNS, etc.)
    /// - Multiple domains per user (1-5 domains each)
    /// - Multiple DNS records per domain (3-15 records of various types)
    /// 
    /// Only runs if database is empty (no existing users).
    /// </remarks>
    public static class DbInitializer
    {
        /// <summary>
        /// Seeds the database with initial test data.
        /// Only executes if the database is empty (no users exist).
        /// </summary>
        /// <param name="context">The application database context.</param>
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Check if database is already seeded
            if (context.Users.Any())
            {
                return;
            }

            // 1. Seed Users (3 predefined + 7 random users)
            var users = new List<User>
            {
                new User
                {
                    FirstName = "admin",
                    LastName = "admin",
                    PasswordHash = "AQAAAAIAAYagAAAAECcuSZMjFdCazAPjxBVlAmCQZWyRqg6fggGoLMXikuXaZ2Fz6SD+kDP1aSDeaGUbvg==",
                    Role = "Admin",
                    Email = "admin@admin.fr",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    FirstName = "user",
                    LastName = "user",
                    PasswordHash = "AQAAAAIAAYagAAAAEPfiLei9vzYZAbxxbd9VpIOZ27gwCrc/MZLfXVNXPtj8DEOWkL9QO4foKnXkuoiP1A==",
                    Role = "User",
                    Email = "user@user.fr",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    FirstName = "plop",
                    LastName = "plop",
                    PasswordHash = "AQAAAAIAAYagAAAAELwP0fHSzY9JOkS1jKuYKGsTt69+2wtm4EKtes/W173q6HyVmZOvdN2p2k4s3YMKTQ==",
                    Role = "User",
                    Email = "plop@plop.fr",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            };

            // Add additional fake users using Faker library
            for (int i = 0; i < 7; i++)
            {
                users.Add(new User
                {
                    FirstName = Name.First(),
                    LastName = Name.Last(),
                    Email = Internet.Email(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                    Role = RandomNumber.Next(0, 10) > 2 ? "User" : "Admin",
                    CreatedAt = DateTime.UtcNow.AddDays(-RandomNumber.Next(1, 365)),
                    UpdatedAt = RandomNumber.Next(0, 10) > 5 ? DateTime.UtcNow : null,
                    IsActive = RandomNumber.Next(0, 10) > 1
                });
            }

            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            // 2. Seed DNS Servers
            var dnsServers = new List<DNSServer>
            {
                new DNSServer
                {
                    IPAddress = "8.8.8.8",
                    URL = "dns.google",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-6)
                },
                new DNSServer
                {
                    IPAddress = "1.1.1.1",
                    URL = "cloudflare-dns.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-5)
                },
                new DNSServer
                {
                    IPAddress = "9.9.9.9",
                    URL = "dns.quad9.net",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-4)
                },
                new DNSServer
                {
                    IPAddress = "208.67.222.222",
                    URL = "dns.opendns.com",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow.AddMonths(-3)
                },
                new DNSServer
                {
                    IPAddress = "8.8.4.4",
                    URL = "dns.google.secondary",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-2)
                }
            };

            context.DNSServers.AddRange(dnsServers);
            await context.SaveChangesAsync();

            // 3. Seed Domains with realistic names
            var domains = new List<Domain.Entities.DomainEntities>();
            var domainExtensions = new[] { ".com", ".net", ".org", ".io", ".fr", ".dev", ".app" };
            var domainAdjectives = new[] { "awesome", "best", "cool", "digital", "easy", "fast", "global", "hyper", "innovative" };
            var domainNouns = new[] { "tech", "shop", "blog", "site", "cloud", "host", "web", "data", "api", "tools" };

            var random = new Random();
            foreach (var user in users)
            {
                // Each user gets 1-5 domains
                var domainCount = RandomNumber.Next(1, 6);
                for (int i = 0; i < domainCount; i++)
                {
                    var adj = domainAdjectives[random.Next(domainAdjectives.Length)];
                    var noun = domainNouns[random.Next(domainNouns.Length)];
                    var ext = domainExtensions[random.Next(domainExtensions.Length)];

                    var createdAt = DateTime.UtcNow.AddDays(-RandomNumber.Next(1, 730));

                    domains.Add(new Domain.Entities.DomainEntities
                    {
                        UserId = user.Id,
                        Name = $"{adj}{noun}{ext}",
                        CreatedAt = createdAt,
                        UpdatedAt = RandomNumber.Next(0, 10) > 3 ? createdAt.AddDays(RandomNumber.Next(1, 180)) : null,
                        IsActive = RandomNumber.Next(0, 10) > 2,
                        User = user
                    });
                }
            }

            context.Domains.AddRange(domains);
            await context.SaveChangesAsync();

            // 4. Seed DNS Records with realistic data
            var records = new List<Record>();
            var recordTypes = new[] { "A", "AAAA", "CNAME", "MX", "TXT", "NS", "SRV", "CAA" };

            foreach (var domain in domains)
            {
                // Each domain gets 3-15 records
                var recordCount = RandomNumber.Next(3, 16);
                for (int i = 0; i < recordCount; i++)
                {
                    var recordType = recordTypes[random.Next(recordTypes.Length)];
                    var createdAt = domain.CreatedAt.AddHours(RandomNumber.Next(1, 48));

                    var record = new Record
                    {
                        DomainId = domain.Id,
                        RecordName = i == 0 ? "@" : GetRandomSubdomain(),
                        RecordType = recordType,
                        TTL = GetRandomTtl(),
                        CreatedAt = createdAt,
                        UpdatedAt = RandomNumber.Next(0, 10) > 6 ? createdAt.AddHours(RandomNumber.Next(1, 720)) : null,
                        DomainEntities = domain
                    };

                    // Set target and priority based on record type
                    switch (recordType)
                    {
                        case "A":
                            record.Target = $"{RandomNumber.Next(1, 255)}.{RandomNumber.Next(0, 255)}.{RandomNumber.Next(0, 255)}.{RandomNumber.Next(1, 255)}";
                            break;
                        case "AAAA":
                            record.Target = $"2001:db8::{RandomNumber.Next(1000, 9999)}:{RandomNumber.Next(1000, 9999)}:{RandomNumber.Next(1000, 9999)}";
                            break;
                        case "CNAME":
                            record.Target = $"www.{domain.Name}";
                            break;
                        case "MX":
                            record.Target = $"mail.{domain.Name}";
                            record.Priority = RandomNumber.Next(1, 50);
                            break;
                        case "TXT":
                            record.Target = $"\"v=spf1 include:{domain.Name} ~all\"";
                            break;
                        case "NS":
                            record.Target = $"ns{RandomNumber.Next(1, 4)}.{domain.Name}";
                            break;
                        case "SRV":
                            record.Target = $"{RandomNumber.Next(0, 65535)} {RandomNumber.Next(0, 65535)} {domain.Name}";
                            record.Priority = RandomNumber.Next(0, 65535);
                            break;
                        case "CAA":
                            record.Target = "0 issue \"letsencrypt.org\"";
                            break;
                        default:
                            record.Target = domain.Name;
                            break;
                    }

                    records.Add(record);
                }
            }

            context.Records.AddRange(records);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Returns a random subdomain name from common subdomain patterns.
        /// </summary>
        private static string GetRandomSubdomain()
        {
            var subdomains = new[] { "www", "mail", "ftp", "blog", "shop", "api", "dev", "test", "staging", "app", "cdn", "static", "assets" };
            return subdomains[new Random().Next(subdomains.Length)];
        }

        /// <summary>
        /// Returns a random TTL (Time To Live) value from common DNS TTL values.
        /// </summary>
        private static int GetRandomTtl()
        {
            var commonTtLs = new[] { 300, 600, 900, 1800, 3600, 7200, 14400, 28800, 43200, 86400 };
            return commonTtLs[new Random().Next(commonTtLs.Length)];
        }
    }
}