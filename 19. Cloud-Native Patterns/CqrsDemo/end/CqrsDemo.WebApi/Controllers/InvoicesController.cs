﻿using CqrsDemo.Core.Commands;
using CqrsDemo.Core.Models.Dto;
using CqrsDemo.Core.Notification;
using CqrsDemo.Core.Queries;
using CqrsDemo.Core.Services.Interfaces;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CqrsDemo.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InvoicesController(IInvoiceService invoiceService, ISender mediatorSender,
    IPublisher mediatorPublisher) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceWithoutItemsDto>>> GetInvoices()
    {
        var invoices = await invoiceService.GetAllListAsync();
        return Ok(invoices);
    }

    [HttpGet]
    [Route("paged")]
    public async Task<ActionResult<IEnumerable<InvoiceWithoutItemsDto>>> GetInvoices(int pageIndex, int pageSize)
    {
        // var invoices = await invoiceService.GetPagedListAsync(page, pageSize);
        // return Ok(invoices);
        
        // decouple the controller from the service layer (controller -> Queries -> Handlers(GetPagedListAsync()))
        var invoices = await mediatorSender.Send(new GetInvoiceListQuery(pageIndex, pageSize));
        return Ok(invoices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDto>> GetInvoice(Guid id)
    {
        // var invoice = await invoiceService.GetAsync(id);
        // return invoice == null ? NotFound() : Ok(invoice);

        // decouple the controller from the service layer (controller -> Queries -> Handlers(GetAsync()))
        var invoice = await mediatorSender.Send(new GetInvoiceByIdQuery(id));
        return invoice == null ? NotFound() : Ok(invoice);
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateOrUpdateInvoiceDto invoiceDto)
    {
        // var invoice = await invoiceService.AddAsync(invoiceDto);
        // return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);

        // decouple the controller from the service layer (controller -> Commands -> Handlers(AddAsync()))
        var invoice = await mediatorSender.Send(new CreateInvoiceCommand(invoiceDto));

        // send(publish) request to multiple handler
        await mediatorPublisher.Publish(new SendInvoiceNotification(invoice.Id));

        return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(Guid id, CreateOrUpdateInvoiceDto invoiceDto)
    {
        try
        {
            await invoiceService.UpdateAsync(id, invoiceDto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        try
        {
            await invoiceService.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        return NoContent();
    }
}
