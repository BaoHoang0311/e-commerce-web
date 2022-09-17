using e_commerce_web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data
{
    public class ProductServices
    {
        private readonly dbMarketsContext _context;
        public ProductServices(dbMarketsContext context)
        {
            _context = context;
        }
        public IQueryable<Product> XL_trang_san_pham(string keySearch, int? CatId, string sortOrder)
        {
            IQueryable<Product> LsProducts = _context.Products
                                                    .AsNoTracking()
                                                    .Include(x => x.Cat)
                                                    .Where(p => p.Active == true)
                                                    .OrderByDescending(x => x.DateCreated);

            if(keySearch != null)
            {
                LsProducts = LsProducts.Where(p => p.ProductName.Contains(keySearch));
            }

            return LsProducts;
        }
    }
}
