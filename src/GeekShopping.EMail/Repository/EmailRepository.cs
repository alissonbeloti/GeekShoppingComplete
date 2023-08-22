using GeekShopping.Email.Model;
using GeekShopping.Email.Messages;
using Microsoft.EntityFrameworkCore;
using GeekShopping.Email.Model.Context;

namespace GeekShopping.Email.Repository;

public class EmailRepository : IEmailRepository
{
    private readonly DbContextOptions<MySqlContext> _context;

    public EmailRepository(DbContextOptions<MySqlContext> context)
    {
        _context = context;
    }

    public async Task LogEmail(UpdatePaymentResultMessage message)
    {
        var emailLog = new EmailLog
        {
            Email = message.Email,
            SentDate = DateTime.Now,
            Log = $"Order - {message.OrderId} has been created succesfully!"
        };

        await using var _db = new MySqlContext(_context);
        _db.EmailLogs.Add(emailLog);
        await _db.SaveChangesAsync();
    }
            
}
