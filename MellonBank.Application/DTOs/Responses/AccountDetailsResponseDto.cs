using MellonBank.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MellonBank.Application.DTOs.Responses
{
    public record AccountDetailsResponseDto(
        Guid Id,
        string AccountNumber,
        decimal Balance,
        CurrencyType Currency,
        string Branch,
        AccountType AccountType
    );
}
