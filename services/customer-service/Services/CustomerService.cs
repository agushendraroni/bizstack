using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CustomerService.Data;
using CustomerService.DTOs;
using CustomerService.Models;
using SharedLibrary.DTOs;

namespace CustomerService.Services;

public class CustomerService : ICustomerService
{
    private readonly CustomerDbContext _context;
    private readonly IMapper _mapper;

    public CustomerService(CustomerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerDto>>> GetAllCustomersAsync(int? tenantId = null)
    {
        var query = _context.Customers.Where(x => !x.IsDeleted)
            .Include(c => c.CustomerGroup)
            .Where(c => c.IsActive);
            
        if (tenantId.HasValue)
            query = query.Where(c => c.TenantId == tenantId.Value);
            
        var customers = await query.ToListAsync();
        var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
        return ApiResponse<List<CustomerDto>>.Success(customerDtos);
    }

    public async Task<ApiResponse<CustomerDto>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _context.Customers.Where(x => !x.IsDeleted)
            .Include(c => c.CustomerGroup)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null)
            return ApiResponse<CustomerDto>.Error("Customer not found");

        var customerDto = _mapper.Map<CustomerDto>(customer);
        return ApiResponse<CustomerDto>.Success(customerDto);
    }

    public async Task<ApiResponse<List<CustomerDto>>> GetCustomersByGroupAsync(Guid groupId)
    {
        var customers = await _context.Customers.Where(x => !x.IsDeleted)
            .Include(c => c.CustomerGroup)
            .Where(c => c.CustomerGroupId == groupId && c.IsActive)
            .ToListAsync();

        var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
        return ApiResponse<List<CustomerDto>>.Success(customerDtos);
    }

    public async Task<ApiResponse<List<CustomerDto>>> SearchCustomersAsync(string searchTerm)
    {
        var customers = await _context.Customers.Where(x => !x.IsDeleted)
            .Include(c => c.CustomerGroup)
            .Where(c => c.IsActive && 
                       (c.Name.Contains(searchTerm) || 
                        (c.Email != null && c.Email.Contains(searchTerm)) ||
                        (c.Phone != null && c.Phone.Contains(searchTerm))))
            .ToListAsync();

        var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
        return ApiResponse<List<CustomerDto>>.Success(customerDtos);
    }

    public async Task<ApiResponse<List<CustomerDto>>> GetVipCustomersAsync()
    {
        var customers = await _context.Customers.Where(x => !x.IsDeleted)
            .Include(c => c.CustomerGroup)
            .Where(c => c.IsActive && (c.CustomerType == "VIP" || c.TotalSpent >= 10000))
            .OrderByDescending(c => c.TotalSpent)
            .ToListAsync();

        var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
        return ApiResponse<List<CustomerDto>>.Success(customerDtos);
    }

    public async Task<ApiResponse<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto, int? tenantId = null)
    {
        var customer = _mapper.Map<Customer>(createCustomerDto);
        customer.TenantId = tenantId;
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var customerDto = _mapper.Map<CustomerDto>(customer);
        return ApiResponse<CustomerDto>.Success(customerDto);
    }

    public async Task<ApiResponse<CustomerDto>> UpdateCustomerAsync(Guid id, UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return ApiResponse<CustomerDto>.Error("Customer not found");

        _mapper.Map(updateCustomerDto, customer);
        await _context.SaveChangesAsync();

        var customerDto = _mapper.Map<CustomerDto>(customer);
        return ApiResponse<CustomerDto>.Success(customerDto);
    }

    public async Task<ApiResponse<string>> DeleteCustomerAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return ApiResponse<string>.Error("Customer not found");

        customer.IsActive = false;
        await _context.SaveChangesAsync();

        return ApiResponse<string>.Success("Customer deleted successfully");
    }

    public async Task<ApiResponse<string>> UpdateCustomerStatsAsync(Guid id, decimal orderAmount)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return ApiResponse<string>.Error("Customer not found");

        customer.TotalSpent += orderAmount;
        customer.TotalOrders += 1;
        customer.LastOrderDate = DateTime.UtcNow;

        // Auto-upgrade to VIP if spent > 10000
        if (customer.TotalSpent >= 10000 && customer.CustomerType == "Regular")
        {
            customer.CustomerType = "VIP";
        }

        await _context.SaveChangesAsync();

        return ApiResponse<string>.Success($"Customer stats updated. Total spent: {customer.TotalSpent:C}");
    }
}
