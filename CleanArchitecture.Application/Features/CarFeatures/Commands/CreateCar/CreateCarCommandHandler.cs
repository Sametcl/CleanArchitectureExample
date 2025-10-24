using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Dtos;
using MediatR;

namespace CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar
{
    public sealed class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, MessageResponse>
    {
        private readonly ICarService _carService;
        private readonly IAwsS3Service awsS3Service;
        public CreateCarCommandHandler(ICarService carService, IAwsS3Service awsS3Service)
        {
            this._carService = carService;
            this.awsS3Service = awsS3Service;
        }

        public async Task<MessageResponse> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            string imageUrl = null;
            if (request.ImageFile != null)
            {
                imageUrl = await awsS3Service.UploadFileAsync(request.ImageFile, "cars");
            }

            await _carService.CreateAsync(request, imageUrl, cancellationToken);

            return new MessageResponse("Arac basariyla uretildi.");
        }
    }
}
