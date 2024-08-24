using MediatR;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class DeleteUserHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            
            User user = userRepository.FindById(userId) ?? throw new NotFoundException();
        
            await userRepository.Delete(user);
        }
    }
}
