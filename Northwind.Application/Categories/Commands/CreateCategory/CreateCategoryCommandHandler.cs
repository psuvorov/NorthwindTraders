using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;

namespace Northwind.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly INorthwindDbContext _context;

        public CreateCategoryCommandHandler(INorthwindDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Category
            {
                CategoryName = request.CategoryName,
                Description = request.Description,
                Picture = request.Picture
            };

            _context.Categories.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.CategoryId;
        }
    }
}