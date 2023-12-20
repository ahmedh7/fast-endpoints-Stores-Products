using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreProdFastEndpoints.Context;
using StoreProdFastEndpoints.DTOs;
using System.Collections;
using System.Collections.Generic;
using M = StoreProdFastEndpoints.Models;

namespace StoreProdFastEndpoints.Services
{
    public class ProductService
    {
        private readonly StoreContext _context;
        public ProductService(StoreContext context)
        {
            _context = context;
        }

        public IEnumerable<object> GetProductsPaginated(PaginationDTO dto)
        {
            if (dto.GetAll)
            {
                IQueryable<M.Product> query;
                bool asc = dto.Ascending;
                if (asc)
                {
                    query = _context.Products!
                                .OrderBy(s => s.Name).AsQueryable();
                }
                else
                {
                    query = _context.Products!
                                .OrderByDescending(s => s.Name).AsQueryable();
                }
                var prods = query
                                .Select(p => new
                                {
                                    p.Id,
                                    p.Name,
                                    Stores = p.Stores.Select(s => new { s.Id, s.Name })
                                })
                                .ToList();
                return prods;
            }
            else
            {
                int pageSkipped = Math.Max(1, dto.Page) - 1;
                int pageSize = Math.Max(1, dto.PageSize);
                string filter = dto.Filter ?? "";
                bool asc = dto.Ascending;
                IQueryable<M.Product> query;
                if (filter.IsNullOrEmpty())
                {
                    if (asc)
                    {
                        query = _context.Products!
                                    .Skip(pageSize * pageSkipped)
                                    .Take(pageSize)
                                    .OrderBy(s => s.Name).AsQueryable();
                    }
                    else
                    {
                        query = _context.Products!
                                    .Skip(pageSize * pageSkipped)
                                    .Take(pageSize)
                                    .OrderByDescending(s => s.Name).AsQueryable();
                    }
                }
                else
                {
                    if (asc)
                    {
                        query = _context.Products!
                                    .Where(s => s.Name.ToLower().Contains(filter.ToLower()))
                                    .Skip(pageSize * pageSkipped)
                                    .Take(pageSize)
                                    .OrderBy(s => s.Name).AsQueryable();
                    }
                    else
                    {
                        query = _context.Products!
                                    .Where(s => s.Name.ToLower().Contains(filter.ToLower()))
                                    .Skip(pageSize * pageSkipped)
                                    .Take(pageSize)
                                    .OrderByDescending(s => s.Name).AsQueryable();
                    }
                }

                var prods = query
                                .Select(p => new
                                {
                                    p.Id,
                                    p.Name,
                                    Stores = p.Stores.Select(s => new { s.Id, s.Name })
                                })
                                .ToList();
                return prods;

            }
        }

        public async Task<CreatedProductDTO> CreateProduct(ProductDTO dto, CancellationToken ct)
        {
            M.Product newProd = new()
            {
                Name = dto.Name,

            };
            List<int> invalidStoreIDs = new();
            foreach (var storeID in dto.StoreIDs)
            {
                var store = await _context.Stores!
                                    .FirstOrDefaultAsync(s => s.Id == storeID, cancellationToken: ct);

                if (store != null)
                {
                    newProd.Stores.Add(store);
                }
                else { invalidStoreIDs.Add(storeID); }
            }
            _context.Products!.Add(newProd);
            await _context.SaveChangesAsync(ct);
            return new()
            {
                InvalidStoreIDs = invalidStoreIDs,
                Product = newProd
            };
        }

        public async Task<List<int>> UpdateProduct(ProductDTO dto, int id, CancellationToken ct)
        {
            var existingProduct = _context.Products!.Include(p => p.Stores)
                                    .FirstOrDefault(p => p.Id == id) ?? throw new Exception($"No product found with ID = {id}");
            existingProduct!.Name = dto.Name;

            var updatedStoreIds = dto.StoreIDs;

            existingProduct.Stores.Clear();
            List<int> invalidStoretIDs = new();
            foreach (var storeID in updatedStoreIds)
            {
                var store = await _context.Stores!
                                    .FirstOrDefaultAsync(s => s.Id == storeID, cancellationToken: ct);

                if (store != null)
                {
                    existingProduct.Stores.Add(store);
                }
                else { invalidStoretIDs.Add(storeID); }
            }


            await _context.SaveChangesAsync(ct);
            return invalidStoretIDs;
        }

        public async Task<bool> DeleteProduct(int id, CancellationToken ct)
        {
            M.Product? prod = await _context.Products!.FirstOrDefaultAsync(p => p.Id == id, cancellationToken: ct) ?? throw new($"No product found with id = {id}");

            _context.Products!.Remove(prod!);
            await _context.SaveChangesAsync(ct);
            return true;
        }

    }
}
