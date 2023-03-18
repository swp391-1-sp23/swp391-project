using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391.Project.Models.Dtos.Image
{
    public class UpdateAccountAvatarDto
    {
        public IFormFile Avatar { get; set; } = null!;
    }
}