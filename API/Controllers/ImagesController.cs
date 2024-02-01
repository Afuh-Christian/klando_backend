

using Model;
using Repository.Repositories;

namespace API.Controllers
{
    public class ImagesController : BaseController<Image>
    {
        public ImagesController(ImageRepository baseRepository) : base(baseRepository)
        {
        }
    }
}