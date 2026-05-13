using CafeteriaAPI.DTOs;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;
using CafeteriaAPI.Services.Interfaces;

namespace CafeteriaAPI.Services;

public class ClienteService(IClienteRepository repo) : IClienteService
{
    public async Task<IEnumerable<ClienteResponseDto>> GetAllAsync()
    {
        var clientes = await repo.GetAllAsync();
        return clientes.Select(ToDto);
    }

    public async Task<ClienteResponseDto?> GetByIdAsync(int id)
    {
        var cliente = await repo.GetByIdAsync(id);
        return cliente is null ? null : ToDto(cliente);
    }

    public async Task<ClienteResponseDto> CreateAsync(ClienteCreateDto dto)
    {
        var cliente = new Cliente
        {
            Nome          = dto.Nome.Trim(),
            Email         = dto.Email.Trim(),
            Telefone      = dto.Telefone?.Trim(),
            Cpf           = dto.Cpf?.Trim(),
            DataCadastro  = DateOnly.FromDateTime(DateTime.Today),
            Ativo         = true
        };
        var criado = await repo.CreateAsync(cliente);
        return ToDto(criado);
    }

    public async Task<ClienteResponseDto?> UpdateAsync(int id, ClienteCreateDto dto)
    {
        var atualizado = new Cliente
        {
            Nome     = dto.Nome.Trim(),
            Email    = dto.Email.Trim(),
            Telefone = dto.Telefone?.Trim(),
            Cpf      = dto.Cpf?.Trim(),
            Ativo    = true
        };
        var cliente = await repo.UpdateAsync(id, atualizado);
        return cliente is null ? null : ToDto(cliente);
    }

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static ClienteResponseDto ToDto(Cliente c) => new()
    {
        IdCliente    = c.IdCliente,
        Nome         = c.Nome,
        Email        = c.Email,
        Telefone     = c.Telefone,
        Cpf          = c.Cpf,
        DataCadastro = c.DataCadastro.ToString("dd/MM/yyyy"),
        Ativo        = c.Ativo
    };
}
