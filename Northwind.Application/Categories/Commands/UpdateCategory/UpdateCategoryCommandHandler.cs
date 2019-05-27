using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Northwind.Application.Exceptions;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;

namespace Northwind.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly INorthwindDbContext _context;

        public UpdateCategoryCommandHandler(INorthwindDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories.FindAsync(request.CategoryId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            entity.CategoryName = request.CategoryName;
            entity.Description = request.Description;
            entity.Picture = request.Picture;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}