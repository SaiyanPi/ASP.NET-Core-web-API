﻿namespace CqrsDemo.Core.Models.Dto;

public class InvoiceDto
{
    public Guid Id { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset InvoiceDate { get; set; }

    public DateTimeOffset DueDate { get; set; }

    public InvoiceStatus Status { get; set; }

    public List<InvoiceItemDto> InvoiceItems { get; set; } = new();
    

    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
}
