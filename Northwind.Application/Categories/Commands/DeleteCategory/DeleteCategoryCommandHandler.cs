using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using Northwind.Application.Exceptions;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;
using System.Linq;

namespace Northwind.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly INorthwindDbContext _context;

        public DeleteCategoryCommandHandler(INorthwindDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            var hasProducts = _context.Products.Any(p => p.Category.CategoryId == entity.CategoryId);
            if (hasProducts)
            {
                // TODO: Add functional test for this behaviour.
                throw new DeleteFailureException(nameof(Category), request.Id, "There are existing products associated with this category.");
            }

            _context.Categories.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}