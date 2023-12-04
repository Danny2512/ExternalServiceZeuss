using ExternalService.Data.Entities;
using ExternalService.Helpers.Auth;

namespace ExternalService.Data
{
    public class SeedDb
    {
        private readonly IAuthHelper _authHelper;
        private readonly DataContext _context;

        public SeedDb(IAuthHelper authHelper, DataContext context)
        {
            _authHelper = authHelper;
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckUserAsync();
            await CheckCategoriesAsync();
            await CheckProductsAsync();
        }
        private async Task CheckUserAsync()
        {
            if (!_context.tblUser.Any())
            {
                var user_Id1 = Guid.NewGuid();
                var user_Id2 = Guid.NewGuid();

                await _context.tblUser.AddAsync(
                    new User { Id = user_Id1, StrUserName = "zeuss.test", HsPassword = _authHelper.CalculatePasswordHash(user_Id1, "Prueba")}
                );

                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckCategoriesAsync()
        {
            if (!_context.tblCategory.Any())
            {
                await _context.tblCategory.AddRangeAsync(
                    new Category { Id = Guid.NewGuid(), StrName = "Lubircantes", BiActive = true },
                    new Category { Id = Guid.NewGuid(), StrName = "Aseo", BiActive = true }
                );

                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckProductsAsync()
        {
            if (!_context.tblProduct.Any())
            {
                var categoryId1 = _context.tblCategory.First(c => c.StrName == "Lubircantes").Id;
                var categoryId2 = _context.tblCategory.First(c => c.StrName == "Aseo").Id;

                await _context.tblProduct.AddRangeAsync(
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId1, StrName = "Zeuss ecoextreme SAE 5W30", StrImageUrl= "https://i.postimg.cc/9frDvhwg/image.png", DePrice = 30500, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId1, StrName = "Zeuss premium SAE 5W30", StrImageUrl = "https://i.postimg.cc/ncBYPvf9/image.png", DePrice = 50500, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId1, StrName = "Zeuss plus SAE 10W30", StrImageUrl = "https://i.postimg.cc/s2Fm2wPc/image.png", DePrice = 90500, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId1, StrName = "Zeuss protection SAE 20W50", StrImageUrl = "https://i.postimg.cc/HsYsqgf0/image.png", DePrice = 110500, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId1, StrName = "Zeuss heavy duty SAE 50", StrImageUrl = "https://i.postimg.cc/1tK1BgRk/image.png", DePrice = 30500, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId2, StrName = "Super Blue 200ml", StrImageUrl = "https://i.postimg.cc/kg71NjRg/image.png", DePrice = 25400, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId2, StrName = "Cera Simoniz", StrImageUrl = "https://i.postimg.cc/KcNgkJbN/image.png", DePrice = 49400, BiActive = true },                   
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId2, StrName = "Cera autobrillante Simoniz", StrImageUrl = "https://i.postimg.cc/BZV4VKB3/image.png", DePrice = 25400, BiActive = true }
                );

                await _context.SaveChangesAsync();
            }
        }
    }
}
