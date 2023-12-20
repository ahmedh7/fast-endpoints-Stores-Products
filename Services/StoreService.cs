using StoreProdFastEndpoints.Context;
using M = StoreProdFastEndpoints.Models;
using StoreProdFastEndpoints.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreProdFastEndpoints.Models;

namespace StoreProdFastEndpoints.Services
{
    public class StoreService
    {
        private readonly StoreContext _context;

        public StoreService(StoreContext context) 
        {   
            _context = context;
        }
        public IEnumerable<object> GetStoresPaginated(PaginationDTO dto)
        {
            if (dto.GetAll)
            {
                var stores = _context.Stores!
                                       .Select(s => new
                                       {
                                           s.Id,
                                           s.Name,
                                           Products = s.Products.Select(p => new { p.Id, p.Name })
                                       }).ToList();
                return stores;
            }
            else
            {
                int pageSkipped = Math.Max(1, dto.Page) - 1;
                int pageSize = Math.Max(1, dto.PageSize);
                string filter = dto.Filter ?? "";
                bool asc = dto.Ascending;
                IQueryable<M.Store> query;
                if (filter.IsNullOrEmpty())
                {
                    if (asc)
                    {
                        query = _context.Stores!
                                    .Skip(pageSize * pageSkipped)
                                    .Take(pageSize)
                                    .OrderBy(s => s.Name).AsQueryable();
                    }
                    else
                    {
                        query = _context.Stores!
                                    .Skip(pageSize * pageSkipped)
                                    .Take(pageSize)
                                    .OrderByDescending(s => s.Name).AsQueryable();
                    }
                }
                else
                {
                    if (asc)
                    {
                        query = _context.Stores!
                                    .Where(s => s.Name.ToLower().Contains(filter.ToLower()))
                                    .Skip(pageSize * pageSkipped)
                                    .Take(pageSize)
                                    .OrderBy(s => s.Name).AsQueryable();
                    }
                    else
                    {
                        query = _context.Stores!
                                    .Where(s => s.Name.ToLower().Contains(filter.ToLower()))
                                    .Skip(pageSize * pageSkipped)
                                    .Take(pageSize)
                                    .OrderByDescending(s => s.Name).AsQueryable();
                    }
                }

                var stores = query
                                       .Select(s => new
                                       {
                                           s.Id,
                                           s.Name,
                                           Products = s.Products.Select(p => new { p.Id, p.Name })
                                       }).ToList();
                return stores;

            }
        }

        public async Task<CreatedStoreDTO> CreateStore(StoreDTO dto, CancellationToken ct)
        {
            M.Store newStore = new()
            {
                Name = dto.Name,

            };
            List<int> invalidProductIDs = new();
            foreach (var productId in dto.ProductIDs)
            {
                var product = await _context.Products!
                                    .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken: ct);

                if (product != null)
                {
                    newStore.Products.Add(product);
                }
                else { invalidProductIDs.Add(productId); }
            }
            _context.Stores!.Add(newStore);
            await _context.SaveChangesAsync(ct);
            return new() 
            { 
                InvalidProductIDs = invalidProductIDs,
                Store = newStore
            };
        }

        public async Task<List<int>> UpdateStore(StoreDTO dto, int id, CancellationToken ct)
        {
            var existingStore = _context.Stores!.Include(s => s.Products)
                                    .FirstOrDefault(s => s.Id == id) ?? throw new Exception($"No store found with ID = {id}");

            existingStore!.Name = dto.Name;

            var updatedProductIds = dto.ProductIDs;

            existingStore.Products.Clear();
            List<int> invalidProductIDs = new();
            foreach (var productId in updatedProductIds)
            {
                var product = await _context.Products!
                                    .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken: ct);

                if (product != null)
                {
                    existingStore.Products.Add(product);
                }
                else { invalidProductIDs.Add(productId); }
            }


            await _context.SaveChangesAsync(ct);
            return invalidProductIDs;
        }
    
        public async Task<bool> DeleteStore(int id, CancellationToken ct)
        {
            M.Store? store = await _context.Stores!.FirstOrDefaultAsync(s => s.Id == id, cancellationToken: ct) ?? throw new($"No Store found with id = {id}");
            _context.Stores!.Remove(store!);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    
    
    }
}
